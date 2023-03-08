using EMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace EMS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    ChangeTracker ChangeTracker { get; }
    DbSet<EmployeeEntity> Employees { get; set; }
    DbSet<DepartmentEntity> Departments { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
