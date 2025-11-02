namespace Pezza.Mcp.Tools;

using DataAccess;

/// <summary>
/// Pizza catalog operations exposed to LLM clients.
/// </summary>
public interface IPizzaToolHandler
{
    Task<string> GetMenuAsync();
    Task<string> GetPizzaByIdAsync(string pizzaId);
    Task<string> SearchPizzasAsync(string query);
    Task<string> GetSpecialsAsync();
}

/// <summary>
/// Implementation of pizza catalog operations.
/// </summary>
public class PizzaToolHandler : IPizzaToolHandler
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<PizzaToolHandler> _logger;

    public PizzaToolHandler(DatabaseContext dbContext, ILogger<PizzaToolHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> GetMenuAsync()
    {
        _logger.LogInformation("Fetching pizza menu");
        
        var pizzas = await _dbContext.Pizzas
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                HasOffer = p.Offer.HasValue,
                OfferPrice = p.Offer ?? p.Price
            })
            .OrderBy(p => p.Name)
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            message = $"Found {pizzas.Count} pizzas on the menu",
            data = pizzas
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> GetPizzaByIdAsync(string pizzaId)
    {
        _logger.LogInformation("Fetching pizza {PizzaId}", pizzaId);
        
        if (!Guid.TryParse(pizzaId, out var id))
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = "Invalid pizza ID format"
            });
        }

        var pizza = await _dbContext.Pizzas.FindAsync(id);
        
        if (pizza is null)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = $"Pizza {pizzaId} not found"
            });
        }

        return JsonSerializer.Serialize(new
        {
            success = true,
            data = pizza
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> SearchPizzasAsync(string query)
    {
        _logger.LogInformation("Searching pizzas for: {Query}", query);
        
        var results = await _dbContext.Pizzas
            .Where(p => p.Name.Contains(query) || p.Description!.Contains(query))
            .Select(p => new { p.Id, p.Name, p.Price, p.Description })
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            query = query,
            resultCount = results.Count,
            data = results
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> GetSpecialsAsync()
    {
        _logger.LogInformation("Fetching pizza specials");
        
        var specials = await _dbContext.Pizzas
            .Where(p => p.Offer.HasValue && p.OfferStarts <= DateTime.UtcNow && p.OfferEnds >= DateTime.UtcNow)
            .Select(p => new
            {
                p.Id,
                p.Name,
                RegularPrice = p.Price,
                OfferPrice = p.Offer,
                Savings = p.Price - p.Offer,
                ValidUntil = p.OfferEnds
            })
            .OrderBy(p => p.OfferPrice)
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            activeSpecials = specials.Count,
            data = specials
        }, new JsonSerializerOptions { WriteIndented = true });
    }
}

/// <summary>
/// Order management operations exposed to LLM clients.
/// </summary>
public interface IOrderToolHandler
{
    Task<string> GetOrderStatusAsync(string orderId);
    Task<string> GetCustomerOrdersAsync(string customerId);
    Task<string> CreateOrderAsync(string customerId, List<(string PizzaId, int Quantity)> items);
}

/// <summary>
/// Implementation of order operations.
/// </summary>
public class OrderToolHandler : IOrderToolHandler
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<OrderToolHandler> _logger;

    public OrderToolHandler(DatabaseContext dbContext, ILogger<OrderToolHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> GetOrderStatusAsync(string orderId)
    {
        _logger.LogInformation("Fetching order status for {OrderId}", orderId);
        
        if (!Guid.TryParse(orderId, out var id))
        {
            return JsonSerializer.Serialize(new { success = false, error = "Invalid order ID" });
        }

        var order = await _dbContext.Orders
            .Include(o => o.OrderItems!)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
        {
            return JsonSerializer.Serialize(new { success = false, error = $"Order {orderId} not found" });
        }

        return JsonSerializer.Serialize(new
        {
            success = true,
            data = new
            {
                order.Id,
                order.OrderNumber,
                order.OrderDate,
                order.Completed,
                order.Total,
                Items = order.OrderItems?.Count ?? 0,
                Status = order.Completed.HasValue ? "Completed" : "In Progress"
            }
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> GetCustomerOrdersAsync(string customerId)
    {
        _logger.LogInformation("Fetching orders for customer {CustomerId}", customerId);
        
        if (!Guid.TryParse(customerId, out var id))
        {
            return JsonSerializer.Serialize(new { success = false, error = "Invalid customer ID" });
        }

        var orders = await _dbContext.Orders
            .Where(o => o.CustomerId == id)
            .Select(o => new
            {
                o.Id,
                o.OrderNumber,
                o.OrderDate,
                o.Completed,
                o.Total,
                Status = o.Completed.HasValue ? "Completed" : "In Progress"
            })
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            customerOrders = orders.Count,
            data = orders
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> CreateOrderAsync(string customerId, List<(string PizzaId, int Quantity)> items)
    {
        _logger.LogInformation("Creating order for customer {CustomerId} with {ItemCount} items", customerId, items.Count);
        
        if (!Guid.TryParse(customerId, out var cId))
        {
            return JsonSerializer.Serialize(new { success = false, error = "Invalid customer ID" });
        }

        var customer = await _dbContext.Customers.FindAsync(cId);
        if (customer is null)
        {
            return JsonSerializer.Serialize(new { success = false, error = "Customer not found" });
        }

        try
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = cId,
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}",
                OrderDate = DateTime.UtcNow,
                Total = 0
            };

            decimal total = 0;
            foreach (var (pizzaId, quantity) in items)
            {
                if (!Guid.TryParse(pizzaId, out var pId))
                    continue;

                var pizza = await _dbContext.Pizzas.FindAsync(pId);
                if (pizza is null)
                    continue;

                var price = pizza.Offer ?? pizza.Price;
                var itemTotal = price * quantity;
                total += itemTotal;

                order.OrderItems ??= new List<OrderItem>();
                order.OrderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    PizzaId = pId,
                    Quantity = quantity,
                    UnitPrice = price,
                    Total = itemTotal
                });
            }

            order.Total = total;
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return JsonSerializer.Serialize(new
            {
                success = true,
                message = "Order created successfully",
                data = new
                {
                    order.Id,
                    order.OrderNumber,
                    order.OrderDate,
                    order.Total,
                    ItemsCount = items.Count
                }
            }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = $"Failed to create order: {ex.Message}"
            });
        }
    }
}

/// <summary>
/// Stock management and admin operations exposed to LLM clients.
/// </summary>
public interface IStockToolHandler
{
    Task<string> GetStockLevelsAsync();
    Task<string> GetLowStockAlertsAsync();
    Task<string> GetInventorySummaryAsync();
}

/// <summary>
/// Implementation of stock operations.
/// </summary>
public class StockToolHandler : IStockToolHandler
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<StockToolHandler> _logger;

    public StockToolHandler(DatabaseContext dbContext, ILogger<StockToolHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> GetStockLevelsAsync()
    {
        _logger.LogInformation("Fetching stock levels");
        
        var stock = await _dbContext.Stocks
            .Include(s => s.Pizza)
            .Select(s => new
            {
                s.Pizza!.Name,
                s.Quantity,
                s.ReorderLevel,
                Status = s.Quantity <= s.ReorderLevel ? "Low Stock" : "In Stock"
            })
            .OrderBy(s => s.Name)
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            totalItems = stock.Count,
            data = stock
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> GetLowStockAlertsAsync()
    {
        _logger.LogInformation("Checking for low stock items");
        
        var lowStock = await _dbContext.Stocks
            .Where(s => s.Quantity <= s.ReorderLevel)
            .Include(s => s.Pizza)
            .Select(s => new
            {
                s.Pizza!.Name,
                s.Quantity,
                s.ReorderLevel,
                UnitsToOrder = s.ReorderLevel * 2 - s.Quantity
            })
            .ToListAsync();

        return JsonSerializer.Serialize(new
        {
            success = true,
            alertCount = lowStock.Count,
            data = lowStock
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> GetInventorySummaryAsync()
    {
        _logger.LogInformation("Generating inventory summary");
        
        var summary = new
        {
            TotalItems = await _dbContext.Stocks.CountAsync(),
            TotalUnits = await _dbContext.Stocks.SumAsync(s => s.Quantity),
            LowStockCount = await _dbContext.Stocks.CountAsync(s => s.Quantity <= s.ReorderLevel),
            AverageQuantity = await _dbContext.Stocks.AverageAsync(s => (double)s.Quantity)
        };

        return JsonSerializer.Serialize(new
        {
            success = true,
            data = summary
        }, new JsonSerializerOptions { WriteIndented = true });
    }
}
