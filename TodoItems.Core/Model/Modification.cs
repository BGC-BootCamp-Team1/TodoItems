namespace TodoItems.Core.Model
{
    public class Modification
    {
        public DateTime TimeStamp { get; set; }

        public Modification()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
