--Get all AttentdantByMonthAndYear
CREATE PROCEDURE [dbo].[GetAllAttendantByMonthAndYear]
	@Month INT,
	@Year INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ShopCode,EmployeeCode,AttendantDate,AttendentTime FROM Attendants WHERE MONTH(AttendantDate) = 5 AND YEAR(AttendantDate) = 2022 AND AttendantType = 0
    ORDER BY AttendantDate
END
GO

--Get AttendantByMonthAndYear w/ page
CREATE PROCEDURE GetAttendanceByMonthAndYear
	@Month INT,
	@Year INT,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
	SELECT * FROM Attendants WHERE MONTH(AttendantDate) = @Month AND YEAR(AttendantDate) = @Year
    ORDER BY AttendantDate DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

--Get WorkingPlanByMonthAndYear W/ Page
CREATE PROCEDURE [dbo].[GetWorkingPlanByMonthAndYear]
	@month INT,
	@year INT,
    @pageNumber INT,    
    @pageSize INT
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @offset INT = (@pageNumber - 1) * @pageSize;
	SELECT ShopCode,
	PlanDate,
	EmployeeCode
	FROM WorkingPlan
	WHERE MONTH(PlanDate) = @month AND YEAR(PlanDate) = @year
	ORDER BY PlanDate
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY;
END
GO

--GetWorkingPlanInMonthWithPage 
CREATE PROCEDURE [dbo].[GetWorkingPlanInMonth]
 @Month INT,
 @PageSize INT,
 @PageNumber INT
 AS
 BEGIN
	SET NOCOUNT ON;
    DECLARE @Offset INT  = (@PageNumber - 1) * @PageSize;

	SELECT	
		ShopCode,
		PlanDate,
		EmployeeCode
	FROM WorkingPlan
	WHERE MONTH(PlanDate) = @Month
	ORDER BY PlanDate
	OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY

END
GO

--Get All WorkingPlan By Year and Month
CREATE PROCEDURE [dbo].[GetAllWorkingPlanByMonth]
	@Month INT,
	@Year INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ShopCode,
	PlanDate,
	EmployeeCode
	FROM WorkingPlan
	WHERE YEAR(PlanDate) = @Year AND MONTH(PlanDate) = @Month
	ORDER BY PlanDate
END
GO