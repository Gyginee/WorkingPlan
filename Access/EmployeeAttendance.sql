USE [data_test]
GO

/****** Object:  Table [dbo].[Attendants]    Script Date: 10/28/2024 11:11:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Attendants](
	[AttendantId] [bigint] IDENTITY(1,1) NOT NULL,
	[ShopCode] [varchar](32) NULL,
	[EmployeeCode] [varchar](32) NULL,
	[AttendantType] [int] NULL,
	[AttendantDate] [date] NULL,
	[AttendentTime] [datetime] NULL,
	[AttendantPhoto] [varchar](1) NOT NULL,
	[Longitude] [numeric](1, 1) NOT NULL,
	[Latitude] [numeric](1, 1) NOT NULL,
	[Accuracy] [numeric](1, 1) NOT NULL,
	[CreateDate] [datetime] NULL,
	[Status] [int] NULL
) ON [PRIMARY]
GO


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
