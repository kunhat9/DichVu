USE DB_SERVICES
GO
IF OBJECT_ID('SP_InsertTypeDetail', 'P') IS NOT NULL
DROP PROC SP_InsertTypeDetail
GO
CREATE PROCEDURE SP_InsertTypeDetail
(
	@xml NVARCHAR(MAX)
) AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @ecode VARCHAR(50) = '00'
	DECLARE @x XML = @xml
	-- Kiểm tra tồn tại
	DECLARE @typeCode varchar(10)
	SELECT @typeCode = [T].[C].value('TypeCode[1]', 'varchar(10)')
		FROM @x.nodes('/Root/Type') as [T]([C])
	IF EXISTS (SELECT * FROM TB_TYPES T WHERE T.TypeCode = @typeCode)
	BEGIN	-- Update (Đã tồn tại)
		-- Cập nhật Type
		UPDATE T
		SET T.TypeName = V.TypeName, T.TypeStatus = V.TypeStatus, T.TypeType = V.TypeType
		FROM [dbo].[TB_TYPES] T
			JOIN (SELECT [T].[C].value('TypeCode[1]', 'varchar(10)') TypeCode
					, [T].[C].value('TypeName[1]', 'nvarchar(50)') TypeName
					, [T].[C].value('TypeStatus[1]', 'varchar(20)') TypeStatus
					, [T].[C].value('TypeType[1]', 'varchar(20)') TypeType
				FROM @x.nodes('/Root/Type') as [T]([C])) V
			ON T.TypeCode = V.TypeCode
		WHERE T.TypeCode = @typeCode
		-- Xóa giá trị detail cũ (nếu có)
		DELETE D
		FROM [dbo].[TB_TYPE_DETAILS] D
		WHERE D.TypeDetailTypeCode = @typeCode
			AND D.TypeDetailId NOT IN (
					SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
					FROM @x.nodes('/Root/Rows/Detail') as [T]([C])
				)
		-- Cập nhật Detail
		UPDATE D
		SET D.TypeDetailName = V.DetailName, D.TypeDetailType = V.DetailType
		FROM [dbo].[TB_TYPE_DETAILS] D
			JOIN (SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
					, [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
					, [T].[C].value('DetailType[1]', 'varchar(50)') DetailType
				FROM @x.nodes('/Root/Rows/Detail') as [T]([C])) V
			ON D.TypeDetailId = V.DetailId
		WHERE D.TypeDetailTypeCode = @typeCode
		-- Thêm Detail
		INSERT INTO [dbo].[TB_TYPE_DETAILS](TypeDetailName, TypeDetailType, TypeDetailTypeCode)
		SELECT DetailName, DetailType, @typeCode FROM
			(SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
				, [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
				, [T].[C].value('DetailType[1]', 'varchar(50)') DetailType
			FROM @x.nodes('/Root/Rows/Detail') as [T]([C])) V
		WHERE V.DetailId = 0
	END
	ELSE
	BEGIN	-- Thêm mới
		-- Thêm Type
		INSERT INTO [dbo].[TB_TYPES](TypeCode, TypeName, TypeStatus, TypeType)
		SELECT [T].[C].value('TypeCode[1]', 'varchar(10)')
			, [T].[C].value('TypeName[1]', 'nvarchar(50)')
			, [T].[C].value('TypeStatus[1]', 'varchar(20)')
			, [T].[C].value('TypeType[1]', 'varchar(20)')
		FROM @x.nodes('/Root/Type') as [T]([C])
		-- Xóa giá trị detail cũ (nếu có)
		DELETE D
		FROM [dbo].[TB_TYPE_DETAILS] D
		WHERE D.TypeDetailTypeCode = @typeCode
		-- Thêm Detail
		INSERT INTO [dbo].[TB_TYPE_DETAILS](TypeDetailName, TypeDetailType, TypeDetailTypeCode)
		SELECT [T].[C].value('DetailName[1]', 'nvarchar(50)')
			, [T].[C].value('DetailType[1]', 'varchar(50)')
			, @typeCode
		FROM @x.nodes('/Root/Rows/Detail') as [T]([C])
	END
	SELECT @ecode ecode, '' edesc
COMMIT
END TRY
BEGIN CATCH
   ROLLBACK
   SET @ecode = '999'
   SELECT '999' ecode, ERROR_MESSAGE() edesc
END CATCH