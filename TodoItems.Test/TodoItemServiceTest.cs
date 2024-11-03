﻿using TodoItems.Core;
using Moq;
using Moq.Protected;
using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationException;
using static TodoItems.Core.Constants;

namespace TodoItems.Test;

public class TodoItemServiceTest
{
    private readonly Mock<ITodosRepository> _mockRepository = new Mock<ITodosRepository>();
    private const string _description = "test description";
    private readonly DateTime _dueDate = DateTime.Today.AddDays(10);

    [Fact]
    public void Should_create_todo_item()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, _dueDate, DueDateSetStrategy.Manual);

        Assert.Equal(_description, newItem.Description);
        Assert.Equal(_dueDate, newItem.DueDate);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<TodoItem>()), Times.Once);
    }

}
