using Microsoft.AspNetCore.Mvc;
using WorkingPlan.Models;
using WorkingPlan.Repository;

namespace WorkingPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingPlanController : ControllerBase
    {
        private readonly WorkingPlanRepository _workingplanRepository;

        public WorkingPlanController(WorkingPlanRepository workingplanRepository)
        {
            _workingplanRepository = workingplanRepository;
        }

        // GET: api/WorkingPlan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkingPlanModel>>> GetAllWorkingPlans(int pageSize, int pageNumber)
        {
            var data = await _workingplanRepository.GetAllWorkingPlans(pageSize, pageNumber);
            if (data == null || !data.Any())
            {
                return NotFound();
            }
            return Ok(data);
        }

        // GET: api/WorkingPlan/{month}
        [HttpGet("{month:int}/{year:int}")]
        public async Task<ActionResult<IEnumerable<WorkingPlanModel>>> GetWorkingPlansByMonth(int month, int year)
        {
            var data = await _workingplanRepository.GetAllWorkingPlanByMonth(month, year);
            if (data == null || !data.Any())
            {
                return NotFound();
            }
            return Ok(data);
        }
        
        // GET: api/WorkingPlan/ByMonth
        [HttpGet("ByMonth/{month:int}/{year:int}/{pageSize:int}/{pageNumber:int}")]
        public async Task<ActionResult<IEnumerable<WorkingPlanModel>>> GetWorkingPlans(int month, int year, int pageSize, int pageNumber)
        {
            var data = await _workingplanRepository.GetWorkingPlansByMonthAndYear(month, year, pageSize, pageNumber);
            if (data == null || !data.Any())
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
