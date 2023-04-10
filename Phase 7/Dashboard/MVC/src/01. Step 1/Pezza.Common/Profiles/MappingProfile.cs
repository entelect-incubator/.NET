namespace Common.Profiles;

using AutoMapper;
using Common.DTO;
using Common.Entities;
using Common.Models.Base;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Customer, CustomerDTO>()
            .ForMember(x => x.Address, x => x.MapFrom((src) => new AddressBase()
            {
                Address = src.Address,
                City = src.City,
                Province = src.Province,
                PostalCode = src.PostalCode
            }))
            .ReverseMap();
        this.CreateMap<CustomerDTO, Customer>()
            .ForMember(vm => vm.Address, m => m.MapFrom(u => u.Address.Address))
            .ForMember(vm => vm.City, m => m.MapFrom(u => u.Address.City))
            .ForMember(vm => vm.Province, m => m.MapFrom(u => u.Address.Province))
            .ForMember(vm => vm.PostalCode, m => m.MapFrom(u => u.Address.PostalCode));

        this.CreateMap<Notify, NotifyDTO>();
        this.CreateMap<NotifyDTO, Notify>();

        this.CreateMap<OrderItem, OrderItemDTO>();
        this.CreateMap<OrderItemDTO, OrderItem>();

        this.CreateMap<Order, OrderDTO>()
            .ForMember(vm => vm.OrderItems, m => m.MapFrom(u => u.OrderItems));

        this.CreateMap<OrderDTO, Order>()
            .ForMember(vm => vm.OrderItems, m => m.MapFrom(u => u.OrderItems));

        this.CreateMap<Product, ProductDTO>();
        this.CreateMap<ProductDTO, Product>();

        this.CreateMap<Restaurant, RestaurantDTO>()
            .ForMember(x => x.Address, x => x.MapFrom((src) => new AddressBase()
            {
                Address = src.Address,
                City = src.City,
                Province = src.Province,
                PostalCode = src.PostalCode
            }))
            .ReverseMap();
        this.CreateMap<RestaurantDTO, Restaurant>()
            .ForMember(vm => vm.Address, m => m.MapFrom(u => u.Address.Address))
            .ForMember(vm => vm.City, m => m.MapFrom(u => u.Address.City))
            .ForMember(vm => vm.Province, m => m.MapFrom(u => u.Address.Province))
            .ForMember(vm => vm.PostalCode, m => m.MapFrom(u => u.Address.PostalCode));

        this.CreateMap<Stock, PizzaModel>();
        this.CreateMap<StockDTO, Stock>();
    }
}
