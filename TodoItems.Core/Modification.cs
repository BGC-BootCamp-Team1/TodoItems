using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Core
{
    public class Modification
    {
        //public string NewDescription { get; set; }
        //public string OldDescription { get; set; }
        public DateTime TimeStamp {  get; set; }

        public Modification()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
