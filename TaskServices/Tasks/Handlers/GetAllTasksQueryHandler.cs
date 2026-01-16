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
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, List<TaskDTO>>
    {
        private readonly ITaskDbContext _context;
        private readonly IMapper _mapper;
        private readonly HybridCache _hybridCache;

        public GetAllTasksQueryHandler(ITaskDbContext context, IMapper mapper, HybridCache hybridCache)
        {
            _context = context;
            _mapper = mapper;
            _hybridCache = hybridCache;
        }
        /// <summary>
        /// Returns all the tasks i the database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "tasks-all";

            return await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async token =>
                {
                    return await _context.Tasks
                        .AsNoTracking()
                        .ProjectTo<TaskDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync(token);
                },
                cancellationToken: cancellationToken
            );
        }
    }
}
