using System.Data;
using WorkingPlan.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Drawing.Printing;

namespace WorkingPlan.Repository
{
    public class WorkingPlanRepository
    {
        private readonly IDbConnection _connection;

        public WorkingPlanRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // Lấy kế hoạch làm việc theo tháng
        public async Task<WorkingPlanModel> GetWorkingPlanByMonth(int month, int year, int pageSize, int pageNumber)
        {
            var workingPlan = await _connection.QuerySingleOrDefaultAsync<WorkingPlanModel>("GetWorkingPlanByMonth", new { Month = month, Year = year, PageSize = pageSize, PageNumber = pageNumber }, commandType: CommandType.StoredProcedure);
            return workingPlan;
        }

        // Lấy danh sách các kế hoạch làm việc theo tháng và năm
        public async Task<IEnumerable<WorkingPlanModel>> GetWorkingPlansByMonthAndYear(int month, int year, int pageSize, int pageNumber)
        {
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>(
                "GetWorkingPlanByMonthAndYear", 
                new { Month = month, Year = year, PageSize = pageSize, PageNumber = pageNumber }, 
                commandType: CommandType.StoredProcedure
            );
            return workingPlans;
        }

        // Lấy tất cả các kế hoạch làm việc
        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlans(int pageSize, int pageNumber)
        {
            var query = "SELECT * FROM WorkingPlan ORDER BY PlanDate OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>(query, new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize });
            return workingPlans;
        }

        // Lấy tất cả các kế hoạch làm việc theo tháng
        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlanByMonth(int month, int year)
        {
            var query = "SELECT ShopCode, PlanDate, EmployeeCode FROM WorkingPlan WHERE YEAR(PlanDate) = @Year AND MONTH(PlanDate) = @Month ORDER BY PlanDate";
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>(query, new { Month = month, Year = year });
            return workingPlans;
        }
    }
}
