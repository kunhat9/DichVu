﻿USE DB_SERVICES
GO
IF OBJECT_ID('CORE_SP_UpdateRoleFromGroup', 'P') IS NOT NULL
DROP PROC CORE_SP_UpdateRoleFromGroup
GO
CREATE PROCEDURE CORE_SP_UpdateRoleFromGroup
(
	@groupId varchar(50),
	@createBy varchar(50),
	@listRoleId varchar(MAX)
) AS
BEGIN
	DECLARE @check varchar(50)
	CREATE TABLE #ListRole
	(
		ID int identity,
		ROLE_ID varchar(50)
	)
	INSERT INTO #ListRole
	SELECT * FROM FN_SplitStringToTable(@listRoleId,',')
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DELETE FROM SYS_GROUP_ROLES
		WHERE REL_ROLEID NOT IN (SELECT ROLE_ID FROM #ListRole)
			AND REL_GROUPID = @groupId
		CREATE TABLE #ListOldAction (
			 REL_ID int
			,REL_GROUPID varchar(50)
			,REL_ROLEID varchar(50)
			,REL_CREATEBY varchar(50)
			,REL_CREATETIME	date 
		)
		INSERT INTO #ListOldAction (REL_ROLEID)
		SELECT ROLE_ID FROM #ListRole 
		WHERE ROLE_ID NOT IN (SELECT REL_ROLEID FROM SYS_GROUP_ROLES WHERE REL_GROUPID = @groupId)
		UPDATE #ListOldAction
		SET REL_GROUPID = @groupId,
			REL_CREATEBY= @createBy,
			REL_CREATETIME = GETDATE(),
			REL_ID = (NEXT VALUE FOR SYS_GROUP_ROLES_SEQ)
		INSERT INTO SYS_GROUP_ROLES(REL_ID,REL_GROUPID,REL_ROLEID,REL_CREATEBY,REL_CREATETIME)
		SELECT REL_ID,REL_GROUPID,REL_ROLEID,REL_CREATEBY,REL_CREATETIME FROM #ListOldAction
		DROP TABLE #ListRole
		DROP TABLE #ListOldAction
		SET @check = 'SUSCESS'
		SELECT @check
	COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		DECLARE @ErrorMessage VARCHAR(2000)
		SELECT @ErrorMessage = 'Lỗi: ' + ERROR_MESSAGE()
		RAISERROR(@ErrorMessage, 16, 1)
	END CATCH
END
GO
