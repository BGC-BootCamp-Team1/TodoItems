using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace TodoItems.Core.Tests
{
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
        public void DetermineLastDueDate_NullDueDateAndFirstAvailableDayType_ReturnsFirstAvailableDay()
        {
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(0);
            var result = _service.DetermineLastDueDate(null, TodoItemService.DuedateType.FirstAvailableDay);
            Assert.NotNull(result);
            Assert.InRange(result.Value, DateTime.Today.AddDays(1), DateTime.Today.AddDays(5));
        }

        [Fact]
        public void DetermineLastDueDate_NullDueDateAndFewestDayWithIn5DaysType_ReturnsDayWithFewestItems()
        {
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(It.IsAny<DateTime>())).Returns(0);
            var result = _service.DetermineLastDueDate(null, TodoItemService.DuedateType.FewestDayWithIn5Days);
            Assert.NotNull(result);
            Assert.InRange(result.Value, DateTime.Today.AddDays(1), DateTime.Today.AddDays(5));
        }

        [Fact]
        public void DetermineLastDueDate_ValidDueDate_ReturnsDueDate()
        {
            var dueDate = DateTime.Today.AddDays(1);
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(dueDate)).Returns(0);
            var result = _service.DetermineLastDueDate(dueDate, TodoItemService.DuedateType.None);
            Assert.Equal(dueDate, result);
        }

        [Fact]
        public void DetermineLastDueDate_ValidDueDateWithTooManyItems_ThrowsException()
        {
            var dueDate = DateTime.Today.AddDays(1);
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(dueDate)).Returns(Constants.MaxTodoItemsPerDay);
            Assert.Throws<ArgumentException>(() => _service.DetermineLastDueDate(dueDate, TodoItemService.DuedateType.None));
        }

        [Fact]
        public void DetermineLastDueDate_InvalidDueDate_ThrowsException()
        {
            var dueDate = DateTime.Today.AddDays(-1);
            Assert.Throws<ArgumentException>(() => _service.DetermineLastDueDate(dueDate, TodoItemService.DuedateType.None));
        }

        [Fact]
        public void CreateTodoItem_ValidDueDate_AddsTodoItem()
        {
            var dueDate = DateTime.Today.AddDays(1);
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(dueDate)).Returns(0);
            var todoItem = _service.CreateTodoItem("Test Description", dueDate);
            Assert.Contains(todoItem, _service.TodoItems);
        }

        [Fact]
        public void CreateTodoItem_TooManyItemsOnDueDate_ThrowsException()
        {
            var dueDate = DateTime.Today.AddDays(1);
            _mockRepository.Setup(r => r.CountTodoItemsOnDueDate(dueDate)).Returns(Constants.MaxTodoItemsPerDay);
            Assert.Throws<ArgumentException>(() => _service.CreateTodoItem("Test Description", dueDate));
        }

        [Fact]
        public void CreateTodoItem_InvalidDueDate_ThrowsException()
        {
            var dueDate = DateTime.Today.AddDays(-1);
            Assert.Throws<ArgumentException>(() => _service.CreateTodoItem("Test Description", dueDate));
        }
    }
}
