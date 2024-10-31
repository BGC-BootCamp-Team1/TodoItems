namespace TodoItems.Core
{
    public class DueDateEarlierThanCreationDateException : Exception
    {
        public DueDateEarlierThanCreationDateException()
            : base("Cannot create todo item that due date earlier than creation date")
        {
        }


        public DueDateEarlierThanCreationDateException(string message)
            : base(message)
        {
        }

        public DueDateEarlierThanCreationDateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}