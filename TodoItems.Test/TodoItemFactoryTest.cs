using Microsoft.VisualBasic;
using Moq;
using static TodoItems.Core.Constants;
using TodoItems.Core.ApplicationException;
using TodoItems.Core;

namespace TodoItems.Test
{
    public class TodoItemFactoryTest
    {
        private readonly Mock<ITodosRepository> _mockRepository = new Mock<ITodosRepository>();
        private const string _description = "test description";
        private readonly DateTime _dueDate = DateTime.Today.AddDays(10);

        [Fact]
        public void Should_throw_exception_when_exceed_due_date_limit()
        {
            var maxItemsPerDueDay = MAX_ITEMS_PER_DUE_DATE;
            _mockRepository.Setup(repo => repo.GetCountByDueDate(_dueDate)).Returns(maxItemsPerDueDay);
            var expectedErrMsg = $"Cannot create new Todo item completed on {_dueDate}, already reach max limit({maxItemsPerDueDay})";

            var exception = Assert.Throws<MaxItemsPerDueDateReachedException>(() => TodoItemFactory.CreateItem(_mockRepository.Object, _description, _dueDate, DueDateSetStrategy.Manual));
            Assert.Equal(expectedErrMsg, exception.Message);
        }

        [Fact]
        public void Should_not_create_item_when_due_date_earlier_than_creation_date()
        {
            var earlyDueDate = DateTime.Today.AddDays(-5);
            var expectedErrMsg = "Cannot create todo item that due date earlier than creation date";

            var exception = Assert.Throws<DueDateEarlierThanCreationDateException>(() => TodoItemFactory.CreateItem(_mockRepository.Object, _description, earlyDueDate, DueDateSetStrategy.Manual));
            Assert.Equal(expectedErrMsg, exception.Message);
        }

        [Fact]
        public void Should_create_item_when_due_date_is_null()
        {
            var newItem = TodoItemFactory.CreateItem(_mockRepository.Object, _description, null, DueDateSetStrategy.Manual);

            Assert.Equal(_description, newItem.Description);
            Assert.Null(newItem.DueDate);
        }

        [Fact]
        public void Should_return_item_when_due_date_set_to_today()
        {
            var newItem = TodoItemFactory.CreateItem(_mockRepository.Object, _description, DateTime.Today, DueDateSetStrategy.Manual);

            Assert.Equal(_description, newItem.Description);
        }

        [Fact]
        public void Should_return_item_with_correct_due_date_when_set_to_first_available_date()
        {
            MockRepoGetCount(DateTime.Today, new List<int> { 10, 14, 4, 15, 7 });

            var newItem = TodoItemFactory.CreateItem(_mockRepository.Object, _description, null, DueDateSetStrategy.FirstDateOfNextFiveDays);

            Assert.Equal(DateTime.Today.AddDays(2), newItem.DueDate);
        }

        [Fact]
        public void Should_throw_exception_with_no_available_due_date_when_set_to_first_available_date()
        {
            MockRepoGetCount(DateTime.Today, new List<int> { 10, 14, 14, 15, 17 });

            var expectedErrMsg = "Cannot create new Todo item with due date on next five days";

            var exception = Assert.Throws<NoAvailableDateInNextFiveDays>(() => TodoItemFactory.CreateItem(_mockRepository.Object, _description, null, DueDateSetStrategy.FirstDateOfNextFiveDays));

            Assert.Equal(expectedErrMsg, exception.Message);
        }

        [Fact]
        public void Should_return_item_with_correct_due_date_when_set_to_most_free_available_date()
        {
            MockRepoGetCount(DateTime.Today, new List<int> { 10, 1, 4, 15, 7 });

            var newItem = TodoItemFactory.CreateItem(_mockRepository.Object, _description, null, DueDateSetStrategy.MostFreeDateOfNextFiveDays);

            Assert.Equal(DateTime.Today.AddDays(1), newItem.DueDate);
        }

        [Fact]
        public void Should_throw_exception_with_no_available_due_date_when_set_to_most_free_available_dat()
        {
            MockRepoGetCount(DateTime.Today, new List<int> { 10, 14, 14, 15, 17 });

            var expectedErrMsg = "Cannot create new Todo item with due date on next five days";

            var exception = Assert.Throws<NoAvailableDateInNextFiveDays>(() => TodoItemFactory.CreateItem(_mockRepository.Object, _description, null, DueDateSetStrategy.FirstDateOfNextFiveDays));

            Assert.Equal(expectedErrMsg, exception.Message);
        }

        private void MockRepoGetCount(DateTime startDate, List<int> returnCounts)
        {
            for (int i = 0; i < returnCounts.Count; i++)
            {
                var date = startDate.AddDays(i);
                _mockRepository.Setup(repo => repo.GetCountByDueDate(date)).Returns(returnCounts[i]);
            }
        }
    }
}
