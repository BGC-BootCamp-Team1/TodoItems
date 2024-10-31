using TodoItems.Core;
using Moq;

namespace TodoItems.Test
{
    public class TodoItemServiceTest
    {
        [Fact]
        public void should_throw_exception_when_create_nineth_item()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);
            List<TodoItem> mockTodoItems = new List<TodoItem>();
            for (int i = 0; i < 8; i++)
            {
                TodoItem item = new TodoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = "Task " + (i + 1),
                    Modifications = [],
                    DueDate = dueDate
                };
                mockTodoItems.Add(item);
            }

            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.FindAllTodoItemsHaveTheSameDueDate(dueDate))
                .Returns(mockTodoItems);

            var service = new TodoItemService(mockRepository.Object);
            
            Assert.Throws<ExceedMaxTodoItemsPerDueDateException>(() => service.createItem("test", dueDate));
        }

        [Fact]
        public void should_create_when_create_second_item()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);
            List<TodoItem> mockTodoItems = new List<TodoItem>();
            for (int i = 0; i < 1; i++)
            {
                TodoItem item = new TodoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = "Task " + (i + 1),
                    Modifications = [],
                    DueDate = dueDate
                };
                mockTodoItems.Add(item);
            }

            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.FindAllTodoItemsHaveTheSameDueDate(dueDate))
                .Returns(mockTodoItems);

            var service = new TodoItemService(mockRepository.Object);
            var actualTodoItem = service.createItem("test", dueDate);
            var expectedTodoItem = new TodoItem
            {
                Id = Guid.NewGuid().ToString(),
                Description = "test",
                Modifications = [],
                DueDate = dueDate
            };

            Assert.Equal(actualTodoItem.Description, expectedTodoItem.Description);
            Assert.Equal(actualTodoItem.DueDate, expectedTodoItem.DueDate);
        }

        [Fact]
        public void should_throw_exception_when_earlier_due_date()
        {
            DateTime dueDate = DateTime.Now.AddDays(-10);
            List<TodoItem> mockTodoItems = new List<TodoItem>();

            var mockRepository = new Mock<ITodosRepository>();
            mockRepository.Setup(repo => repo.FindAllTodoItemsHaveTheSameDueDate(dueDate))
                .Returns(mockTodoItems);

            var service = new TodoItemService(mockRepository.Object);

            Assert.Throws<TooEarlyDueDateException>(() => service.createItem("test", dueDate));
        }
    }
}
