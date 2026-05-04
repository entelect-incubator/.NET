namespace Common.Models;

public class PagingArgs
{
    private int limit = 20;

    public static PagingArgs NoPaging => new() { UsePaging = false };

    public static PagingArgs Default => new() { UsePaging = true, Limit = 20, Offset = 0 };

    public static PagingArgs FirstItem => new() { UsePaging = true, Limit = 1, Offset = 0 };

    public int Offset { get; set; }

    public int Limit
    {
        get => this.limit;

        set
        {
            if (value == 0)
            {
                value = 20;
            }

            this.limit = value;
        }
    }

    public bool UsePaging { get; set; }
}