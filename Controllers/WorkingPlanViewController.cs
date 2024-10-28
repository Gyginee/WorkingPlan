using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkingPlan.Models;
using WorkingPlan.Repository;
using System.IO;
using OfficeOpenXml;
using NPOI.XSSF.UserModel;

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

        // Trả về danh sách các kế hoạch làm việc cho một tháng và năm 
        public async Task<IActionResult> Index(int? pageNumber = null, int? pageSize = null, int? month = null, int? year = null)
        {
            ViewBag.SelectedMonth = month ?? DateTime.Now.Month;
            ViewBag.SelectedYear = year ?? DateTime.Now.Year;

            // Gọi phương thức để lấy danh sách kế hoạch làm việc
            var workingPlans = await _workingPlanRepository.GetWorkingPlansByMonthAndYear(
                ViewBag.SelectedMonth, 
                ViewBag.SelectedYear, 
                pageSize.HasValue ? pageSize.Value : 15, 
                pageNumber.HasValue ? pageNumber.Value : 1
            );

            return View(workingPlans);
        }

        private async Task<IEnumerable<WorkingPlanModel>> GetWorkingPlansByMonthAndYear(int month, int year, int pageSize, int pageNumber)
        {
            var result = await _workingPlanRepository.GetWorkingPlansByMonthAndYear(month, year, pageSize, pageNumber);
            return result; // Trả về danh sách kết quả
        }

        // Xuất file excel tất cả các data trong tháng
        public async Task<IActionResult> ExportJsonToXls(int month, int year) 
        {
            var allwplan = await _workingPlanRepository.GetAllWorkingPlanByMonth(month, year); 
            
            using (var workbook = new NPOI.XSSF.UserModel.XSSFWorkbook())
            {
                var sheet = workbook.CreateSheet("WorkingPlans");

                var row = sheet.CreateRow(0);
                int columnIndex = 0;
                foreach (var property in typeof(WorkingPlanModel).GetProperties())
                {
                    row.CreateCell(columnIndex++).SetCellValue(property.Name); 
                }

                row = sheet.CreateRow(1);
                int currentRowIndex = 1;
                foreach (var plan in allwplan)
                {
                    row = sheet.CreateRow(++currentRowIndex);
                    row.CreateCell(0).SetCellValue(plan.ShopCode);
                    row.CreateCell(1).SetCellValue(plan.PlanDate.ToString());
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
