namespace TodoItems.Core.AppException
{
    public class MaximumModificationException(string? message) : Exception(message);
    public class DueDayEarlyException(string? message) : Exception(message);
    public class MaximumSameDueDayException(string? message) : Exception(message);
}
