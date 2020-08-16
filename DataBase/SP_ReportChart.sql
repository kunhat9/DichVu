USE DB_SERVICES
GO
IF OBJECT_ID('SP_ReportChart', 'P') IS NOT NULL
DROP PROC SP_ReportChart
GO
CREATE PROCEDURE SP_ReportChart
(
	@serviceId varchar(50)
	,@menuId varchar(50)
	,@startDate varchar(50)
	,@endDate varchar(50)
) AS
BEGIN
	IF @serviceId ='' OR @serviceId IS NULL SET @serviceId = NULL
	IF @menuId ='' OR @menuId IS NULL SET @menuId = NULL
	IF @startDate ='' OR @startDate IS NULL SET @startDate = NULL
	IF @endDate ='' OR @endDate IS NULL SET @endDate = NULL
	DECLARE @mydate DATETIME
	SET @mydate = GETDATE()
	DECLARE @to date, @from date
	IF (@endDate IS NULL OR CONVERT(date, @endDate) > GETDATE()) SET @to = (SELECT CONVERT(DATE,DATEADD(dd,-(DAY(DATEADD(mm,1,@mydate))),DATEADD(mm,1,@mydate)),103))
	ELSE SET @to = (CONVERT(date, @endDate))
	IF(@startDate IS NULL OR @startDate > @to) SET @from = (SELECT CONVERT(DATE,DATEADD(dd,-(DAY(@mydate)-1),@mydate),103))
	ELSE SET @from = (CONVERT(date, @startDate))

	;WITH CTE AS
	(
		SELECT CONVERT(date, @to) sDate
		UNION ALL
		SELECT DATEADD(DAY, -1, sDate)
		FROM CTE
		WHERE sDate > @from
	)
	SELECT sDate DateCreated, ISNULL(data.ServicePrice,0) ServicePrice, ISNULL(data.MenuPrice,0) MenuPrice FROM CTE
	FULL JOIN (SELECT ISNULL(SUM(r.RegisterMenuPrice),0)  MenuPrice , ISNULL(SUM(r.RegisterServicePrice),0) ServicePrice, r.RegisterDateCreate
	FROM TB_REGISTERS r
	Where r.RegisterStatus ='A'
	AND (@serviceId IS NULL OR r.RegisterServiceId = @serviceId)
	AND (@menuId IS NULL OR r.RegisterMenuId = @menuId)
	AND (@startDate IS NULL OR r.RegisterDateCreate <=  CONVERT(date,@startDate))
	AND (@endDate IS NULL OR r.RegisterDateCreate >= CONVERT(date,@startDate))
	GROUP BY r.RegisterDateCreate) data
	ON CTE.sDate = data.RegisterDateCreate
	ORDER BY sDate asc
	


	
END
GO
