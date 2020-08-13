USE DB_SERVICES
GO
IF OBJECT_ID('SP_REGISTER', 'P') IS NOT NULL
DROP PROC SP_REGISTER
GO
CREATE PROCEDURE SP_REGISTER
(
	@reg_service nvarchar(50)
	,@reg_datebegin nvarchar(50)
	,@reg_number nvarchar(50)
	,@reg_name nvarchar(50)
	,@reg_email nvarchar(50)
	,@reg_phone nvarchar(50)
	,@menuId nvarchar(50)
	,@reg_note nvarchar(max)
	,@reg_user varchar(50)
	,@listMenu nvarchar(50)
	,@voucherCode varchar(50)
	,@reg_table varchar(50)
) AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @ecode VARCHAR(50) = '00'
	IF @menuId ='' OR menuId IS NULL SET menuId = NULL
	DECLARE @price decimal(18,0)
	DECLARE @chargeService decimal(18,0) = (select ServicePrice from TB_SERVICES where ServiceId = @reg_service)
	DECLARE @typeVoucher varchar(50) = (select VoucherType from TB_VOUCHERS WHERE VoucherCode = @voucherCode AND VoucherServiceId = @reg_service)
	DECLARE @priceMenu decimal(18,0) = (select MenuPrice from TB_MENUS where MenuId = @menuId)
	IF @typeVoucher ='M'
		BEGIN
		DECLARE @count decimal(18,0) = (select VoucherNum from TB_VOUCHERS WHERE VoucherCode = @voucherCode AND VoucherServiceId = @reg_service)
		-- giam tien
		SET @price = (@chargeService - @count) + @priceMenu*@reg_table
 
		END
	ELSE IF @typeVoucher ='P' 
		BEGIN
			-- %
			DECLARE @countVoucher decimal(18,2) = (select VoucherNum from TB_VOUCHERS WHERE VoucherCode = @voucherCode AND VoucherServiceId = @reg_service)
			SET @price = (@chargeService-(@chargeService*@countVoucher/100)) + (@priceMenu*@reg_table)
		END
	ELSE 
		BEGIN
			SET @price = @chargeService + (@priceMenu*@reg_table)
		END
	IF(@reg_user ='' OR @reg_user IS NULL) SET @reg_user = NULL
	CREATE TABLE #TempId(
	RegisterId int
	)
	INSERT INTO TB_REGISTERS(
      [RegisterUserName]
      ,[RegisterUserEmail]
      ,[RegisterUserPhone]
      ,[RegisterUserNote]

      ,[RegisterDateCreate]
      ,[RegisterDateBegin]
      ,[RegisterDateNumber]

      ,[RegisterStatus]

      ,[RegisterVoucherNum]
      ,[RegisterVoucherType]
      ,[RegisterMenuNumber]
      ,[RegisterMenuPrice]
      ,[RegisterMenuId]
      ,[RegisterServiceId]
      ,[RegisterUserId]
	)
	OUTPUT inserted.RegisterId into #TempId
	VALUES(
	@reg_name
	,@reg_email
	,@reg_phone
	,@reg_note

	,GETDATE()
	,CONVERT(DATE,@reg_datebegin)
	,@reg_number
	,N'N'

	,NULL -- voucher num
	,NULL -- voucher type
	,@reg_number
	,@price -- price
	,@menuId
	,@reg_service
	,@reg_user -- userId
	)

	--CREATE TABLE #TempRegisdetail(
	--RdetailMdetailId int
	--,RdetailRegisterId int
	--)
	DECLARE @regisId int = (SELECT TOP 1 RegisterId FROM #TempId)
	INSERT INTO TB_REGISTER_DETAILS
	SELECT *,@regisId FROM FN_SplitStringToTable(@listMenu,',')
	SELECT @ecode ecode, 'suscess' edesc
	DROP TABLE #TempId
	--DROP TABLE #TempRegisdetail
COMMIT
END TRY
BEGIN CATCH
   ROLLBACK
   SET @ecode = '999'
   SELECT '999' ecode, ERROR_MESSAGE() edesc
END CATCH