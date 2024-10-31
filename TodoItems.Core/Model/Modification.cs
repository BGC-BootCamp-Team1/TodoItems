namespace TodoItems.Core.Model
{
    public class Modification
    {
        //public string NewDescription { get; set; }
        //public string OldDescription { get; set; }
        public DateTime TimeStamp { get; set; }

        public Modification()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
