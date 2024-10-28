using Dapper;
using System.Data;
using Attendant.Models;

namespace Attendant.Repository
{
    public class AttendantRepository
    {
        private readonly IDbConnection _connection;

        public AttendantRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<AttendantModel>> GetAttendanceByMonthAndYear(int month, int year, int pageNumber, int pageSize)
        {
            var attendants = await _connection.QueryAsync<AttendantModel>("GetAttendanceByMonthAndYear", 
            new { Month = month, Year = year, PageNumber = pageNumber, PageSize = pageSize }, 
            commandType: CommandType.StoredProcedure);
            return attendants;
        }

        //Lấy tất cả Attendant
        public async Task<IEnumerable<AttendantModel>> GetAllAttendantByMonthandYear(int month, int year)
        {
            var attendants = await _connection.QueryAsync<AttendantModel>(
                "GetAllAttendantByMonthAndYear", 
                new { Month = month, Year = year }, 
                commandType: CommandType.StoredProcedure);
            return attendants;
        }
    }
}
