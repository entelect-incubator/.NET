namespace Portal.Models;

public class SearchModel<T>
{
    public T SearchData { get; set; }

    public int Limit { get; set; }

    public int Page { get; set; }

    public string OrderBy { get; set; }
}
