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
            var result = service.Create(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(description, result.Description);
            Assert.AreEqual(dueDate, result.DueDate);
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
            Assert.ThrowsException<InvalidDueDateException>(() => service.Create(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption));
            
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
            Assert.ThrowsException<ExceedMaxTodoItemsPerDueDateException>(() => service.Create(description, dueDate, TodoItemService.CreateOptionEnum.ManualOption));
        }
    }
}

