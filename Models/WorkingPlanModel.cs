using Dapper.Contrib.Extensions;

namespace WorkingPlan.Models
{
    public class WorkingPlanModel
    {
        [Key]
        public string ShopCode { get; set; }
        public DateTime PlanDate { get; set; }
        public string EmployeeCode { get; set; }
        //public DateTime CreateDate { get; set; }
    }
}
