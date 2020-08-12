USE DB_SERVICES
GO
IF OBJECT_ID('SP_CheckLogin', 'P') IS NOT NULL
DROP PROC SP_CheckLogin
GO
CREATE PROCEDURE SP_CheckLogin
(
	@username varchar(50),
	@password varchar(50)
) AS
BEGIN
	SELECT UserId
		, Username
		, UserFullName
		, UserPhone
		, UserEmail
		, UserAddress
		, UserType
		, UserStatus
		, UserNote
	FROM TB_USERS
	WHERE Username = @username
		AND UserPassword = @password
		AND UserStatus = 'A'
END
GO
