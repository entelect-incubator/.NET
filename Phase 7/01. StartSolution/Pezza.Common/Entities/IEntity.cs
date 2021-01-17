namespace Pezza.Common.Entities
{
    using Pezza.Common.DTO.Data;

    public interface IEntity : ISearchBase
    {
        int Id { get; set; }
    }
}
