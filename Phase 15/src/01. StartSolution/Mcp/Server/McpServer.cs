namespace Pezza.Mcp.Server;

using Pezza.Mcp.Protocol;
using Pezza.Mcp.Tools;

public sealed class McpServer(ToolHandlers toolHandlers)
{
    public async Task<McpResponse> HandleAsync(McpRequest request)
    {
        if (!string.Equals(request.JsonRpc, "2.0", StringComparison.Ordinal))
        {
            return Error(request, -32600, "Invalid Request");
        }

        try
        {
            var result = await (request.Method switch
            {
                "get_menu" => toolHandlers.GetMenuAsync(),
                "search_pizzas" => toolHandlers.SearchPizzasAsync(ReadString(request, "query")),
                "get_specials" => toolHandlers.GetSpecialsAsync(),
                "create_order" => toolHandlers.CreateOrderAsync(ReadInt(request, "customer_id"), ReadElement(request, "items")),
                "get_order_status" => toolHandlers.GetOrderStatusAsync(ReadInt(request, "order_id")),
                "get_customer_orders" => toolHandlers.GetCustomerOrdersAsync(ReadInt(request, "customer_id")),
                "get_stock_levels" => toolHandlers.GetStockLevelsAsync(),
                "get_low_stock_alerts" => toolHandlers.GetLowStockAlertsAsync(),
                "get_inventory_summary" => toolHandlers.GetInventorySummaryAsync(),
                _ => throw new InvalidOperationException($"Unknown method: {request.Method}")
            });

            return new McpResponse
            {
                Id = request.Id,
                Result = result
            };
        }
        catch (InvalidOperationException ex)
        {
            var code = ex.Message.StartsWith("Unknown method", StringComparison.Ordinal) ? -32601 : -32602;
            return Error(request, code, ex.Message);
        }
        catch (Exception ex)
        {
            return Error(request, -32603, $"Internal error: {ex.Message}");
        }
    }

    private static McpResponse Error(McpRequest request, int code, string message)
    {
        return new McpResponse
        {
            Id = request.Id,
            Error = new McpError
            {
                Code = code,
                Message = message
            }
        };
    }

    private static string ReadString(McpRequest request, string propertyName)
    {
        var element = ReadElement(request, propertyName);
        if (element.ValueKind != System.Text.Json.JsonValueKind.String)
        {
            throw new InvalidOperationException($"{propertyName} parameter is required");
        }

        return element.GetString() ?? string.Empty;
    }

    private static int ReadInt(McpRequest request, string propertyName)
    {
        var element = ReadElement(request, propertyName);

        if (element.ValueKind == System.Text.Json.JsonValueKind.Number && element.TryGetInt32(out var numberValue))
        {
            return numberValue;
        }

        if (element.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(element.GetString(), out var stringValue))
        {
            return stringValue;
        }

        throw new InvalidOperationException($"{propertyName} parameter is required");
    }

    private static System.Text.Json.JsonElement ReadElement(McpRequest request, string propertyName)
    {
        if (request.Parameters.ValueKind != System.Text.Json.JsonValueKind.Object ||
            !request.Parameters.TryGetProperty(propertyName, out var element))
        {
            throw new InvalidOperationException($"{propertyName} parameter is required");
        }

        return element;
    }
}
