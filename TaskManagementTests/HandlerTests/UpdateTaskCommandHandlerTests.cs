using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using Moq;
using System.Reflection.Metadata;
using TaskApplication.Common.Interfaces;
using TaskApplication.Tasks.Commands;
using TaskApplication.Tasks.Handlers;
using TaskDomain;


public class UpdateTaskCommandHandlerTests
{

    private readonly Mock<ITaskDbContext> _mockContext;
    private readonly List<TaskDomain.Task> _taskData;

    public UpdateTaskCommandHandlerTests()
    {
        var taskList = new List<TaskDomain.Task>
        {
            new TaskDomain.Task { Id = 1, Name = "Test Task", StatusID = 1 }
        };

        var mockDbSet = taskList.BuildMockDbSet();
        taskList = new List<TaskDomain.Task>
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

        var mockStatusSet = statusData.BuildMockDbSet();

        _mockContext = new Mock<ITaskDbContext>();

        _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);
        _mockContext.Setup(c => c.Status).Returns(mockStatusSet.Object);

    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_Should_Update_Task_When_Task_Exists()
    {
        var handler = new UpdateTaskCommandHandler(_mockContext.Object);
        var command = new UpdateTaskCommand(1, "New Name", "New Desc", 2);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();

        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_Should_Return_False_When_Task_Does_Not_Exist()
    {
        var handler = new UpdateTaskCommandHandler(_mockContext.Object);
        var command = new UpdateTaskCommand(99, "Name", "Desc", 1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

