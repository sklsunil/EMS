using EMS.Application.Common.Exceptions;
using EMS.Application.Features.Employee.Commands.AddEdit;
using EMS.Application.Features.Employee.Commands.Delete;
using EMS.Application.Features.Employee.Queries.GetAll;
using EMS.Application.Features.Employee.Queries.Pagination;
using EMS.Domain.Entities;

using FluentAssertions;

namespace EMS.Application.IntegrationTests.Employee.Commands;
using static Testing;
internal class DeleteEmployeeCommandTests : TestBase
{
    [Test]
    public void ShouldRequireValidKeyValueId()
    {
        var command = new DeleteEmployeeCommand(new List<int> { 99 });

        FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteOne()
    {
        var addcommand = new AddEditEmployeeCommand()
        {
            Name = "Test1",
            Email = "Test1@test.com",
            DOB = DateTime.Now.AddYears(-10),
            DepartmentId = 1
        };
        var result = await SendAsync(addcommand);

        await SendAsync(new DeleteEmployeeCommand(new List<int> { result.Data }));

        var item = await FindAsync<EmployeeEntity>(result.Data);

        item.Should().BeNull();
    }
    [SetUp]
    public async Task InitData()
    {
        await AddAsync(new EmployeeEntity() { Name = "Test1", Email = "Test1@test.com", DOB = DateTime.Now.AddYears(-10), DepartmentId = 1 });
        await AddAsync(new EmployeeEntity() { Name = "Test2", Email = "Test2@test.com", DOB = DateTime.Now.AddYears(-20), DepartmentId = 2 });
        await AddAsync(new EmployeeEntity() { Name = "Test3", Email = "Test3@test.com", DOB = DateTime.Now.AddYears(-30), DepartmentId = 3 });
        await AddAsync(new EmployeeEntity() { Name = "Test4", Email = "Test4@test.com", DOB = DateTime.Now.AddYears(-40), DepartmentId = 4 });

    }
}
