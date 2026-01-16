using AutoMapper;
using MediatR;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.DTOs;
using TaskApplication.Tasks.Queries;
using Microsoft.Extensions.Caching.Hybrid;

namespace TaskApplication.Tasks.Handlers
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO?>
    {
        private readonly ITaskDbContext _context;
        private readonly IMapper _mapper;
        private readonly HybridCache _hybridCache; // 1. Define the field


        public GetTaskByIdQueryHandler(ITaskDbContext context, IMapper mapper, HybridCache hybridCache)
        {
            _context = context;
            _mapper = mapper;
            _hybridCache = hybridCache;

        }

        public async Task<TaskDTO?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            //var task = await _hybridCache.GetOrCreateAsync(
            //    $"task-{id}",
            //    async token => await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, token),
            //    cancellationToken: ct
            //);
            // Use ProjectTo for optimal SQL generation (SELECT only needed columns)
            //return await _context.Tasks
            //.AsNoTracking()
            //    .Where(t => t.Id == request.Id)
            //    .ProjectTo<TaskDTO>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(cancellationToken);
            string cacheKey = $"task-{request.Id}";

            // 3. Use GetOrCreateAsync for a seamless L1/L2 experience
            return await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async token =>
                {
                    // This code only runs if the cache is empty (Cache Miss)
                    return await _context.Tasks
                        .AsNoTracking()
                        .Where(t => t.Id == request.Id)
                        .ProjectTo<TaskDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(token);
                },
                cancellationToken: cancellationToken
            );
        }
    }
}
