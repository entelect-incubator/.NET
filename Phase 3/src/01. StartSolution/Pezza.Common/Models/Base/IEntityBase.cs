namespace Pezza.Common.Models.Base
{
    using Microsoft.EntityFrameworkCore.Metadata;

    public interface IEntityBase
    {
        int Id { get; set; }
    }
}
