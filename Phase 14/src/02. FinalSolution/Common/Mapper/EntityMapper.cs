namespace Common.Mapper;

using System;
using System.Collections.Generic;
using System.Linq;
using Common.DTO;
using Common.Entities;
using Common.Models.Base;

public static class EntityMapper
{
    public static CustomerDTO ToDto(this Customer entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new CustomerDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Phone = entity.Phone,
            Email = entity.Email,
            ContactPerson = entity.ContactPerson,
            DateCreated = entity.DateCreated,
            Address = new AddressBase
            {
                Address = entity.Address,
                City = entity.City,
                Province = entity.Province,
                PostalCode = entity.PostalCode,
            },
        };
    }

    public static Customer ToEntity(this CustomerDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Customer
        {
            Id = dto.Id,
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            ContactPerson = dto.ContactPerson,
            DateCreated = dto.DateCreated ?? DateTime.UtcNow,
            Address = $"{dto.Address?.Address}, {dto.Address?.City}, {dto.Address?.Province}, {dto.Address?.PostalCode}",
            City = dto.Address?.City,
            Province = dto.Address?.Province,
            PostalCode = dto.Address?.PostalCode,
        };
    }

    public static NotifyDTO ToDto(this Notify entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new NotifyDTO
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            Email = entity.Email,
            Sent = entity.Sent,
            Retry = entity.Retry,
            DateSent = entity.DateSent,
        };
    }

    public static Notify ToEntity(this NotifyDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Notify
        {
            Id = dto.Id,
            CustomerId = dto.CustomerId ?? 0,
            Email = dto.Email,
            Sent = dto.Sent ?? false,
            Retry = dto.Retry ?? 0,
            DateSent = dto.DateSent ?? DateTime.UtcNow,
        };
    }

    public static OrderItemDTO ToDto(this OrderItem entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new OrderItemDTO
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Product = entity.Product?.ToDto(),
        };
    }

    public static OrderItem ToEntity(this OrderItemDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new OrderItem
        {
            Id = dto.Id,
            OrderId = dto.OrderId ?? 0,
            ProductId = dto.ProductId ?? 0,
            Quantity = dto.Quantity ?? 0,
        };
    }

    public static OrderDTO ToDto(this Order entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new OrderDTO
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId,
            RestaurantId = entity.RestaurantId,
            Amount = entity.Amount,
            Completed = entity.Completed,
            DateCreated = entity.DateCreated,
            DeliveryId = entity.DeliveryId,
            DeliveryStatus = entity.DeliveryStatus,
            Customer = entity.Customer?.ToDto(),
            Restaurant = entity.Restaurant?.ToDto(),
            OrderItems = entity.OrderItems?.Select(x => x.ToDto()).ToList(),
        };
    }

    public static Order ToEntity(this OrderDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Order
        {
            Id = dto.Id,
            CustomerId = dto.CustomerId ?? 0,
            RestaurantId = dto.RestaurantId ?? 0,
            Amount = dto.Amount ?? 0,
            Completed = dto.Completed ?? false,
            DateCreated = dto.DateCreated ?? DateTime.UtcNow,
            DeliveryId = dto.DeliveryId,
            DeliveryStatus = dto.DeliveryStatus,
        };
    }

    public static ProductDTO ToDto(this Product entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new ProductDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            PictureUrl = entity.PictureUrl,
            Price = entity.Price,
            Special = entity.Special,
            OfferEndDate = entity.OfferEndDate,
            OfferPrice = entity.OfferPrice,
            IsActive = entity.IsActive,
            DateCreated = entity.DateCreated,
        };
    }

    public static Product ToEntity(this ProductDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            PictureUrl = dto.PictureUrl,
            Price = dto.Price ?? 0,
            Special = dto.Special ?? false,
            OfferEndDate = dto.OfferEndDate,
            OfferPrice = dto.OfferPrice,
            IsActive = dto.IsActive ?? false,
            DateCreated = dto.DateCreated,
        };
    }

    public static RestaurantDTO ToDto(this Restaurant entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new RestaurantDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            PictureUrl = entity.PictureUrl,
            IsActive = entity.IsActive,
            DateCreated = entity.DateCreated,
            Address = new AddressBase
            {
                Address = entity.Address,
                City = entity.City,
                Province = entity.Province,
                PostalCode = entity.PostalCode,
            },
        };
    }

    public static Restaurant ToEntity(this RestaurantDTO dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Restaurant
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            PictureUrl = dto.PictureUrl,
            IsActive = dto.IsActive ?? false,
            DateCreated = dto.DateCreated,
            Address = dto.Address?.Address,
            City = dto.Address?.City,
            Province = dto.Address?.Province,
            PostalCode = dto.Address?.PostalCode,
        };
    }

    public static PizzaModel ToDto(this Stock entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new PizzaModel
        {
            Id = entity.Id,
            Name = entity.Name,
            UnitOfMeasure = entity.UnitOfMeasure,
            ValueOfMeasure = entity.ValueOfMeasure,
            Quantity = entity.Quantity,
            ExpiryDate = entity.ExpiryDate,
            Comment = entity.Comment,
        };
    }

    public static Stock ToEntity(this PizzaModel dto)
    {
        if (dto is null)
        {
            return null;
        }

        return new Stock
        {
            Id = dto.Id,
            Name = dto.Name,
            UnitOfMeasure = dto.UnitOfMeasure,
            ValueOfMeasure = dto.ValueOfMeasure,
            Quantity = dto.Quantity ?? 0,
            ExpiryDate = dto.ExpiryDate,
            Comment = dto.Comment,
        };
    }
}
