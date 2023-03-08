using AutoMapper;

using EMS.Application.Common.Mappings;
using EMS.Domain.Entities;

using System.ComponentModel;

namespace EMS.Application.Features.Employee.DTOs;


public class EmployeeDto : IMapFrom<EmployeeEntity>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<EmployeeEntity, EmployeeDto>()
                        .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(x => x.Department.Name));
        profile.CreateMap<EmployeeDto, EmployeeEntity>();
    }

    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string Name { get; set; }

    [Description("Email")]
    public string? Email { get; set; }

    [Description("DOB")]
    public DateTime DOB { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }

}

