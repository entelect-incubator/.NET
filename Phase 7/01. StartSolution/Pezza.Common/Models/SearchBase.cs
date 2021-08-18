namespace Pezza.Common.DTO.Data
{
    using Pezza.Common.Models;

    public class SearchBase
    {
        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
