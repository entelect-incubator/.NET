namespace Pezza.Mcp.Tools;

using System.Text.Json;
using Common.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class ToolHandlers(DatabaseContext databaseContext)
{
    public async Task<object> GetMenuAsync()
    {
        var pizzas = await databaseContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                description = x.Description,
                price = x.Price,
                hasOffer = x.Special,
                offerPrice = x.OfferPrice ?? x.Price
            })
            .ToListAsync();

        return new
        {
            success = true,
            message = $"Found {pizzas.Count} pizzas on the menu",
            data = pizzas
        };
    }

    public async Task<object> SearchPizzasAsync(string query)
    {
        var normalized = query.Trim();

        var pizzas = await databaseContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Where(x => x.Name.Contains(normalized) || x.Description.Contains(normalized))
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                price = x.Price,
                description = x.Description
            })
            .ToListAsync();

        return new
        {
            success = true,
            query = normalized,
            resultCount = pizzas.Count,
            data = pizzas
        };
    }

    public async Task<object> GetSpecialsAsync()
    {
        var now = DateTime.UtcNow;

        var specials = await databaseContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Where(x => x.Special && (!x.OfferEndDate.HasValue || x.OfferEndDate.Value >= now))
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                regularPrice = x.Price,
                offerPrice = x.OfferPrice ?? x.Price,
                savings = x.Price - (x.OfferPrice ?? x.Price),
                validUntil = x.OfferEndDate
            })
            .ToListAsync();

        return new
        {
            success = true,
            activeSpecials = specials.Count,
            data = specials
        };
    }

    public async Task<object> CreateOrderAsync(int customerId, JsonElement items)
    {
        if (items.ValueKind != JsonValueKind.Array || items.GetArrayLength() == 0)
        {
            throw new InvalidOperationException("items parameter is required and must be a non-empty array");
        }

        var customerExists = await databaseContext.Customers.AnyAsync(x => x.Id == customerId);
        if (!customerExists)
        {
            throw new InvalidOperationException($"Customer '{customerId}' was not found");
        }

        var requestedItems = new List<(int productId, int quantity)>();

        foreach (var item in items.EnumerateArray())
        {
            var productId = ReadInt(item, "pizza_id");
            var quantity = ReadInt(item, "quantity");

            if (quantity <= 0)
            {
                throw new InvalidOperationException("quantity must be greater than zero");
            }

            requestedItems.Add((productId, quantity));
        }

        var productIds = requestedItems.Select(x => x.productId).Distinct().ToList();
        var products = await databaseContext.Products
            .Where(x => productIds.Contains(x.Id) && x.IsActive)
            .ToDictionaryAsync(x => x.Id);

        if (products.Count != productIds.Count)
        {
            throw new InvalidOperationException("One or more pizza_id values are invalid");
        }

        var order = new Order
        {
            CustomerId = customerId,
            RestaurantId = 1,
            DateCreated = DateTime.UtcNow,
            Completed = false,
            Amount = 0m
        };

        databaseContext.Orders.Add(order);
        await databaseContext.SaveChangesAsync();

        decimal total = 0m;
        foreach (var item in requestedItems)
        {
            var product = products[item.productId];
            var unitPrice = product.Special ? (product.OfferPrice ?? product.Price) : product.Price;
            total += unitPrice * item.quantity;

            databaseContext.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = item.quantity
            });
        }

        order.Amount = total;
        await databaseContext.SaveChangesAsync();

        return new
        {
            success = true,
            message = "Order created successfully",
            data = new
            {
                id = order.Id,
                orderNumber = $"ORD-{order.DateCreated:yyyyMMddHHmmss}-{order.Id}",
                orderDate = order.DateCreated,
                total,
                itemsCount = requestedItems.Count
            }
        };
    }

    public async Task<object> GetOrderStatusAsync(int orderId)
    {
        var order = await databaseContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == orderId)
            ?? throw new InvalidOperationException($"Order '{orderId}' was not found");

        return new
        {
            success = true,
            data = new
            {
                id = order.Id,
                orderNumber = $"ORD-{order.DateCreated:yyyyMMddHHmmss}-{order.Id}",
                orderDate = order.DateCreated,
                completed = order.Completed,
                total = order.Amount,
                items = order.OrderItems.Count,
                status = order.Completed ? "Completed" : "In Progress"
            }
        };
    }

    public async Task<object> GetCustomerOrdersAsync(int customerId)
    {
        var orders = await databaseContext.Orders
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new
            {
                id = x.Id,
                orderNumber = $"ORD-{x.DateCreated:yyyyMMddHHmmss}-{x.Id}",
                orderDate = x.DateCreated,
                completed = x.Completed,
                total = x.Amount,
                status = x.Completed ? "Completed" : "In Progress"
            })
            .ToListAsync();

        return new
        {
            success = true,
            customerOrders = orders.Count,
            data = orders
        };
    }

    public async Task<object> GetStockLevelsAsync()
    {
        var stocks = await databaseContext.Stocks
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                name = x.Name,
                quantity = x.Quantity,
                reorderLevel = 10,
                status = x.Quantity <= 10 ? "Low Stock" : "In Stock"
            })
            .ToListAsync();

        return new
        {
            success = true,
            totalItems = stocks.Count,
            data = stocks
        };
    }

    public async Task<object> GetLowStockAlertsAsync()
    {
        var alerts = await databaseContext.Stocks
            .AsNoTracking()
            .Where(x => x.Quantity <= 10)
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                name = x.Name,
                quantity = x.Quantity,
                reorderLevel = 10,
                unitsToOrder = x.Quantity < 20 ? 20 - x.Quantity : 0
            })
            .ToListAsync();

        return new
        {
            success = true,
            alertCount = alerts.Count,
            data = alerts
        };
    }

    public async Task<object> GetInventorySummaryAsync()
    {
        var stocks = await databaseContext.Stocks
            .AsNoTracking()
            .ToListAsync();

        var totalItems = stocks.Count;
        var totalUnits = stocks.Sum(x => x.Quantity);
        var lowStockCount = stocks.Count(x => x.Quantity <= 10);
        var averageQuantity = totalItems == 0 ? 0 : Math.Round((double)totalUnits / totalItems, 2);

        return new
        {
            success = true,
            data = new
            {
                totalItems,
                totalUnits,
                lowStockCount,
                averageQuantity
            }
        };
    }

    private static int ReadInt(JsonElement source, string propertyName)
    {
        if (!source.TryGetProperty(propertyName, out var value))
        {
            throw new InvalidOperationException($"{propertyName} parameter is required");
        }

        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var numberValue))
        {
            return numberValue;
        }

        if (value.ValueKind == JsonValueKind.String && int.TryParse(value.GetString(), out var stringValue))
        {
            return stringValue;
        }

        throw new InvalidOperationException($"{propertyName} must be an integer value");
    }
}
