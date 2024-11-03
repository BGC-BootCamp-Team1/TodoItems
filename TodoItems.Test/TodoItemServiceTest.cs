using TodoItems.Core;
using Moq;
using TodoItems.Core.ApplicationExcepetions;
using TodoItems.Core.DueDateSettingStrategy;

namespace TodoItems.Test
{
    public class TodoItemServiceTest
    {
        [Fact]
        public void CreateItem_ShouldThrowException_WhenCreateNinthItem()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);

            var mockRepository = new Mock<ITodoItemsRepository>();

            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(It.IsAny<DateTime>()))
                .ReturnsAsync(8);

            var service = new TodoItemService(mockRepository.Object);
            
            Assert.Throws<ExceedMaxTodoItemsPerDueDateException>(() => service.CreateItem("test", dueDate));
        }

        [Fact]
        public void CreateItem_ShouldCreate_WhenCreateSecondItem()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);

            var mockRepository = new Mock<ITodoItemsRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(It.IsAny<DateTime>()))
                .ReturnsAsync(1);

            var service = new TodoItemService(mockRepository.Object);
            var actualTodoItem = service.CreateItem("test", dueDate);

            Assert.Equal("test", actualTodoItem.Description);
            Assert.Equal(dueDate, actualTodoItem.DueDate);
        }

        [Fact]
        public void CreateItem_ShouldThrowEexceptionWhenEarlierDueDate()
        {
            DateTime dueDate = DateTime.Now.AddDays(-10);

            var mockRepository = new Mock<ITodoItemsRepository>();
            mockRepository.Setup(repo => repo.CountTodoItemsOnTheSameDueDate(It.IsAny<DateTime>()))
                .ReturnsAsync(0);

            var service = new TodoItemService(mockRepository.Object);

            Assert.Throws<TooEarlyDueDateException>(() => service.CreateItem("test", dueDate));
        }
    }
}
