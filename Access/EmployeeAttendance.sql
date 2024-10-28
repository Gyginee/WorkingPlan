CREATE TABLE EmployeeAttendance (
    id INT AUTO_INCREMENT PRIMARY KEY,
    employee_code VARCHAR(50),
    employee_name VARCHAR(100),
    position VARCHAR(50),
    date DATE,
    time_in TIME
);

CREATE PROCEDURE GetEmployeeAttendanceById
 @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM EmployeeAttendance WHERE id = @Id;
END;

CREATE PROCEDURE TotalDaysWorked
 @EmployeeCode VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM EmployeeAttendance WHERE employee_code = @EmployeeCode;
END;

