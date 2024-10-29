using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkingPlan.Models;
using WorkingPlan.Repository;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;


namespace WorkingPlan.Controllers
{
    public class WorkingPlanViewController : Controller
    {
        private readonly ILogger<WorkingPlanViewController> _logger;
        private readonly WorkingPlanRepository _workingPlanRepository;

        public WorkingPlanViewController(ILogger<WorkingPlanViewController> logger, WorkingPlanRepository workingPlanRepository)
        {
            _logger = logger;
            _workingPlanRepository = workingPlanRepository;
        }

        // Trả về view dsach WorkingPlan
        public async Task<IActionResult> Index(int? pageNumber = null, int? pageSize = null, int? month = null, int? year = null)
        {
            ViewBag.SelectedMonth = month ?? DateTime.Now.Month;
            ViewBag.SelectedYear = year ?? DateTime.Now.Year;


            var workingPlans = await _workingPlanRepository.GetWorkingPlansByMonthAndYear(
                ViewBag.SelectedMonth,
                ViewBag.SelectedYear,
                pageSize.HasValue ? pageSize.Value : 15,
                pageNumber.HasValue ? pageNumber.Value : 1
            );

            return View(workingPlans);
        }

        // Xuất file excel tất cả các data trong tháng
        public async Task<IActionResult> ExportJsonToXls(int month, int year)
        {
            var allwplan = await _workingPlanRepository.GetAllWorkingPlanByMonth(month, year);

            using (var workbook = new XSSFWorkbook())
            {
                var sheet = workbook.CreateSheet("WorkingPlans");

                var row = sheet.CreateRow(0);
                int columnIndex = 0;
                var headerCellStyle = workbook.CreateCellStyle();
                headerCellStyle.Alignment = HorizontalAlignment.Center;
                headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                headerCellStyle.FillForegroundColor = HSSFColor.Grey50Percent.Index;
                headerCellStyle.FillPattern = FillPattern.SolidForeground;

                //Tạo Header
                foreach (var property in typeof(WorkingPlanModel).GetProperties())
                {
                    var cell = row.CreateCell(columnIndex++);
                    cell.SetCellValue(property.Name);
                    cell.CellStyle = headerCellStyle;
                }

                //Tạo hàng và thêm data vào ô
                row = sheet.CreateRow(1);
                int currentRowIndex = 1;
                foreach (var plan in allwplan)
                {
                    row = sheet.CreateRow(currentRowIndex++);
                    row.CreateCell(0).SetCellValue(plan.ShopCode);
                    row.CreateCell(1).SetCellValue(plan.PlanDate.ToString("dd-MM-yyyy"));
                    row.CreateCell(2).SetCellValue(plan.EmployeeCode);

                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var fileName = $"WorkingPlans_{month}_{year}.xlsx";

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        //PENDING
        public async Task<IActionResult> ExportWorkingPlanByDay(int month, int year)
        {
            var allWorkingPlan = await _workingPlanRepository.GetAllWorkingPlanByMonth(month, year);
            using (var workbook = new XSSFWorkbook())
            {
                var sheet = workbook.CreateSheet("WorkingPlans");
                var row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("EmployeeCode");

                int daysInMonth = DateTime.DaysInMonth(year, month);

                for (int day = 1; day <= daysInMonth; daysInMonth++)
                {
                    row.CreateCell(day).SetCellValue($"{day}-{month}-{year}");
                }
                var addedEmployees = new Dictionary<string, int>();
                int rowIndex = 1;

                foreach (var workingplan in allWorkingPlan)
                {
                    string employeeCode = workingplan.EmployeeCode;

                    if (!addedEmployees.ContainsKey(employeeCode))
                    {
                        var rowData = sheet.CreateRow(rowIndex++);
                        rowData.CreateCell(0).SetCellValue(employeeCode);
                        addedEmployees[employeeCode] = rowIndex - 1;
                    }

                    int existingRowIndex = addedEmployees[employeeCode];
                    var existingRow = sheet.GetRow(existingRowIndex);

                    int day = workingplan.PlanDate.Day;
                    existingRow.CreateCell(day).SetCellValue("x");

                }
                // Save the workbook to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var fileName = $"WorkingPlanNew_{month}_{year}.xlsx";
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
