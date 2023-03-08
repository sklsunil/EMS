using System.ComponentModel;

namespace EMS.Application.Model.Employee
{
    public class EmployeeModel
    {

        [Description("Name")]
        public string Name { get; set; }

        [Description("Email")]
        public string Email { get; set; }

        [Description("DOB")]
        public DateTime DOB { get; set; }
        public int DepartmentId { get; set; }
    }
    public class UpdateEmployeeModel : EmployeeModel
    {
        [Description("Id")]
        public int Id { get; set; }
    }
}
