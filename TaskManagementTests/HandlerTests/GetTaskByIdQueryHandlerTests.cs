
using Moq;
using FluentAssertions;
using TaskApplication.Tasks.Handlers;
using TaskApplication.Common.Interfaces;
using TaskDomain;
using AutoMapper;
using TaskApplication.Tasks.Queries;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using TaskApplication.Mappings;
using TaskManagementTests.Helpers;



public class GetTaskByIdQueryHandlerTests
{
    private readonly IMapper _mapper;

    private readonly Mock<ITaskDbContext> _mockContext;
    private readonly List<TaskDomain.Task> _taskData; 

    public GetTaskByIdQueryHandlerTests()
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
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_Should_Return_TaskDTO_When_Task_Exists()
    {
        var passthroughCache = new PassthroughHybridCache();

        _taskData.Add(new TaskDomain.Task 
        { 
            Id = 2, 
            Name = "Existing Task", 
            StatusID = 1, 
            Description = "Description", 
            Status = new Status { Id = 1, Name = "Open" } 
        });

        var mockTaskSet = _taskData.BuildMockDbSet();
        _mockContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);

        var handler = new GetTaskByIdQueryHandler(_mockContext.Object, _mapper, passthroughCache);

        var result = await handler.Handle(new GetTaskByIdQuery(2), default);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Existing Task");
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_Should__Not_Return_TaskDTO_When_Task_Does_NotExists()
    {
        var passthroughCache = new PassthroughHybridCache();

        _taskData.Add(new TaskDomain.Task
        {
            Id = 2,
            Name = "Existing Task",
            StatusID = 1,
            Description = "Description",
            Status = new Status { Id = 1, Name = "Open" }
        });

        var mockTaskSet = _taskData.BuildMockDbSet();
        _mockContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);

        var handler = new GetTaskByIdQueryHandler(_mockContext.Object, _mapper, passthroughCache);

        var result = await handler.Handle(new GetTaskByIdQuery(3), default);

        result.Should().BeNull();
    }
}