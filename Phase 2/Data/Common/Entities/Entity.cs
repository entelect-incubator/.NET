namespace Pezza.Common.Entities
{
    using Pezza.Common.Models;

    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
