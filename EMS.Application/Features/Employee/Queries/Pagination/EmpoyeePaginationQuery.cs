using AutoMapper;
using AutoMapper.QueryableExtensions;

using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Common.Mappings;
using EMS.Application.Common.Models;
using EMS.Application.Features.Employee.Caching;
using EMS.Application.Features.Employee.DTOs;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using System.Linq.Dynamic.Core;

namespace EMS.Application.Features.Employee.Queries.Pagination;

public class EmployeeWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<EmployeeDto>>
{
    public string CacheKey => EmployeeCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => EmployeeCacheKey.MemoryCacheEntryOptions;
}

public class EmployeeWithPaginationQueryHandler :
     IRequestHandler<EmployeeWithPaginationQuery, PaginatedData<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedData<EmployeeDto>> Handle(EmployeeWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = _context.Employees.Include(x => x.Department).AsQueryable();
        var adS = request.AdvancedSearch;
        if (adS != null)
        {
            foreach (var field in adS.Fields)
            {
                if (field.ToLower() == "id" && adS.Keyword != null)
                {
                    var isDigit = adS.Keyword.All(char.IsDigit);
                    if (isDigit)
                    {
                        var id = int.Parse(adS.Keyword);
                        data = data.Where(x => x.Id == id);
                    }
                }
                else if (field.ToLower() == "name" && adS.Keyword != null)
                {
                    data = data.Where(x => x.Name.Contains(adS.Keyword));
                }
                else if (field.ToLower() == "email" && adS.Keyword != null)
                {
                    data = data.Where(x => x.Email.Contains(adS.Keyword));
                }
            }
        }

        var result = await data.OrderBy($"{request.OrderBy} {request.SortDirection}")
               .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
               .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return result;
    }
}