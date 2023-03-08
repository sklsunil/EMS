using System.ComponentModel;

namespace EMS.Application.Model.Department
{
    public class DepartmentModel
    {

        [Description("Name")]
        public string Name { get; set; }
    }
    public class UpdateDepartmentModel : DepartmentModel
    {
        [Description("Id")]
        public int Id { get; set; }
    }
}
