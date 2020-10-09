namespace Pezza.Test
{
    using System;
    using System.Collections.Generic;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class OrderTestData
    {
        public static Faker faker = new Faker();

        public static List<Order> Orders()
        {
            var orders = new List<Order>
            {
                Order,
                Order,
                Order,
                Order,
                Order
            };

            return orders;
        }

        public static Order Order = new Order()
        {
            Amount = faker.Finance.Amount(),
            Customer = CustomerTestData.Customer,
            CustomerId = 1,
            DateCreated = DateTime.Now,
            OrderItems = OrderItems()
        };

        public static List<OrderItem> OrderItems()
        {
            var orderItems = new List<OrderItem>
            {
                OrderItem,
                OrderItem,
                OrderItem,
                OrderItem,
                OrderItem
            };

            return orderItems;
        }

        public static OrderItem OrderItem = new OrderItem()
        {
            OrderId = 1,
            Product = ProductTestData.Product
        };

        public static List<OrderDTO> OrdersDTO()
        {
            var orders = new List<OrderDTO>
            {
                OrderDTO,
                OrderDTO,
                OrderDTO,
                OrderDTO,
                OrderDTO
            };

            return orders;
        }

        public static OrderDTO OrderDTO = new OrderDTO()
        {
            Amount = faker.Finance.Amount(),
            Customer = CustomerTestData.CustomerDTO,
            CustomerId = 1,
            DateCreated = DateTime.Now,
            OrderItems = OrderItemsDTO()
        };

        public static List<OrderItemDTO> OrderItemsDTO()
        {
            var orderItems = new List<OrderItemDTO>
            {
                OrderItemDTO,
                OrderItemDTO,
                OrderItemDTO,
                OrderItemDTO,
                OrderItemDTO
            };

            return orderItems;
        }

        public static OrderItemDTO OrderItemDTO = new OrderItemDTO()
        {
            OrderId = 1,
            Product = ProductTestData.ProductDTO
        };
    }

}
