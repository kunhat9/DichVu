USE DB_SERVICES
GO
IF OBJECT_ID('SP_InsertMenu', 'P') IS NOT NULL
DROP PROC SP_InsertMenu
GO
CREATE PROCEDURE SP_InsertMenu
(
	@xml NVARCHAR(MAX)
) AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @ecode VARCHAR(50) = '00'
	DECLARE @x XML = @xml
	DECLARE @i int, @j int, @menuId int, @groupId int
	DECLARE @tblGroup TABLE (Id int)
	-- Lấy menuId, kiểm tra tồn tại
	SELECT @menuId = COALESCE([T].[C].value('MenuId[1]', 'int'), 0)
		FROM @x.nodes('/Root/Menu') as [T]([C])
	IF @menuId > 0 AND EXISTS (SELECT * FROM TB_MENUS M WHERE M.MenuId = @menuId)
	BEGIN	-- Update (Đã tồn tại)
		UPDATE M
		SET M.MenuName = V.MenuName, M.MenuPrice = V.MenuPrice, M.MenuNum = V.MenuNum, M.MenuNote = V.MenuNote, M.MenuStatus = V.MenuStatus
			--, M.MenuServiceId = V.MenuServiceId
		FROM [dbo].[TB_MENUS] M
			JOIN (SELECT COALESCE([T].[C].value('MenuId[1]', 'int'), 0) MenuId
					, [T].[C].value('MenuName[1]', 'nvarchar(50)') MenuName
					, COALESCE([T].[C].value('MenuPrice[1]', 'decimal'), 0) MenuPrice
					, COALESCE([T].[C].value('MenuNum[1]', 'int'), 0) MenuNum
					, [T].[C].value('MenuNote[1]', 'nvarchar(MAX)') MenuNote
					, [T].[C].value('MenuStatus[1]', 'varchar(20)') MenuStatus
					--, COALESCE([T].[C].value('MenuServiceId[1]', 'int'), 0) MenuServiceId
				FROM @x.nodes('/Root/Menu') as [T]([C])) V
			ON M.MenuId = V.MenuId
		WHERE M.MenuId = @menuId
		-- Detail đã có
		INSERT INTO @tblGroup(Id)
		SELECT G.MgroupId FROM [dbo].[TB_MENU_GROUPS] G WHERE G.MgroupMenuId = @menuId
		-- Thêm hoặc cập nhật detail
		SET @i = @x.value('count(/Root/Groups/Group/GroupName/text())','nvarchar(50)')
		SET @j = 1
		WHILE @j <= @i
		BEGIN
			-- Lấy groupId, kiểm tra tồn tại
			SELECT @groupId = COALESCE([T].[C].value('GroupId[1]', 'int'), 0)
				FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]') as [T]([C])
			IF @groupId > 0 AND EXISTS (SELECT * FROM TB_MENU_GROUPS G WHERE G.MgroupId = @groupId)
			BEGIN	-- Update (Đã tồn tại)
				DELETE G FROM @tblGroup G WHERE G.Id = @groupId
				UPDATE G
				SET G.MgroupName = V.GroupName
				FROM [dbo].[TB_MENU_GROUPS] G
					JOIN (SELECT COALESCE([T].[C].value('GroupId[1]', 'int'), 0) GroupId
							, [T].[C].value('GroupName[1]', 'nvarchar(50)') GroupName
						FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]') as [T]([C])) V
					ON G.MgroupId = V.GroupId
				WHERE G.MgroupMenuId = @menuId
				-- Xóa giá trị detail cũ (nếu có)
				DELETE D
				FROM [dbo].[TB_MENU_DETAILS] D
				WHERE D.MdetailMgroupId = @groupId
					AND D.MdetailId NOT IN (
							SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
							FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]/Rows/Detail') as [T]([C])
						)
				-- Cập nhật Detail
				UPDATE D
				SET D.MdetailName = V.DetailName
				FROM [dbo].[TB_MENU_DETAILS] D
					JOIN (SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
							, [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
						FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]/Rows/Detail') as [T]([C])) V
					ON D.MdetailId = V.DetailId
				WHERE D.MdetailMgroupId = @groupId
				-- Thêm Detail
				INSERT INTO [dbo].[TB_MENU_DETAILS](MdetailName, MdetailMgroupId)
				SELECT V.DetailName, @groupId FROM
					(SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
						, [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
					FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]/Rows/Detail') as [T]([C])) V
				WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TB_MENU_DETAILS] D WHERE D.MdetailId = V.DetailId AND D.MdetailMgroupId = @groupId)
			END
			ELSE
			BEGIN	-- Thêm mới
				INSERT INTO [dbo].[TB_MENU_GROUPS](MgroupName, MgroupSelected, MgroupMenuId)
				SELECT [T].[C].value('GroupName[1]', 'nvarchar(50)')
					, 1
					, @menuId
					FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]') as [T]([C])
				SET @groupId = @@IDENTITY
				INSERT INTO [dbo].[TB_MENU_DETAILS](MdetailName, MdetailMgroupId)
				SELECT [T].[C].value('DetailName[1]', 'nvarchar(50)')
					, @groupId
					FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]/Rows/Detail') as [T]([C])
			END
			SET @j += 1
		END
		-- Xóa detail
		DELETE D FROM [dbo].[TB_MENU_DETAILS] D WHERE D.MdetailMgroupId IN (SELECT Id FROM @tblGroup)
		DELETE G FROM [dbo].[TB_MENU_GROUPS] G WHERE G.MgroupId IN (SELECT Id FROM @tblGroup)
	END
	ELSE
	BEGIN	-- Thêm mới
		-- Thêm Menu
		INSERT INTO [dbo].[TB_MENUS](MenuName, MenuPrice, MenuNum, MenuNote, MenuStatus, MenuServiceId)
		SELECT [T].[C].value('MenuName[1]', 'nvarchar(50)')
			, COALESCE([T].[C].value('MenuPrice[1]', 'decimal'), 0)
			, COALESCE([T].[C].value('MenuNum[1]', 'int'), 0)
			, [T].[C].value('MenuNote[1]', 'nvarchar(MAX)')
			, [T].[C].value('MenuStatus[1]', 'varchar(20)')
			, COALESCE([T].[C].value('MenuServiceId[1]', 'int'), 0)
		FROM @x.nodes('/Root/Menu') as [T]([C])
		SET @menuId = @@IDENTITY
		-- Thêm Detail
		SET @i = @x.value('count(/Root/Groups/Group/GroupName/text())','nvarchar(50)')
		SET @j = 1
		WHILE @j <= @i
		BEGIN
			INSERT INTO [dbo].[TB_MENU_GROUPS](MgroupName, MgroupSelected, MgroupMenuId)
			SELECT [T].[C].value('GroupName[1]', 'nvarchar(50)')
				, 1
				, @menuId
				FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]') as [T]([C])
			SET @groupId = @@IDENTITY
			INSERT INTO [dbo].[TB_MENU_DETAILS](MdetailName, MdetailMgroupId)
			SELECT [T].[C].value('DetailName[1]', 'nvarchar(50)')
				, @groupId
				FROM @x.nodes('/Root/Groups/Group[sql:variable("@j")]/Rows/Detail') as [T]([C])
			SET @j += 1
		END
	END
	SELECT @ecode ecode, @menuId edesc
COMMIT
END TRY
BEGIN CATCH
   ROLLBACK
   SET @ecode = '999'
   SELECT '999' ecode, ERROR_MESSAGE() edesc
END CATCH