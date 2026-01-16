using Moq;
using MockQueryable.Moq;
using FluentAssertions;
using TaskApplication.Tasks.Commands;
using TaskApplication.Tasks.Handlers;
using TaskApplication.Common.Interfaces;
using TaskDomain;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskDbContext> _mockContext;
    private readonly CreateTasksCommandHandler _handler;


    public CreateTaskCommandHandlerTests()
    {
        var taskList = new List<TaskDomain.Task>();

        var mockDbSet = taskList.BuildMockDbSet();

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

        _handler = new CreateTasksCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async void Handle_Should_Add_Task_To_Database()
    {
        var command = new CreateTaskCommand("Test Task", "Desc", 1);
        var handler = new CreateTasksCommandHandler(_mockContext.Object);

        await handler.Handle(command, default);

        _mockContext.Verify(x => x.Tasks.Add(It.IsAny<TaskDomain.Task>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void Handle_Should_Faile_To_Add_Task_To_Database_With_Incorrect_Status()
    {
        var command = new CreateTaskCommand("Test Task", "Desc", 5);
        var handler = new CreateTasksCommandHandler(_mockContext.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
         handler.Handle(command, CancellationToken.None));

        _mockContext.Verify(x => x.Tasks.Add(It.IsAny<TaskDomain.Task>()), Times.Never);
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}