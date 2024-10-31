public class MaxItemsPerDueDateReachedException : Exception
{
    public MaxItemsPerDueDateReachedException(DateTime dueDate, int maxItems)
        : base($"Cannot create new Todo item completed on { dueDate }, already reach max limit({ maxItems})")
    {
    }

    public MaxItemsPerDueDateReachedException(string message)
        : base(message)
    {
    }

    public MaxItemsPerDueDateReachedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
