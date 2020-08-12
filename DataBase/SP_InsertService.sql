USE DB_SERVICES
GO
IF OBJECT_ID('SP_InsertService', 'P') IS NOT NULL
DROP PROC SP_InsertService
GO
CREATE PROCEDURE SP_InsertService
(
	@xml NVARCHAR(MAX)
) AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @ecode VARCHAR(50) = '00'
	DECLARE @x XML = @xml
	-- Kiểm tra tồn tại
	DECLARE @serviceId int
	SELECT @serviceId = COALESCE([T].[C].value('ServiceId[1]', 'int'), 0)
		FROM @x.nodes('/Root/Service') as [T]([C])
	IF @serviceId > 0 AND EXISTS (SELECT * FROM TB_SERVICES S WHERE S.ServiceId = @serviceId)
	BEGIN	-- Update (Đã tồn tại)
		-- Cập nhật Service
		UPDATE S
		SET S.ServiceName = V.ServiceName, S.ServicePrice = V.ServicePrice, S.ServiceUnit = V.ServiceUnit, S.ServiceBase = V.ServiceBase, S.ServiceContent = V.ServiceContent, S.ServiceTypeCode = V.ServiceTypeCode, S.ServiceStatus = V.ServiceStatus
		FROM [dbo].[TB_SERVICES] S
			JOIN (SELECT COALESCE([T].[C].value('ServiceId[1]', 'int'), 0) ServiceId
					, [T].[C].value('ServiceName[1]', 'nvarchar(50)') ServiceName
					, COALESCE([T].[C].value('ServicePrice[1]', 'decimal'), 0) ServicePrice
					, [T].[C].value('ServiceUnit[1]', 'nvarchar(30)') ServiceUnit
					, [T].[C].value('ServiceBase[1]', 'nvarchar(50)') ServiceBase
					, [T].[C].value('ServiceContent[1]', 'nvarchar(max)') ServiceContent
					, [T].[C].value('ServiceStatus[1]', 'varchar(20)') ServiceStatus
					, [T].[C].value('ServiceTypeCode[1]', 'varchar(10)') ServiceTypeCode
				FROM @x.nodes('/Root/Service') as [T]([C])) V
			ON S.ServiceId = V.ServiceId
		WHERE S.ServiceId = @serviceId
		-- Xóa giá trị detail cũ (nếu có)
		DELETE S
		FROM [dbo].[TB_SERVICE_DETAILS] S
		WHERE S.SrvDetailServiceId = @serviceId
			AND S.SrvDetailId NOT IN (
					SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
					FROM @x.nodes('/Root/Rows/Detail') as [T]([C])
				)
		-- Cập nhật Detail
		UPDATE S
		SET S.SrvDetailValue = V.DetailName
		FROM [dbo].[TB_SERVICE_DETAILS] S
			JOIN (SELECT COALESCE([T].[C].value('DetailId[1]', 'int'), 0) DetailId
					, [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
				FROM @x.nodes('/Root/Rows/Detail') as [T]([C])) V
			ON S.SrvTypeDetailId = V.DetailId
		WHERE S.SrvDetailServiceId = @serviceId
		-- Thêm Detail
		INSERT INTO [dbo].[TB_SERVICE_DETAILS](SrvDetailValue, SrvDetailServiceId, SrvTypeDetailId)
		SELECT DetailName, @serviceId, DetailId FROM
			(SELECT [T].[C].value('DetailName[1]', 'nvarchar(50)') DetailName
				, [T].[C].value('DetailId[1]', 'varchar(50)') DetailId
			FROM @x.nodes('/Root/Rows/Detail') as [T]([C])) V
		WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TB_SERVICE_DETAILS] D WHERE D.SrvTypeDetailId = V.DetailId AND D.SrvDetailServiceId = @serviceId)
	END
	ELSE
	BEGIN	-- Thêm mới
		-- Thêm Service
		INSERT INTO [dbo].[TB_SERVICES](ServiceName, ServicePrice, ServiceUnit, ServiceBase, ServiceContent, ServiceStatus, ServiceTypeCode)
		SELECT [T].[C].value('ServiceName[1]', 'nvarchar(50)') ServiceName
			, COALESCE([T].[C].value('ServicePrice[1]', 'decimal'), 0) ServicePrice
			, [T].[C].value('ServiceUnit[1]', 'nvarchar(30)') ServiceUnit
			, [T].[C].value('ServiceBase[1]', 'nvarchar(50)') ServiceBase
			, [T].[C].value('ServiceContent[1]', 'nvarchar(max)') ServiceContent
			, [T].[C].value('ServiceStatus[1]', 'varchar(20)') ServiceStatus
			, [T].[C].value('ServiceTypeCode[1]', 'varchar(10)') ServiceTypeCode
		FROM @x.nodes('/Root/Service') as [T]([C])
		SET @serviceId = @@IDENTITY
		-- Xóa giá trị detail cũ (nếu có)
		DELETE S
		FROM [dbo].[TB_SERVICE_DETAILS] S
		WHERE S.SrvDetailServiceId = @serviceId
		-- Thêm Detail
		INSERT INTO [dbo].[TB_SERVICE_DETAILS](SrvDetailValue, SrvDetailServiceId, SrvTypeDetailId)
		SELECT [T].[C].value('DetailName[1]', 'nvarchar(50)')
			, @serviceId
			, [T].[C].value('DetailId[1]', 'varchar(50)')
		FROM @x.nodes('/Root/Rows/Detail') as [T]([C])
	END
	SELECT @ecode ecode, @serviceId edesc
COMMIT
END TRY
BEGIN CATCH
   ROLLBACK
   SET @ecode = '999'
   SELECT '999' ecode, ERROR_MESSAGE() edesc
END CATCH