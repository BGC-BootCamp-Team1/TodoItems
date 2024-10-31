namespace TodoItems.Core.BizException
{
    public class NotificationException : Exception
    {
        public NotificationException(string? message) : base(message)
        {
        }
    }
}
