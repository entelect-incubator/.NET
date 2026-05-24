# Phase 13: MCP Server - AI Chat Integration

This phase implements a **Model Context Protocol (MCP)** server that enables AI assistants (Claude, ChatGPT, etc.) to interact with the Pezza pizza ordering system through natural language.

> ⚠️ **CODE QUALITY NOTE**: StartSolution uses legacy constructor patterns for learning purposes; **FinalSolution implements C# 12+ primary constructors** as per COPILOT-INSTRUCTIONS. See the conversions in `Core` handlers (e.g., `GetNotifiesQueryHandler`, `GetPizzasQueryHandler`) for the correct pattern:
> ```csharp
> public sealed class GetPizzasQueryHandler(
>     DatabaseContext databaseContext,
>     IMapper mapper) : IQueryHandler<GetPizzasQuery, Result<IEnumerable<PizzaDTO>>>
> ```
> Prefer FinalSolution as your reference template.

## What is MCP?

**Model Context Protocol** is a standard for exposing tools and resources to Large Language Models (LLMs). It allows AI to:

- 🤖 Call defined functions/tools
- 📊 Access structured data
- 💬 Have context-aware conversations
- ⚙️ Perform actions on your behalf

### MCP Architecture

```mermaid
┌────────────────┐
│ LLM Client     │
│ (Claude, GPT)  │
└────────┬───────┘
         │ JSON-RPC
         │
┌────────▼──────────────────────┐
│   Mcp Server (STDIO)    │
│                               │
│  ┌──────────────────────────┐ │
│  │  Pizza Tools             │ │
│  │  • get_menu()            │ │
│  │  • search_pizzas()       │ │
│  │  • get_specials()        │ │
│  └──────────────────────────┘ │
│                               │
│  ┌──────────────────────────┐ │
│  │  Order Tools             │ │
│  │  • create_order()        │ │
│  │  • get_order_status()    │ │
│  │  • get_customer_orders() │ │
│  └──────────────────────────┘ │
│                               │
│  ┌──────────────────────────┐ │
│  │  Stock Tools             │ │
│  │  • get_stock_levels()    │ │
│  │  • get_low_stock_alerts()│ │
│  │  • get_inventory_summary()│ │
│  └──────────────────────────┘ │
│                               │
└────────────────────────────────┘
         │ JSON-RPC
         │
┌────────▼──────────────────┐
│  Pezza Backend            │
│  • Database               │
│  • Dispatcher / LiteBus   │
│  • Business Logic         │
└───────────────────────────┘
```

## Available Tools for LLM Clients

### Pizza Menu Tools

#### `get_menu` - View Complete Menu

**Description**: Get all available pizzas with prices and descriptions

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "1",
  "method": "get_menu",
  "params": {}
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "1",
  "result": {
    "success": true,
    "message": "Found 8 pizzas on the menu",
    "data": [
      {
        "id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
        "name": "Margherita",
        "description": "Classic tomato, mozzarella, and basil",
        "price": 9.99,
        "hasOffer": false,
        "offerPrice": 9.99
      },
      {
        "id": "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy",
        "name": "Pepperoni",
        "description": "Tomato, mozzarella, and pepperoni",
        "price": 11.99,
        "hasOffer": true,
        "offerPrice": 10.99
      }
    ]
  }
}
```

#### `search_pizzas` - Search Menu

**Description**: Search for pizzas by name or description keyword

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "2",
  "method": "search_pizzas",
  "params": {
    "query": "veggie"
  }
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "2",
  "result": {
    "success": true,
    "query": "veggie",
    "resultCount": 1,
    "data": [
      {
        "id": "zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz",
        "name": "Vegetarian",
        "price": 10.99,
        "description": "Fresh vegetables and cheese"
      }
    ]
  }
}
```

#### `get_specials` - View Current Offers

**Description**: Get active special offers and discounts

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "3",
  "method": "get_specials",
  "params": {}
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "3",
  "result": {
    "success": true,
    "activeSpecials": 2,
    "data": [
      {
        "id": "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy",
        "name": "Pepperoni",
        "regularPrice": 11.99,
        "offerPrice": 10.99,
        "savings": 1.00,
        "validUntil": "2025-12-31T23:59:59.000Z"
      }
    ]
  }
}
```

### Order Management Tools

#### `create_order` - Place New Order

**Description**: Create a new pizza order for a customer

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "4",
  "method": "create_order",
  "params": {
    "customer_id": "12345678-1234-1234-1234-123456789012",
    "items": [
      {
        "pizza_id": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
        "quantity": 2
      },
      {
        "pizza_id": "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy",
        "quantity": 1
      }
    ]
  }
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "4",
  "result": {
    "success": true,
    "message": "Order created successfully",
    "data": {
      "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
      "orderNumber": "ORD-20251030142530",
      "orderDate": "2025-10-30T14:25:30.000Z",
      "total": 32.97,
      "itemsCount": 2
    }
  }
}
```

---

Teaching Thread

- From: Phase 12 migrated to the custom dispatcher.
- This phase: build an MCP server and integrate AI-assisted workflows and tools.
- Next: Phase 14 focuses on external API integration.

Libraries (why they matter)

- MCP (Model Context Protocol) tooling: enables structured AI assistant interactions — document how to run and secure an MCP server.
- AI tooling integration notes: list third-party clients and their educational purpose.

Clean Code & SOLID (teaching notes)

- Treat AI-generated changes as suggestions: require tests and linter passes before accepting.
- Provide clear instructions on how to validate AI-assisted edits and how to revert them.

Dispatcher policy

- AI integration is independent of dispatcher choice — ensure the dispatcher pattern used by the codebase is reflected in AI examples and prompts.

Notes

- Add example prompts and expected outputs in `AI_PROMPTING_EXAMPLES.md` and link them from this phase README.

#### `get_order_status` - Check Order

**Description**: Get current status of a specific order

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "5",
  "method": "get_order_status",
  "params": {
    "order_id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
  }
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "5",
  "result": {
    "success": true,
    "data": {
      "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
      "orderNumber": "ORD-20251030142530",
      "orderDate": "2025-10-30T14:25:30.000Z",
      "completed": null,
      "total": 32.97,
      "items": 2,
      "status": "In Progress"
    }
  }
}
```

#### `get_customer_orders` - View Customer History

**Description**: Get all orders placed by a customer

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "6",
  "method": "get_customer_orders",
  "params": {
    "customer_id": "12345678-1234-1234-1234-123456789012"
  }
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "6",
  "result": {
    "success": true,
    "customerOrders": 5,
    "data": [
      {
        "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
        "orderNumber": "ORD-20251030142530",
        "orderDate": "2025-10-30T14:25:30.000Z",
        "completed": null,
        "total": 32.97,
        "status": "In Progress"
      }
    ]
  }
}
```

### Stock Management Tools

#### `get_stock_levels` - Admin: View Inventory

**Description**: Get current quantity of all pizzas in stock

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "7",
  "method": "get_stock_levels",
  "params": {}
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "7",
  "result": {
    "success": true,
    "totalItems": 8,
    "data": [
      {
        "name": "Margherita",
        "quantity": 45,
        "reorderLevel": 10,
        "status": "In Stock"
      },
      {
        "name": "Pepperoni",
        "quantity": 8,
        "reorderLevel": 10,
        "status": "Low Stock"
      }
    ]
  }
}
```

#### `get_low_stock_alerts` - Admin: Reorder Alert

**Description**: Get items that need reordering

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "8",
  "method": "get_low_stock_alerts",
  "params": {}
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "8",
  "result": {
    "success": true,
    "alertCount": 2,
    "data": [
      {
        "name": "Pepperoni",
        "quantity": 8,
        "reorderLevel": 10,
        "unitsToOrder": 12
      }
    ]
  }
}
```

#### `get_inventory_summary` - Admin: Inventory Stats

**Description**: Get overall inventory statistics

**Request**:

```json
{
  "jsonrpc": "2.0",
  "id": "9",
  "method": "get_inventory_summary",
  "params": {}
}
```

**Response**:

```json
{
  "jsonrpc": "2.0",
  "id": "9",
  "result": {
    "success": true,
    "data": {
      "totalItems": 8,
      "totalUnits": 410,
      "lowStockCount": 2,
      "averageQuantity": 51.25
    }
  }
}
```

## Running the MCP Server

### Prerequisites

- .NET 10.0 SDK or later
- SQL Server (running with Phase 11 migrations applied)
- An LLM that supports MCP (Claude, etc.)

### Start the Server

```powershell
# StartSolution path
cd "Phase 13/src/01. StartSolution"
dotnet run --project Mcp/Mcp.csproj

# FinalSolution path
cd "Phase 13/src/02. FinalSolution"
dotnet run --project Mcp/Mcp.csproj
```

### Server Output

```text
[10:30:15 INF] Starting Pezza MCP Server...
[10:30:16 INF] Available MCP Tools:
[10:30:16 INF]   - get_menu: Get the complete pizza menu with prices and descriptions
[10:30:16 INF]   - search_pizzas: Search for pizzas by name or description
[10:30:16 INF]   - get_specials: Get current special offers and discounts
[10:30:16 INF]   - get_order_status: Check the status of an order
[10:30:16 INF]   - get_customer_orders: Get all orders for a specific customer
[10:30:16 INF]   - create_order: Create a new pizza order
[10:30:16 INF]   - get_stock_levels: View current inventory levels for all pizzas
[10:30:16 INF]   - get_low_stock_alerts: Get list of items with low stock that need reordering
[10:30:16 INF]   - get_inventory_summary: Get overall inventory statistics
[10:30:16 INF] MCP Server ready. Listening for requests on STDIO...
```

## Integration with Claude

### Claude Desktop Configuration

Edit `%APPDATA%\Claude\claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "pezza": {
      "command": "dotnet",
      "args": ["run", "--project", "C:\\Users\\YourName\\Dev\\Incubator\\.NET\\Phase 13\\src\\02. FinalSolution\\Mcp\\Mcp.csproj"]
    }
  }
}
```

### Example Claude Conversations

**User**: "What pizzas do you have on special?"

```text
Claude will use: get_specials() → Returns active offers
Response: "We have 2 specials running today:
- Pepperoni: $10.99 (save $1.00)
- Hawaiian: $11.99 (save $1.00)"
```

**User**: "I'd like to order 2 Margheritas and 1 Pepperoni for customer 12345678-1234-1234-1234-123456789012"

```text
Claude will use: 
1. get_menu() → Find pizza IDs
2. create_order() → Place the order
Response: "Order created! Order #ORD-20251030142530 
Total: $32.97 (2x Margherita @ $9.99 + 1x Pepperoni @ $10.99)"
```

**Admin User**: "What inventory needs reordering?"

```text
Claude will use: get_low_stock_alerts() → Check inventory
Response: "2 items need reordering:
- Pepperoni: Currently 8 units (reorder level: 10), suggest ordering 12 units"
```

## Error Handling

### Invalid Parameters

```json
{
  "jsonrpc": "2.0",
  "id": "10",
  "error": {
    "code": -32602,
    "message": "query parameter is required"
  }
}
```

### Unknown Method

```json
{
  "jsonrpc": "2.0",
  "id": "11",
  "error": {
    "code": -32601,
    "message": "Unknown method: invalid_method"
  }
}
```

### Internal Server Error

```json
{
  "jsonrpc": "2.0",
  "id": "12",
  "error": {
    "code": -32603,
    "message": "Internal error: Database connection failed"
  }
}
```

## MCP Protocol Details

### JSON-RPC 2.0 Specification

All requests follow JSON-RPC 2.0:

```json
{
  "jsonrpc": "2.0",
  "id": "<unique-request-id>",
  "method": "<tool-name>",
  "params": { /* tool-specific parameters */ }
}
```

### Error Codes

| Code   | Meaning          |
| ------ | ---------------- |
| -32700 | Parse error      |
| -32600 | Invalid Request  |
| -32601 | Method not found |
| -32602 | Invalid params   |
| -32603 | Internal error   |

## Implementation Architecture

### Class Structure

```text
Mcp/
├── Program.cs                 (Entry point, STDIO loop)
├── appsettings.json          (Connection strings, settings)
├── Protocol/
│   └── McpTypes.cs           (Request/Response DTOs)
├── Server/
│   └── McpServer.cs          (Route requests to handlers)
├── Tools/
│   └── ToolHandlers.cs       (Pizza, Order, Stock implementations)
└── Mcp.csproj          (Dependencies: EF Core, Serilog)
```

### Data Flow

```text
STDIO Input
   ↓
JsonSerializer.Deserialize<McpRequest>()
   ↓
McpServer.HandleRequestAsync()
   ↓
Route to appropriate handler:
   ├─ IPizzaToolHandler
   ├─ IOrderToolHandler
   └─ IStockToolHandler
   ↓
Query DatabaseContext via Entity Framework
   ↓
Format response with JsonSerializer
   ↓
Console.WriteLine() → STDIO Output
```

## Security Considerations

### Authentication & Authorization

Current implementation:

- ⚠️ No authentication required (localhost only recommended)
- ⚠️ No authorization checks (all tools available to all clients)

**Production Recommendations**:

- Add API key validation
- Implement role-based access (customers vs. admin)
- Use HTTPS with mutual TLS authentication
- Rate limiting per client
- Request signing with HMAC

### Data Validation

All tools validate:

- ✅ Required parameters present
- ✅ UUID format for IDs
- ✅ Positive quantities for orders
- ✅ Database entity existence

### Logging

All requests logged with:

- Request timestamp
- Method name
- Parameters (redacted for sensitive data)
- Response status
- Execution time
- Error details

## Performance Considerations

### Query Optimization

Tools use efficient EF Core queries:

- ✅ `.Select()` projections (avoid loading unnecessary columns)
- ✅ `.Include()` for related entities
- ✅ `.OrderBy()` for consistent sorting
- `.FirstOrDefaultAsync()` for single records

### Connection Pooling

SQL Server connection pooling active:

- Min: 10 connections
- Max: 100 connections
- Timeout: 15 seconds

### Response Times

Typical tool response times:

- Menu: 50-100ms
- Search: 30-80ms
- Create Order: 100-200ms
- Stock Levels: 80-150ms

## Troubleshooting

### Server Won't Start

```text
Error: Unable to connect to the database
```

**Solution**:

1. Check SQL Server is running
2. Verify connection string in appsettings.json
3. Ensure migrations have run: `dotnet run --project DbUp.Migrations`

### No Response from Tools

```text
Error: STDIO timeout
```

**Solution**:

1. Check server logs for errors
2. Verify JSON is valid
3. Ensure unique request IDs
4. Check database permissions

### Slow Responses

```text
Tool takes > 1 second to respond
```

**Solution**:

1. Check database query plans
2. Add indexes on frequently searched columns
3. Monitor SQL Server CPU/memory
4. Consider caching popular results

## Advanced: Adding Custom Tools

### Step 1: Create Handler Interface

```csharp
public interface ICustomToolHandler
{
    Task<string> GetCustomDataAsync(string parameter);
}
```

### Step 2: Implement Handler

```csharp
public class CustomToolHandler : ICustomToolHandler
{
    private readonly DatabaseContext _dbContext;
    
    public async Task<string> GetCustomDataAsync(string parameter)
    {
        var data = await _dbContext.CustomTable
            .Where(x => x.Property == parameter)
            .ToListAsync();
            
        return JsonSerializer.Serialize(new { success = true, data });
    }
}
```

### Step 3: Register in DI

```csharp
services.AddScoped<ICustomToolHandler, CustomToolHandler>()
```

### Step 4: Add to McpServer Tools List

```csharp
public List<McpTool> GetAvailableTools()
{
    return new List<McpTool>
    {
        // ... existing tools
        new McpTool
        {
            Name = "custom_tool",
            Description = "Description of custom tool",
            InputSchema = // JSON schema
        }
    };
}
```

### Step 5: Add Handler in switch

```csharp
"custom_tool" => await HandleCustomTool(request.Parameters)
```

## Directory Structure

```md
Phase 13/
├── src/
│   ├── 01. StartSolution/
│   │   ├── Mcp/
│   │   │   ├── Mcp.csproj
│   │   │   ├── Program.cs (STDIO MCP server)
│   │   │   ├── GlobalUsings.cs
│   │   │   ├── appsettings.json
│   │   │   ├── Protocol/
│   │   │   │   └── McpTypes.cs (Request/Response DTOs)
│   │   │   ├── Server/
│   │   │   │   └── McpServer.cs (Request routing)
│   │   │   └── Tools/
│   │   │       └── ToolHandlers.cs (Pizza/Order/Stock operations)
│   │   ├── Api/
│   │   ├── AspireHost/
│   │   ├── Common/
│   │   ├── Core/
│   │   ├── DataAccess/
│   │   ├── DbUp.Migrations/
│   │   ├── docker-compose.yml
│   │   └── Pezza.slnx
│   ├── 02. FinalSolution/
│   │   ├── Mcp/
│   │   │   ├── Mcp.csproj
│   │   │   ├── Program.cs (STDIO MCP server)
│   │   │   ├── appsettings.json
│   │   │   ├── Protocol/
│   │   │   │   └── McpTypes.cs (Request/Response DTOs)
│   │   │   ├── Server/
│   │   │   │   └── McpServer.cs (Request routing)
│   │   │   └── Tools/
│   │   │       └── ToolHandlers.cs (Pizza/Order/Stock operations)
│   └── ... (other phases)
└── README.md
```

## Next Steps

- Integrate with Claude Desktop
- Add WebSocket support for HTTP clients
- Implement tool caching for frequently called operations
- Add OAuth2 authentication
- Create browser UI for MCP server
- Monitor tool usage and analytics

## References

- [Model Context Protocol Spec](https://spec.modelcontextprotocol.io/)
- [Claude API Documentation](https://claude.ai/docs)
- [JSON-RPC 2.0 Spec](https://www.jsonrpc.org/specification)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [LiteBus Documentation](https://github.com/rafaelfeitosa/LiteBus)

## Learning Outcomes

After completing Phase 13, you'll understand:

1. ✅ How LLMs interact with external tools via MCP
2. ✅ Building STDIO-based protocol handlers
3. ✅ Error handling for AI-driven applications
4. ✅ Structuring domain logic for AI consumption
5. ✅ Security considerations for AI-powered systems
6. ✅ JSON-RPC 2.0 protocol implementation
7. ✅ Integration testing with LLM clients


[Move to Phase 14](https://github.com/entelect-incubator/.NET/tree/master/Phase%2014)
