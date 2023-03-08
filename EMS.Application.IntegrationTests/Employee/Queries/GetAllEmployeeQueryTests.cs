using EMS.Application.Features.Employee.Queries.GetAll;
using EMS.Application.Features.Employee.Queries.Pagination;
using EMS.Domain.Entities;

namespace EMS.Application.IntegrationTests.Employee.Queries;
using static Testing;
internal class GetAllEmployeeQueryTests : TestBase
{
    [SetUp]
    public async Task InitData()
    {
        await AddAsync(new EmployeeEntity() { Name = "Test1", Email = "Test1@test.com", DOB = DateTime.Now.AddYears(-10), DepartmentId = 1 });
        await AddAsync(new EmployeeEntity() { Name = "Test2", Email = "Test2@test.com", DOB = DateTime.Now.AddYears(-20), DepartmentId = 2 });
        await AddAsync(new EmployeeEntity() { Name = "Test3", Email = "Test3@test.com", DOB = DateTime.Now.AddYears(-30), DepartmentId = 3 });
        await AddAsync(new EmployeeEntity() { Name = "Test4", Email = "Test4@test.com", DOB = DateTime.Now.AddYears(-40), DepartmentId = 4 });
    }
    [Test]
    public async Task ShouldQueryAll()
    {

        var query = new EmployeeWithPaginationQuery();
        var result = await SendAsync(query);
        Assert.AreEqual(4, result.TotalItems);
    }
}
