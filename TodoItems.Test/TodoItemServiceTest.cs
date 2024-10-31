using TodoItems.Core;
using Moq;

namespace TodoItems.Test
{
    public class TodoItemServiceTest
    {
        [Fact]
        public void should_throw_exception_when_create_nineth_item()
        {
            DateTime dueDate = new DateTime(2024,10,31);
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
            
            Assert.Throws<Exception>(() => service.createItem("test", dueDate));
        }

        [Fact]
        public void should_create_when_create_second_item()
        {
            DateTime dueDate = new DateTime(2024, 10, 31);
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
    }
}
