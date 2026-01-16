
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using TaskApplication.Tasks.Handlers;
using TaskApplication.Common.Interfaces;
using TaskDomain;
using AutoMapper;
using TaskApplication.Tasks.Queries;
using Microsoft.Extensions.Logging.Abstractions; // Add this namespace
using MockQueryable.Moq;
using TaskApplication.Mappings;
using Microsoft.Extensions.Caching.Hybrid;
using TaskApplication.Tasks.DTOs;
using Azure;
using Moq.Protected;
using Microsoft.Extensions.Caching.Hybrid;
namespace TaskManagementTests.Helpers;



public class GetTaskByIdQueryHandlerTests
{
    private readonly IMapper _mapper;

    private readonly Mock<ITaskDbContext> _mockContext;
    private readonly List<TaskDomain.Task> _taskData;
    //private readonly Mock<HybridCache> _mockCache; 


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

        //_mockContext.Setup(c => c.Tasks).Returns(mockTaskSet.Object);
        //_mockContext.Setup(c => c.Status).Returns(mockStatusSet.Object);

        //_mockCache = new Mock<HybridCache>();

        // We use .Protected() because the core logic is not public
        //_mockCache.Protected()
        //    .Setup<ValueTask<TResponse>>(
        //        "GetOrCreateAsyncCore",
        //        ItExpr.IsAny<string>(),
        //        ItExpr.IsAny<object>(), // state
        //        ItExpr.IsAny<HybridCacheEntryOptions>(),
        //        ItExpr.IsAny<IEnumerable<string>>(),
        //        ItExpr.IsAny<CancellationToken>()
        //)
        //    .Returns(new ValueTask<TResponse>(/* logic to execute factory */));

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

        var result = await handler.Handle(new GetTaskByIdQuery(2), default);

        result.Should().BeNull();
    }
}