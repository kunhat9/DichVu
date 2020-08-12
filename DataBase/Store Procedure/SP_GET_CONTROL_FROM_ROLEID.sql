USE DB_SERVICES
GO
IF OBJECT_ID('SP_GET_CONTROL_FROM_ROLEID', 'P') IS NOT NULL
DROP PROC SP_GET_CONTROL_FROM_ROLEID
GO
CREATE PROCEDURE SP_GET_CONTROL_FROM_ROLEID
	@role_id VARCHAR(50)
AS
BEGIN
	
	SELECT ct.* FROM SYS_CONTROLS ct
	JOIN SYS_ROLE_CONTROLS rc ON ct.CONTROL_ID = rc.RC_CONTROL_ID
	WHERE rc.RC_ROLE_ID = @role_id
	
END