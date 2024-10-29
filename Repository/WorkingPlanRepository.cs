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

        // Lấy danh sách WorkingPlans theo tháng và năm
        public async Task<IEnumerable<WorkingPlanModel>> GetWorkingPlansByMonthAndYear(int month, int year, int pageSize, int pageNumber)
        {
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>(
                "GetWorkingPlanByMonthAndYear", 
                new { Month = month, Year = year, PageSize = pageSize, PageNumber = pageNumber }, 
                commandType: CommandType.StoredProcedure
            );
            return workingPlans;
        }


        // Lấy tất cả WorkingPlans theo tháng
        public async Task<IEnumerable<WorkingPlanModel>> GetAllWorkingPlanByMonth(int month, int year)
        {
            
            //  var query = "SELECT ShopCode, PlanDate, EmployeeCode FROM WorkingPlan WHERE YEAR(PlanDate) = @Year AND MONTH(PlanDate) = @Month ORDER BY PlanDate";
            var workingPlans = await _connection.QueryAsync<WorkingPlanModel>("GetAllWorkingPlanByMonth", 
                new { Month = month, Year = year },
                commandType: CommandType.StoredProcedure);
            return workingPlans;
        }
    }
}
