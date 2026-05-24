namespace Common.Profiles;

using AutoMapper;
using Common.DTO;
using Common.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDTO>().ReverseMap();
        CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        CreateMap<Order, OrderDTO>().ReverseMap();
        CreateMap<Notify, NotifyDTO>().ReverseMap();
        CreateMap<Stock, PizzaModel>().ReverseMap();
    }
}
