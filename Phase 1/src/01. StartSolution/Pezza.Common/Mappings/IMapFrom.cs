namespace Pezza.Application.Common.Mappings
{
    using AutoMapper;

    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), this.GetType());
    }
}
