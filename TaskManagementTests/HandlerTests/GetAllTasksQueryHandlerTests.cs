using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using Moq;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.Handlers;
using TaskApplication.Tasks.Queries;
using TaskApplication.Mappings;
using TaskDomain;
using AutoMapper;
using TaskManagementTests.Helpers;

namespace TaskManagementTests.HandlerTests
{
    public class GetAllTasksQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ITaskDbContext> _mockContext;
        private readonly PassthroughHybridCache _passthroughCache;
        private readonly List<TaskDomain.Task> _taskData;


        public GetAllTasksQueryHandlerTests()
        {
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(myProfile);
            }, NullLoggerFactory.Instance);

            _mapper = configuration.CreateMapper();

            _mockContext = new Mock<ITaskDbContext>();
            _taskData = new List<TaskDomain.Task>
            {
                new TaskDomain.Task { Id = 1, Name = "Test Task", StatusID = 1 }
            };
                var statusData = new List<Status>()
            {
                new Status { Id = 1, Name = "Open" },
                new Status { Id = 2, Name = "Closed" },
                new Status { Id = 3, Name = "Pending" },
                new Status { Id = 4, Name = "On-Hold" }
            };

            var mockTaskSet = _taskData.BuildMockDbSet();
            var mockStatusSet = statusData.BuildMockDbSet();

            _mockContext = new Mock<ITaskDbContext>();
            _passthroughCache = new PassthroughHybridCache();

        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Return_List_Of_TaskDTOs_When_Tasks_Exist()
        {
            var taskData = new List<TaskDomain.Task>
            {
                new TaskDomain.Task
                {
                    Id = 1,
                    Name = "Task 1",
                    StatusID = 1,
                    Status = new Status { Id = 1, Name = "Open" }
                },
                new TaskDomain.Task
                {
                    Id = 2,
                    Name = "Task 2",
                    StatusID = 2,
                    Status = new Status { Id = 2, Name = "Closed" }
                }
            };

            var mockTaskSet = taskData.BuildMockDbSet();
            _mockContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);

            var handler = new GetAllTasksQueryHandler(_mockContext.Object, _mapper, _passthroughCache);
            var query = new GetAllTasksQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Name == "Task 1" && t.StatusName == "Open");
            result.Should().Contain(t => t.Name == "Task 2" && t.StatusName == "Closed");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_Should_Return_Empty_List_When_No_Tasks_Exist()
        {
            var taskData = new List<TaskDomain.Task>();
            var mockTaskSet = taskData.BuildMockDbSet();
            _mockContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);

            var handler = new GetAllTasksQueryHandler(_mockContext.Object, _mapper, _passthroughCache);

            var result = await handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
