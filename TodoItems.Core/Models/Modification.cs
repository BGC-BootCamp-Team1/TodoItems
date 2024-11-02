namespace TodoItems.Core
{
    public class Modification
    {
        public DateTime ModificationTimestamp { get; private set; }
        public Modification(DateTime date)
        {
            ModificationTimestamp = date;
        }
    }
}
