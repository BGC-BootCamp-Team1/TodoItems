
using System;
using System.Collections.Generic;
using Moq;
using TodoItems.Core;
using Xunit;
using static TodoItems.Core.TodoItemService;

namespace TodoServiceTest;

public class TodoItemServiceTests
{
    private readonly Mock<ITodoRepository> _mockRepository;
    private readonly TodoItemService _service;

    public TodoItemServiceTests()
    {
        _mockRepository = new Mock<ITodoRepository>();
        _service = new TodoItemService(_mockRepository.Object);
    }

    [Fact]
    public void DetermineLastDueDate_NullDueDateAndNoneType_ReturnsNull()
    {
        var result = _service.DetermineLastDueDate(null, TodoItemService.DuedateType.None);
        Assert.Null(result);
    }


    [Fact]
    public void CreateTodoItem_ShouldCreateTodoItem_WhenDueDateIsValidAndCountIsLessThan8()
    {
        // Arrange
        
        _mockRepository.Setup(repo => repo.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(5);

        var service = new TodoItemService(_mockRepository.Object);
        var description = "Test Todo Item";
        var dueDate = DateTime.Now.AddDays(1);

        // Act
        var todoItem = service.CreateTodoItem(description, dueDate);

        // Assert
        Assert.NotNull(todoItem);
        Assert.Equal(description, todoItem.Description);
        Assert.Equal(dueDate, todoItem.DueDate);
        Assert.NotNull(todoItem.Id);
        _mockRepository.Verify(repo => repo.CountTodoItemsOnDueDate(dueDate), Times.Once);
    }

    [Fact]
    public void CreateTodoItem_ShouldThrowArgumentException_WhenDueDateIsInThePast()
    {
        // Arrange
        
        var service = new TodoItemService(_mockRepository.Object);
        var description = "Test Todo Item";
        var dueDate = DateTime.Now.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => service.CreateTodoItem(description, dueDate));
        Assert.Equal("Due date cannot be in the past.", exception.Message);
    }

    [Fact]
    public void CreateTodoItem_ShouldThrowArgumentException_WhenCountIs8OrMore()
    {
        // Arrange
      
        _mockRepository.Setup(repo => repo.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(8);

        var service = new TodoItemService(_mockRepository.Object);
        var description = "Test Todo Item";
        var dueDate = DateTime.Now.AddDays(1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => service.CreateTodoItem(description, dueDate));
        Assert.Equal("You have reached the maximum number of todo items for this due date.", exception.Message);
    }
}


