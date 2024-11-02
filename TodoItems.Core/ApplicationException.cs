using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoItems.Core;

public static class ApplicationException
{
    public static void ThrowTodoItemLimitExceededException()
    {
        throw new ArgumentException("You have reached the maximum number of todo items for this due date.");
    }

    public static void ThrowDuedateInPastException()
    {
        throw new ArgumentException("Due date cannot be in the past.");
    }

    public static void ThrowModificationsLimitTodayExceededException()
    {
        throw new ArgumentException("You have reached the maximum number of modifications for today.Please try again tomorrow.");
    }


}