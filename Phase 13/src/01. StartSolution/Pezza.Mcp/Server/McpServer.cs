namespace Pezza.Mcp.Server;

using Protocol;
using Tools;

/// <summary>
/// Main MCP server that routes tool calls from LLM clients.
/// </summary>
public class McpServer
{
    private readonly IPizzaToolHandler _pizzaHandler;
    private readonly IOrderToolHandler _orderHandler;
    private readonly IStockToolHandler _stockHandler;
    private readonly ILogger<McpServer> _logger;

    public McpServer(
        IPizzaToolHandler pizzaHandler,
        IOrderToolHandler orderHandler,
        IStockToolHandler stockHandler,
        ILogger<McpServer> logger)
    {
        _pizzaHandler = pizzaHandler;
        _orderHandler = orderHandler;
        _stockHandler = stockHandler;
        _logger = logger;
    }

    /// <summary>
    /// Get available tools for the LLM client.
    /// </summary>
    public List<McpTool> GetAvailableTools()
    {
        return new List<McpTool>
        {
            // Pizza tools
            new McpTool
            {
                Name = "get_menu",
                Description = "Get the complete pizza menu with prices and descriptions",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {},
                  "required": []
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "search_pizzas",
                Description = "Search for pizzas by name or description",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {
                    "query": {
                      "type": "string",
                      "description": "Search term (e.g., 'veggie', 'meat')"
                    }
                  },
                  "required": ["query"]
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "get_specials",
                Description = "Get current special offers and discounts",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {},
                  "required": []
                }
                """).RootElement
            },
            
            // Order tools
            new McpTool
            {
                Name = "get_order_status",
                Description = "Check the status of an order",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {
                    "order_id": {
                      "type": "string",
                      "description": "The order ID (UUID format)"
                    }
                  },
                  "required": ["order_id"]
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "get_customer_orders",
                Description = "Get all orders for a specific customer",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {
                    "customer_id": {
                      "type": "string",
                      "description": "The customer ID (UUID format)"
                    }
                  },
                  "required": ["customer_id"]
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "create_order",
                Description = "Create a new pizza order",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {
                    "customer_id": {
                      "type": "string",
                      "description": "The customer ID (UUID format)"
                    },
                    "items": {
                      "type": "array",
                      "description": "List of pizzas to order",
                      "items": {
                        "type": "object",
                        "properties": {
                          "pizza_id": {
                            "type": "string",
                            "description": "Pizza ID (UUID)"
                          },
                          "quantity": {
                            "type": "integer",
                            "description": "Number of pizzas"
                          }
                        },
                        "required": ["pizza_id", "quantity"]
                      }
                    }
                  },
                  "required": ["customer_id", "items"]
                }
                """).RootElement
            },
            
            // Stock tools
            new McpTool
            {
                Name = "get_stock_levels",
                Description = "View current inventory levels for all pizzas",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {},
                  "required": []
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "get_low_stock_alerts",
                Description = "Get list of items with low stock that need reordering",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {},
                  "required": []
                }
                """).RootElement
            },
            new McpTool
            {
                Name = "get_inventory_summary",
                Description = "Get overall inventory statistics",
                InputSchema = JsonDocument.Parse("""
                {
                  "type": "object",
                  "properties": {},
                  "required": []
                }
                """).RootElement
            }
        };
    }

    /// <summary>
    /// Handle incoming MCP request and route to appropriate tool.
    /// </summary>
    public async Task<string> HandleRequestAsync(McpRequest request)
    {
        _logger.LogInformation("Handling MCP request: {Method}", request.Method);

        try
        {
            var result = request.Method switch
            {
                // Pizza menu tools
                "get_menu" => await _pizzaHandler.GetMenuAsync(),
                "search_pizzas" => await HandleSearchPizzas(request.Parameters),
                "get_specials" => await _pizzaHandler.GetSpecialsAsync(),

                // Order tools
                "get_order_status" => await HandleGetOrderStatus(request.Parameters),
                "get_customer_orders" => await HandleGetCustomerOrders(request.Parameters),
                "create_order" => await HandleCreateOrder(request.Parameters),

                // Stock tools
                "get_stock_levels" => await _stockHandler.GetStockLevelsAsync(),
                "get_low_stock_alerts" => await _stockHandler.GetLowStockAlertsAsync(),
                "get_inventory_summary" => await _stockHandler.GetInventorySummaryAsync(),

                _ => CreateErrorResponse(request.Id, -32601, $"Unknown method: {request.Method}")
            };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling MCP request: {Method}", request.Method);
            return CreateErrorResponse(request.Id, -32603, $"Internal error: {ex.Message}");
        }
    }

    private async Task<string> HandleSearchPizzas(JsonElement? parameters)
    {
        if (parameters is null)
            return CreateErrorResponse(null, -32602, "Missing parameters");

        var query = parameters.Value.GetProperty("query").GetString();
        if (string.IsNullOrWhiteSpace(query))
            return CreateErrorResponse(null, -32602, "query parameter is required");

        return await _pizzaHandler.SearchPizzasAsync(query);
    }

    private async Task<string> HandleGetOrderStatus(JsonElement? parameters)
    {
        if (parameters is null)
            return CreateErrorResponse(null, -32602, "Missing parameters");

        var orderId = parameters.Value.GetProperty("order_id").GetString();
        if (string.IsNullOrWhiteSpace(orderId))
            return CreateErrorResponse(null, -32602, "order_id parameter is required");

        return await _orderHandler.GetOrderStatusAsync(orderId);
    }

    private async Task<string> HandleGetCustomerOrders(JsonElement? parameters)
    {
        if (parameters is null)
            return CreateErrorResponse(null, -32602, "Missing parameters");

        var customerId = parameters.Value.GetProperty("customer_id").GetString();
        if (string.IsNullOrWhiteSpace(customerId))
            return CreateErrorResponse(null, -32602, "customer_id parameter is required");

        return await _orderHandler.GetCustomerOrdersAsync(customerId);
    }

    private async Task<string> HandleCreateOrder(JsonElement? parameters)
    {
        if (parameters is null)
            return CreateErrorResponse(null, -32602, "Missing parameters");

        var customerId = parameters.Value.GetProperty("customer_id").GetString();
        if (string.IsNullOrWhiteSpace(customerId))
            return CreateErrorResponse(null, -32602, "customer_id parameter is required");

        var itemsElement = parameters.Value.GetProperty("items");
        var items = new List<(string PizzaId, int Quantity)>();

        foreach (var item in itemsElement.EnumerateArray())
        {
            var pizzaId = item.GetProperty("pizza_id").GetString();
            var quantity = item.GetProperty("quantity").GetInt32();

            if (!string.IsNullOrWhiteSpace(pizzaId))
            {
                items.Add((pizzaId, quantity));
            }
        }

        if (items.Count == 0)
            return CreateErrorResponse(null, -32602, "At least one item is required");

        return await _orderHandler.CreateOrderAsync(customerId, items);
    }

    private static string CreateErrorResponse(string? id, int code, string message)
    {
        var error = new McpResponse
        {
            Id = id,
            Error = new McpError { Code = code, Message = message }
        };

        return JsonSerializer.Serialize(error, new JsonSerializerOptions { WriteIndented = true });
    }
}
