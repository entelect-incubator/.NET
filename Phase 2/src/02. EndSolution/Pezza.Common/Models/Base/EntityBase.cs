namespace Pezza.Common.Models.Base
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore.Metadata;

    public abstract class EntityBase : IEntityBase
    {
        public virtual int Id { get; set; }
    }
}
