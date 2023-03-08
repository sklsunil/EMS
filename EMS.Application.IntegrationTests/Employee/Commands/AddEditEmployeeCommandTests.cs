using EMS.Application.Features.Employee.Commands.AddEdit;
using EMS.Application.Features.Employee.Commands.Update;
using EMS.Domain.Entities;

using FluentAssertions;

namespace EMS.Application.IntegrationTests.Employee.Commands;
using static Testing;
internal class AddEditEmployeeCommandTests : TestBase
{
    [Test]
    public void ShouldThrowValidationException()
    {
        var command = new AddEditEmployeeCommand();
        FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Test]
    public async Task InsertItem()
    {
        var addcommand = new AddEditEmployeeCommand()
        {
            Name = "Test1",
            Email = "Test1@test.com",
            DOB = DateTime.Now.AddYears(-10),
            DepartmentId = 1
        };
        var result = await SendAsync(addcommand);
        var find = await FindAsync<EmployeeEntity>(result.Data);
        find.Should().NotBeNull();
        find.Name.Should().Be("Test1");
        find.Email.Should().Be("Test1@test.com");
        find.DepartmentId.Should().Be(1);
    }
    [Test]
    public async Task UpdateItem()
    {
        var addcommand= new AddEditEmployeeCommand()
        {
            Name = "Test1",
            Email = "Test1@test.com",
            DOB = DateTime.Now.AddYears(-10),
            DepartmentId = 1
        };
        var resultAdded = await SendAsync(addcommand);
        var getAdded = await FindAsync<EmployeeEntity>(resultAdded.Data);

        var updateCmd = new UpdateEmployeeCommand()
        {
            Name = "Test",
            Email = "Test@test.com",
            DOB = DateTime.Now.AddYears(-10),
            DepartmentId = 1,
            Id = getAdded.Id
        };
        var result = await SendAsync(updateCmd);
        var find = await FindAsync<EmployeeEntity>(getAdded.Id);
        var editcommand = new UpdateEmployeeCommand()
        {
            Id = find.Id,
            Name = "Test1",
            Email = "Test1@test.com",
            DOB = DateTime.Now.AddYears(-20),
            DepartmentId = 2
        };
        await SendAsync(editcommand);
        var updated = await FindAsync<EmployeeEntity>(find.Id);
        updated.Should().NotBeNull();
        updated.Id.Should().Be(find.Id);
        updated.Name.Should().Be("Test1");
    }
}
