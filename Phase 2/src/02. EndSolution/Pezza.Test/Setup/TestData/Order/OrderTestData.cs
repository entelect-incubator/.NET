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
            Restaurant = RestaurantTestData.Restaurant,
            RestaurantId = 1,
            DateCreated = DateTime.Now,
            OrderItems = OrderItems()
        };

        public static List<OrderItem> OrderItems()
        {
            var orderItems = new List<OrderItem>
            {
                OrderItem(1),
                OrderItem(2),
                OrderItem(3),
                OrderItem(4),
                OrderItem(5)
            };

            return orderItems;
        }

        public static OrderItem OrderItem(int id) => new OrderItem
        {
            OrderId = id,
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

        public static OrderDataDTO OrderDataDTO = new OrderDataDTO()
        {
            Amount = faker.Finance.Amount(),
            Customer = CustomerTestData.CustomerDataDTO,
            CustomerId = 1,
            OrderItems = OrderItemsDataDTO()
        };

        public static List<OrderItemDataDTO> OrderItemsDataDTO()
        {
            var orderItems = new List<OrderItemDataDTO>
            {
                OrderItemDataDTO,
                OrderItemDataDTO,
                OrderItemDataDTO,
                OrderItemDataDTO,
                OrderItemDataDTO
            };

            return orderItems;
        }

        public static OrderItemDataDTO OrderItemDataDTO = new OrderItemDataDTO()
        {
            Product = ProductTestData.ProductDataDTO
        };
    }

}
