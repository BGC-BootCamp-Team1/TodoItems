using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoItems.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualBasic;
using TodoItems.Core.AppException;
using static TodoItems.Core.TodoItemService;

namespace TodoItems.Core.Tests
{
    [TestClass()]
    public class TodoItemServiceTests
    {
        [TestMethod]
        public void Create_ShouldReturnNewTodoItem_WhenDueDateIsValidAndCountIsLessThan8()
        {
            // Arrange
            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsByDueDate(It.IsAny<DateOnly>())).Returns(5);

            var service = new TodoItemService(mockRepository.Object);
            var description = "Test Todo Item";
            var dueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

            // Act
            var result = service.CreateAsync(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(description, result.Result.Description);
            Assert.AreEqual(dueDate, result.Result.DueDate);
            mockRepository.Verify(repo => repo.CountTodoItemsByDueDate(dueDate), Times.Once);
        }

        [TestMethod]
        public void Create_ShouldThrowException_WhenDueDateIsInvalid()
        {
            // Arrange
            var mockRepository = new Mock<ITodosRepository>();
            var service = new TodoItemService(mockRepository.Object);
            var description = "Test Todo Item";
            var dueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

            // Act & Assert
            Assert.ThrowsExceptionAsync<InvalidDueDateException>(() => service.CreateAsync(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption));
            
        }

        [TestMethod]
        public void Create_ShouldThrowException_WhenCountIs8OrMore()
        {
            // Arrange
            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsByDueDate(It.IsAny<DateOnly>())).Returns(8);

            var service = new TodoItemService(mockRepository.Object);
            var description = "Test Todo Item";
            var dueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

            // Act & Assert
            Assert.ThrowsExceptionAsync<ExceedMaxTodoItemsPerDueDateException>(() => service.CreateAsync(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption));
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateTodoItem_WithNextAvailableInFiveDaysOption()
        {
            // Arrange
            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsInFiveDays())
                          .Returns(new List<int> { 8, 6, 5, 4, 3, 8 });
            mockRepository.Setup(repo => repo.SaveAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

            var service = new TodoItemService(mockRepository.Object);
            var description = "Test Todo Item";

            // Act
            var result = await service.CreateAsync(description, null, CreateOptionEnum.NextAvailableInFiveDaysOption);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(description, result.Description);
            Assert.AreEqual(DateOnly.FromDateTime(DateTime.Today.AddDays(1)), result.DueDate);
            mockRepository.Verify(repo => repo.SaveAsync(It.IsAny<TodoItem>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateTodoItem_WithMostAvailableInFiveDaysOption()
        {
            // Arrange
            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsInFiveDays())
                          .Returns(new List<int> { 8, 6, 5, 4, 3, 8 });

            mockRepository.Setup(repo => repo.SaveAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

            var service = new TodoItemService(mockRepository.Object);
            var description = "Test Todo Item";

            // Act
            var result = await service.CreateAsync(description, null, CreateOptionEnum.MostAvailableInFiveDaysOption);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(description, result.Description);
            Assert.AreEqual(DateOnly.FromDateTime(DateTime.Today.AddDays(4)), result.DueDate);
            mockRepository.Verify(repo => repo.SaveAsync(It.IsAny<TodoItem>()), Times.Once);
        }

    }
}

