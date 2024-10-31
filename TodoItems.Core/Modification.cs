namespace TodoItems.Core;

public class Modification
{
    public DateTimeOffset TimesStamp { get; set; }
    public Modification()
    {
        TimesStamp = DateTimeOffset.Now;
    }
}

