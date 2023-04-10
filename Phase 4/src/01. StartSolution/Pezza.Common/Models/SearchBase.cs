namespace Common.Models;

using Common.Models.Base;

public class SearchBase : EntityBase
{
    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
}
