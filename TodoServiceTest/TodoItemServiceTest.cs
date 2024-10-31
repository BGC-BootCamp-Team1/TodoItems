
using System;
using System.Collections.Generic;
using Moq;
using TodoItems.Core;
using Xunit;

namespace TodoServiceTest;

public class TodoItemServiceTests
{
    [Fact]
    public void CreateTodoItem_ShouldCreateTodoItem_WhenDueDateIsValidAndCountIsLessThan8()
    {
        // Arrange
        var mockRepository = new Mock<ITodoRepository>();
        mockRepository.Setup(repo => repo.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(5);

        var service = new TodoItemService(mockRepository.Object);
        var description = "Test Todo Item";
        var dueDate = DateTime.Now.AddDays(1);

        // Act
        var todoItem = service.CreateTodoItem(description, dueDate);

        // Assert
        Assert.NotNull(todoItem);
        Assert.Equal(description, todoItem.Description);
        Assert.Equal(dueDate, todoItem.DueDate);
        Assert.NotNull(todoItem.Id);
        mockRepository.Verify(repo => repo.CountTodoItemsOnDueDate(dueDate), Times.Once);
    }

    [Fact]
    public void CreateTodoItem_ShouldThrowArgumentException_WhenDueDateIsInThePast()
    {
        // Arrange
        var mockRepository = new Mock<ITodoRepository>();
        var service = new TodoItemService(mockRepository.Object);
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
        var mockRepository = new Mock<ITodoRepository>();
        mockRepository.Setup(repo => repo.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(8);

        var service = new TodoItemService(mockRepository.Object);
        var description = "Test Todo Item";
        var dueDate = DateTime.Now.AddDays(1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => service.CreateTodoItem(description, dueDate));
        Assert.Equal("You have reached the maximum number of todo items for this due date.", exception.Message);
    }
}


