using EMS.Application.Common.Models;
using EMS.Application.Features.Department.Commands.Create;
using EMS.Application.Features.Department.Commands.Delete;
using EMS.Application.Features.Department.Commands.Update;
using EMS.Application.Features.Department.Queries.Pagination;
using EMS.Application.Model.Department;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace EMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {

        private readonly ILogger<DepartmentController> _logger;
        public readonly IMediator _mediator;

        public DepartmentController(ILogger<DepartmentController> logger, IMediator mediator)
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
            var query = new DepartmentWithPaginationQuery
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
        public async Task<IActionResult> PostAsync([FromBody] DepartmentModel model)
        {
            var cmd = new CreateDepartmentCommand
            {
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
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateDepartmentModel model)
        {
            if (id != model.Id)
            {
                return BadRequest(await Result.FailureAsync(new List<string> { "Invalid Id" }));
            }
            var cmd = new UpdateDepartmentCommand
            {
                Id = model.Id,
                Name = model.Name
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
            var cmd = new DeleteDepartmentCommand(new List<int> { id });
            return Ok(await _mediator.Send(cmd).ConfigureAwait(false));
        }
    }
}