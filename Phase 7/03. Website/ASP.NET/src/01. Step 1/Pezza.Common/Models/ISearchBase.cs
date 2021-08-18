namespace Pezza.Common.DTO.Data
{
    using Pezza.Common.Models;

    public interface ISearchBase
    {
        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
