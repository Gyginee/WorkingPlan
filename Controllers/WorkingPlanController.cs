
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WorkingPlan.Models;
using WorkingPlan.Repository;
using System.Data;
using SuperConvert.Extensions;


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

        // GET: api/product
        [HttpGet]
        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlans(int pageSize, int pageNumber)
        {
            return await _workingplanRepository.GetAllWorkingPlans(pageSize, pageNumber);
        }

        // GET Month
        [HttpGet("{month:int}")]
        public async Task<ActionResult<IEnumerable<WorkingPlanModel>>> GetAllWorkingPlanByMonth(int month)
        {
            var data = await _workingplanRepository.GetAllWorkingPlanByMonth(month);
            if (data == null || !data.Any())
            {
                return NotFound();
            }
            return Ok(data);
        }

        
    }
}
