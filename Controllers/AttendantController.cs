using Attendant.Repository;
using Microsoft.AspNetCore.Mvc;
using Attendant.Models;
using Attendant.Repository;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;

namespace Attendant.Controllers
{
    public class AttendantController : Controller
    {
        private readonly ILogger<AttendantController> logger;
        private readonly AttendantRepository _attendantRepository;

        public AttendantController(ILogger<AttendantController> logger, AttendantRepository attendantRepository)
        {
            this.logger = logger;
            this._attendantRepository = attendantRepository;
        }

        //Trả về view danh sách Attendants
        public async Task<IActionResult> Index(int? pageNumber = null, int? pageSize = null, int? month = null, int? year = null)
        {
            ViewBag.SelectedMonth = month ?? DateTime.Now.Month;
            ViewBag.SelectedYear = year ?? DateTime.Now.Year;


            var workingPlans = await _attendantRepository.GetAttendanceByMonthAndYear(
                ViewBag.SelectedMonth,
                ViewBag.SelectedYear,
                pageSize.HasValue ? pageSize.Value : 15,
                pageNumber.HasValue ? pageNumber.Value : 1
            );

            return View(workingPlans);
        }
        public async Task<IActionResult> ExportAttendantsXlsxByDay(int month, int year)
        {
            var allAttendants = await _attendantRepository.GetAllAttendantByMonthandYear(month, year);
            using(var workbook = new XSSFWorkbook())
            {
                var sheet = workbook.CreateSheet("Attendants");
                // Thêm style cho header row
                var headerCellStyle = workbook.CreateCellStyle();
                headerCellStyle.Alignment = HorizontalAlignment.Center;
                headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                headerCellStyle.FillForegroundColor = HSSFColor.Grey50Percent.Index;
                headerCellStyle.FillPattern = FillPattern.SolidForeground;

                // Tạo dòng header
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("EmployeeCode");
                headerRow.GetCell(0).CellStyle = headerCellStyle;
              

                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    headerRow.CreateCell(day).SetCellValue($"{day}-{month}-{year}");
                    headerRow.GetCell(day).CellStyle = headerCellStyle;
                }

                var addedEmployees = new Dictionary<string, int>();
                int rowIndex = 1;

                foreach(var attendant in allAttendants)
                {
                    if (attendant.AttendantDate.HasValue && attendant.AttendentTime.HasValue)
                    {
                        string employeeCode = attendant.EmployeeCode;

                        // Kiểm tra employeeCode có tồn tại trong Dictionary
                        if (!addedEmployees.ContainsKey(employeeCode))
                        {
                            var row = sheet.CreateRow(rowIndex++);
                            row.CreateCell(0).SetCellValue(employeeCode);

                            addedEmployees[employeeCode] = rowIndex - 1;
                        }

                        // Get the row index for the employee
                        int existingRowIndex = addedEmployees[employeeCode];
                        var existingRow = sheet.GetRow(existingRowIndex);

                        // Add the time to the corresponding day cell
                        int day = attendant.AttendantDate.Value.Day;
                        existingRow.CreateCell(day).SetCellValue(attendant.AttendentTime.Value.ToString("HH:mm:ss"));
                    }
                }
                // Save the workbook to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var fileName = $"AttendantNew_{month}_{year}.xlsx";
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        //Xuất file excel tất cả Attendant trong tháng
        public async Task<IActionResult> ExportJsonToXls(int month, int year)
        {
            var allAttendants = await _attendantRepository.GetAllAttendantByMonthandYear(month, year);
            using (var workbook = new XSSFWorkbook())
            {
                var sheet = workbook.CreateSheet("Attendants");
                // Thêm style cho header row
                var headerCellStyle = workbook.CreateCellStyle();
                headerCellStyle.Alignment = HorizontalAlignment.Center;
                headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                headerCellStyle.FillForegroundColor = HSSFColor.Grey50Percent.Index;
                headerCellStyle.FillPattern = FillPattern.SolidForeground;

                // Tạo dòng header
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ShopCode");
                headerRow.CreateCell(1).SetCellValue("EmployeeCode");
                headerRow.GetCell(0).CellStyle = headerCellStyle;
                headerRow.GetCell(1).CellStyle = headerCellStyle;

                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    headerRow.CreateCell(day + 1).SetCellValue($"{day}-{month}-{year}");
                    headerRow.GetCell(day + 1).CellStyle = headerCellStyle;
                }

                //Thêm data
                int rowIndex = 1;
                foreach (var attendant in allAttendants)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(attendant.ShopCode);
                    row.CreateCell(1).SetCellValue(attendant.EmployeeCode);
                    
                    //Thêm data time dựa theo ngày
                    if (attendant.AttendantDate.HasValue && attendant.AttendentTime.HasValue)
                    {
                        int day = attendant.AttendantDate.Value.Day;
                        row.CreateCell(day + 1).SetCellValue(attendant.AttendentTime.Value.ToString("HH:mm:ss"));
                    }
                }

                // Save the workbook to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var fileName = $"Attendant_{month}_{year}.xlsx";
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }
}
