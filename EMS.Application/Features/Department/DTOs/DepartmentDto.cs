using AutoMapper;

using EMS.Application.Common.Mappings;
using EMS.Domain.Entities;

using System.ComponentModel;

namespace EMS.Application.Features.Department.DTOs;


public class DepartmentDto : IMapFrom<DepartmentEntity>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<DepartmentEntity, DepartmentDto>();
        profile.CreateMap<DepartmentDto, DepartmentEntity>();
    }

    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string Name { get; set; }

}

