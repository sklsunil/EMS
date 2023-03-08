using EMS.Domain.Entities;

namespace EMS.Infrastructure.Persistence
{

    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                _context.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }


        public async Task TrySeedAsync()
        {
            List<string> departments = new() { "HR", "IT", "Software", "DevOps" };
            foreach (var department in departments)
            {
                var dbValue = await _context.Departments.FirstOrDefaultAsync(x => x.Name == department);
                if (dbValue == null)
                {
                    var obj = new DepartmentEntity
                    {
                        Name = department
                    };
                    await _context.Departments.AddAsync(obj);
                    await _context.SaveChangesAsync();
                }
            }

            List<EmployeeEntity> employees = new()
            {
                            new EmployeeEntity() { Name = "Test1", Email = "test1@test.com", DOB = DateTime.Now.AddYears(-10), DepartmentId = 1 },
                            new EmployeeEntity() { Name = "Test2", Email = "test2@test.com", DOB = DateTime.Now.AddYears(-20), DepartmentId = 2 },
                            new EmployeeEntity() { Name = "Test3", Email = "test3@test.com", DOB = DateTime.Now.AddYears(-30), DepartmentId = 3 },
                            new EmployeeEntity() { Name = "Test4", Email = "test4@test.com", DOB = DateTime.Now.AddYears(-40), DepartmentId = 4 }

            };
            foreach (var employee in employees)
            {
                var dbValue = await _context.Employees.FirstOrDefaultAsync(x => x.Email == employee.Email);
                if (dbValue == null)
                {                      
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
