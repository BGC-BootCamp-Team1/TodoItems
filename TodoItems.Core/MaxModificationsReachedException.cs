public class MaxModificationsReachedException : Exception
{
    public MaxModificationsReachedException(string message)
        : base(message)
    {
    }

    public MaxModificationsReachedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
