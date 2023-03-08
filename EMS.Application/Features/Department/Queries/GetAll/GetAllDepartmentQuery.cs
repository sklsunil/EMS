using AutoMapper;
using AutoMapper.QueryableExtensions;

using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Department.Caching;
using EMS.Application.Features.Department.DTOs;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EMS.Application.Features.Department.Queries.GetAll;

public class GetAllDepartmentQuery : ICacheableRequest<IEnumerable<DepartmentDto>>
{
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => DepartmentCacheKey.MemoryCacheEntryOptions;
}

public class GetAllDepartmentQueryHandler :
     IRequestHandler<GetAllDepartmentQuery, IEnumerable<DepartmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllDepartmentQueryHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Departments
                     .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


