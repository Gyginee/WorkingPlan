USE [data_test]
GO

/****** Object:  Table [dbo].[WorkingPlan]    Script Date: 10/28/2024 7:26:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkingPlan](
	[ShopCode] [varchar](32) NOT NULL,
	[PlanDate] [date] NULL,
	[EmployeeCode] [nvarchar](32) NULL,
	[CreateDate] [datetime] NULL
) ON [PRIMARY]
GO

CREATE PROCEDURE GetAllWorkingPlanWithPagination
	@pageNumber INT,
	@pageSize INT
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @offset INT = (@pageNumber - 1) * @pageSize;

	SELECT ShopCode,
	PlanDate,
	EmployeeCode
	FROM WorkingPlan
	ORDER BY PlanDate
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY;
END
GO

CREATE PROCEDURE GetAllWorkingPlan
AS BEGIN
	SET NOCOUNT ON;
	SELECT ShopCode,
	PlanDate,
	EmployeeCode
	FROM WorkingPlan
END
GO

CREATE PROCEDURE GetWorkingPlanByMonth
	@month INT,
	@year INT
AS BEGIN
	SET NOCOUNT ON;
	SELECT ShopCode,
	PlanDate,
	EmployeeCode
	FROM WorkingPlan
	WHERE MONTH(PlanDate) = @month AND YEAR(PlanDate) = @year
END
GO

CREATE PROCEDURE GetWorkingPlanByMonthAndYear
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

SELECT ShopCode,
EmployeeCode
FROM WorkingPlan
WHERE EXISTS (
    SELECT 1
    FROM WorkingPlan AS wp2
    WHERE wp2.ShopCode = WorkingPlan.ShopCode AND wp2.EmployeeCode = WorkingPlan.EmployeeCode
    GROUP BY ShopCode, EmployeeCode
    HAVING COUNT(*) > 1
)