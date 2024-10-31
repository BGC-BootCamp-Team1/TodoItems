using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Core
{
    public class Modification
    {
        public DateTime ModificationTimestamp { get; private set; }
        public Modification(DateTime date)
        {
            this.ModificationTimestamp = date;
        }
    }
}
