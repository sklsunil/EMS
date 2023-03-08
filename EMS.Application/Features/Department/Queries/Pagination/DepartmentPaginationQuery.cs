using AutoMapper;
using AutoMapper.QueryableExtensions;

using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Common.Mappings;
using EMS.Application.Common.Models;
using EMS.Application.Features.Department.Caching;
using EMS.Application.Features.Department.DTOs;

using MediatR;

using Microsoft.Extensions.Caching.Memory;

using System.Linq.Dynamic.Core;

namespace EMS.Application.Features.Department.Queries.Pagination;

public class DepartmentWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<DepartmentDto>>
{
    public string CacheKey => DepartmentCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => DepartmentCacheKey.MemoryCacheEntryOptions;
}

public class DepartmentWithPaginationQueryHandler :
     IRequestHandler<DepartmentWithPaginationQuery, PaginatedData<DepartmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedData<DepartmentDto>> Handle(DepartmentWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Departments.OrderBy($"{request.OrderBy} {request.SortDirection}")
             .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }
}