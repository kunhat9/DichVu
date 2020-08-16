
USE DB_SERVICES
GO
IF OBJECT_ID('SP_UpdateStatus', 'P') IS NOT NULL
DROP PROC SP_UpdateStatus
GO
CREATE PROCEDURE SP_UpdateStatus
(
	@registerId varchar(50)
	,@status varchar(50)
	
) AS
BEGIN
	
	UPDATE TB_REGISTERS 
		SET RegisterStatus =@status
	WHERE RegisterId = @registerId
	SELECT '00' ecode , 'suscess' edesc
END
GO
