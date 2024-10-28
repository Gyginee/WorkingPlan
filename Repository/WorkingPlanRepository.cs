using System.Data;
using WorkingPlan.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Drawing.Printing;


namespace WorkingPlan.Repository
{
    public class WorkingPlanRepository
    {
        public readonly IDbConnection _connection;

        public WorkingPlanRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // Get WorkingPlanByMonth
        public async Task<WorkingPlanModel> GetWorkingPlanByMonth(int month, int pageSize, int pageNumber)
        {
            var workingPlan = await _connection.QuerySingleOrDefaultAsync<WorkingPlanModel>("GetWorkingPlanByMonth", new
            {
                Month = month,
                PageSize = pageSize,
                PageNumber = pageNumber
            }, commandType: CommandType.StoredProcedure);
            return workingPlan;
        }

        // GET All By Month
        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlans(int pageSize, int pageNumber)
        {
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>("GetAllWorkingPlanWithPagination", new
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            }, commandType: CommandType.StoredProcedure);
            return workingPlans;
        }

        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlanByMonth(int month)
        {
            var query = "SELECT ShopCode, PlanDate, EmployeeCode FROM WorkingPlan WHERE MONTH(PlanDate) = MONTH(@Month)";
            return await _connection.QueryAsync<WorkingPlanModel>(query, new { Month = month });
        }
        public async Task AddWorkingPlan(WorkingPlanModel workingPlan)
        {
            await _connection.InsertAsync(workingPlan);
        }

        public async Task UpdateWorkingPlan(WorkingPlanModel workingPlan)
        {
            await _connection.UpdateAsync(workingPlan);
        }

        public async Task DeleteWorkingPlan(int id)
        {
            var workingPlan = await _connection.GetAsync<WorkingPlanModel>(id);
            if (workingPlan != null)
            {
                await _connection.DeleteAsync(workingPlan);
            }
        }
    }
}
