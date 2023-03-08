using EMS.Application.Common.Models;
using EMS.Application.Features.Employee.Commands.Create;
using EMS.Application.Features.Employee.Commands.Delete;
using EMS.Application.Features.Employee.Commands.Update;
using EMS.Application.Features.Employee.Queries.Pagination;
using EMS.Application.Model.Employee;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace EMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {

        private readonly ILogger<EmployeeController> _logger;
        public readonly IMediator _mediator;

        public EmployeeController(ILogger<EmployeeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationFilter request)
        {
            var query = new EmployeeWithPaginationQuery
            {
                PageSize = request.PageSize,
                AdvancedSearch = request.AdvancedSearch,
                Keyword = request.Keyword,
                OrderBy = request.OrderBy,
                PageNumber = request.PageNumber,
                SortDirection = request.SortDirection,
            };
            return Ok(await _mediator.Send(query).ConfigureAwait(false));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] EmployeeModel model)
        {
            var cmd = new CreateEmployeeCommand
            {
                DepartmentId = model.DepartmentId,
                DOB = model.DOB,
                Email = model.Email,
                Name = model.Name
            };
            return Ok(await _mediator.Send(cmd).ConfigureAwait(false));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateEmployeeModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(await Result.FailureAsync(new List<string> { "Invalid Id" }));
            }
            var cmd = new UpdateEmployeeCommand
            {
                DepartmentId = model.DepartmentId,
                DOB = model.DOB,
                Email = model.Email,
                Name = model.Name,
                Id = model.Id
            };
            return Ok(await _mediator.Send(cmd).ConfigureAwait(false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var cmd = new DeleteEmployeeCommand(new List<int> { id });
            return Ok(await _mediator.Send(cmd).ConfigureAwait(false));
        }
    }
}