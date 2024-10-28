using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkingPlan.Models;
using WorkingPlan.Repository;

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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 25)
        {
            var workingPlans = await _workingPlanRepository.GetAllWorkingPlans(pageSize, pageNumber);
            return View(workingPlans);
        }

        public async Task<IActionResult> ExportData(int month)
        {
            var allwplan = await _workingPlanRepository.GetAllWorkingPlanByMonth(month);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
