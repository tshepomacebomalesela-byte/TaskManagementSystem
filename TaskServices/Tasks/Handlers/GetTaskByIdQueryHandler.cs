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
    /// <summary>
    /// Handler to get tasks by Id
    /// </summary>
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO?>
    {
        private readonly ITaskDbContext _context;
        private readonly IMapper _mapper;
        private readonly HybridCache _hybridCache;


        public GetTaskByIdQueryHandler(ITaskDbContext context, IMapper mapper, HybridCache hybridCache)
        {
            _context = context;
            _mapper = mapper;
            _hybridCache = hybridCache;

        }
        /// <summary>
        /// Method to retrieve task from cache if exists or database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TaskDTO?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"task-{request.Id}";

            return await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async token =>
                {
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
