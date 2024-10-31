using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Core
{
    public class NotificationException : Exception
    {
        public NotificationException(string? message) : base(message)
        {
        }
    }
}
