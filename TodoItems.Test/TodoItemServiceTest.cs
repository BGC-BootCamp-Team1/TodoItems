using TodoItems.Core;
using Moq;
using TodoItems.Core.ApplicationExcepetions;

namespace TodoItems.Test
{
    public class TodoItemServiceTest
    {
        [Fact]
        public void Should_throw_exception_when_create_ninth_item()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);

            var mockRepository = new Mock<ITodoItemsRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(dueDate))
                .Returns(8);

            var service = new TodoItemService(mockRepository.Object);
            
            Assert.Throws<ExceedMaxTodoItemsPerDueDateException>(() => service.CreateItem("test", dueDate));
        }

        [Fact]
        public void Should_create_when_create_second_item()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);

            var mockRepository = new Mock<ITodoItemsRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(dueDate))
                .Returns(1);

            var service = new TodoItemService(mockRepository.Object);
            var actualTodoItem = service.CreateItem("test", dueDate);

            Assert.Equal("test", actualTodoItem.Description);
            Assert.Equal(dueDate, actualTodoItem.DueDate);
        }

        [Fact]
        public void Should_throw_exception_when_earlier_due_date()
        {
            DateTime dueDate = DateTime.Now.AddDays(-10);

            var mockRepository = new Mock<ITodoItemsRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(dueDate))
                .Returns(0);

            var service = new TodoItemService(mockRepository.Object);

            Assert.Throws<TooEarlyDueDateException>(() => service.CreateItem("test", dueDate));
        }
    }
}
