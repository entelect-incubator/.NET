<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 6 - Step 4** [![.NET - Phase 6 - Step 4](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step4.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step4.yml)

<br/><br/>

## **Orders**

One final step before building our customer website and portal is to establish the capability to track orders.

Add new Order Entity in Common Project

```cs
namespace Common.Entities;

public class Order
{
	public Order() => this.Pizzas = new HashSet<Pizza>();

	public int Id { get; set; }

	public required int CustomerId { get; set; }

	public virtual Customer Customer { get; set; }

	public DateTime? DateCreated { get; set; }

	public required bool Completed { get; set; }

	public List<int> PizzaIds { get; set; } // List of Pizza IDs

	public ICollection<Pizza> Pizzas { get; set; }
}
```

Add new Order Mapping in DataAccess Project

```cs
namespace DataAccess.Mapping;

public sealed class OrderMap : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.ToTable("Order", "dbo");

		builder.HasKey(t => t.Id);

		builder.Property(t => t.Id)
			.IsRequired()
			.HasColumnName("Id")
			.HasColumnType("int")
			.ValueGeneratedOnAdd();

		builder.Property(t => t.Completed)
			.HasColumnName("Completed")
			.HasColumnType("bool");

		builder.HasOne(o => o.Customer)
			.WithOne()
			.HasForeignKey<Order>(o => o.CustomerId);

		// Many-to-many relationship with Pizza
		builder.HasMany(o => o.Pizzas)
			.WithMany()
			.UsingEntity(j => j.ToTable("OrderPizzas"));

		builder.Property(t => t.DateCreated)
			.IsRequired()
			.HasColumnName("DateCreated")
			.HasColumnType("datetime")
			.HasDefaultValueSql("(getdate())");
	}
}
```

Modify OrderModel with the new Entity changes

```cs
namespace Common.Models.Order;

public class OrderModel
{
	public int Id { get; set; }

	public required int CustomerId { get; set; }

	public required CustomerModel Customer { get; set; }

	public List<int> PizzaIds { get; set; } // List of Pizza IDs

	public required List<PizzaModel> Pizzas { get; set; }

	public DateTime? DateCreated { get; set; }

	public required bool Completed { get; set; }
}
```

Create a Create Order Model that will be used in the new Order Controller later.

```cs
namespace Common.Models.Order;

public sealed class CreateOrderModel
{
	public required int CustomerId { get; set; }

	public required List<int> PizzaIds { get; set; }
}
```

Change Pizza Mapper to change IEnumerable to List

```cs
namespace Common.Mappers;

public static class PizzaMapper
{
	public static PizzaModel Map(this Pizza entity)
		=> new()
		{
			Id = entity.Id,
			Name = entity.Name,
			Description = entity.Description,
			Price = entity.Price,
			DateCreated = entity.DateCreated
		};

	public static Pizza Map(this PizzaModel model)
	{
		var entity = new Pizza
		{
			Id = model.Id,
			Name = model.Name,
			Description = model.Description,
			DateCreated = model.DateCreated
		};

		if (model.Price.HasValue)
		{
			entity.Price = model.Price.Value;
		}

		return entity;
	}

	public static List<PizzaModel> Map(this List<Pizza> entities)
		=> entities.Select(x => x.Map()).ToList();

	public static List<Pizza> Map(this List<PizzaModel> models)
		=> models.Select(x => x.Map()).ToList();
}
```

Create new Order Mappers from Model to Entity and back

```cs
namespace Common.Mappers;

using Common.Models.Order;

public static class OrderMapper
{
	public static OrderModel Map(this Order entity)
		=> new()
		{
			Id = entity.Id,
			Completed = entity.Completed,
			CustomerId = entity.CustomerId,
			Customer = entity.Customer.Map(),
			PizzaIds = entity.PizzaIds,
			Pizzas = entity.Pizzas.Map(),
			DateCreated = entity.DateCreated
		};

	public static Order Map(this OrderModel model)
		=> new()
		{
			Id = model.Id,
			Completed = model.Completed,
			CustomerId = model.CustomerId,
			Customer = model.Customer.Map(),
			PizzaIds = model.PizzaIds,
			Pizzas = model.Pizzas.Map(),
			DateCreated = model.DateCreated
		};

	public static Order Map(this CreateOrderModel model)
		=> new()
		{
			Completed = false,
			CustomerId = model.CustomerId,
			PizzaIds = model.PizzaIds,
			DateCreated = DateTime.UtcNow
		};

	public static IEnumerable<CustomerModel> Map(this List<Customer> entities)
		=> entities.Select(x => x.Map());

	public static IEnumerable<Customer> Map(this List<CustomerModel> models)
		=> models.Select(x => x.Map());
}
```

Add Order Entity and Mapping to DatabaseContext

```cs
namespace DataAccess;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	{
	}

	public DatabaseContext(DbContextOptions options) : base(options)
	{
	}

	public virtual DbSet<Customer> Customers { get; set; }

	public virtual DbSet<Pizza> Pizzas { get; set; }

	public virtual DbSet<Notify> Notifies { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CustomerMap());
		modelBuilder.ApplyConfiguration(new PizzaMap());
		modelBuilder.ApplyConfiguration(new NotifyMap());
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(databaseName: "PezzaDb");
}
```

Add Order filter in Common Project under Filters

```cs
namespace Common.Filters;

public static class OrderFilter
{
	public static IQueryable<Order> FilterByCustomerId(this IQueryable<Order> query, int? customerID)
	{
		if (!customerID.HasValue)
		{
			return query;
		}

		return query.Where(x => x.CustomerId == customerID.Value);
	}
}
```

Add a new folder under Core Project for Order Queries then create a new Query GetOrdersQuery.cs

```cs
namespace Core.Order.Queries;

using Common.Models.Order;

public class GetOrdersQuery : IRequest<ListResult<OrderModel>>
{
	public int CustomerID { get; set; }
}

public class GetOrdersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetOrdersQuery, ListResult<OrderModel>>
{
	public async Task<ListResult<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var entities = databaseContext.Orders
			.Select(x => x)
			.AsNoTracking()
			.FilterByCustomerId(request.CustomerID)
			.OrderBy("DateCreated desc");

		var count = entities.Count();
		var paged = await entities.ToListAsync(cancellationToken);

		return ListResult<OrderModel>.Success(paged.Map(), count);
	}
}
```

Add Orders to Order Event

```cs
namespace Core.Order.Events;

using System.Text;
using Common.Entities;
using Common.Models.Order;
using DataAccess;

public class OrderEvent : INotification
{
	public OrderModel Data { get; set; }
}

public class OrderEventHandler(DatabaseContext databaseContext) : INotificationHandler<OrderEvent>
{
	async Task INotificationHandler<OrderEvent>.Handle(OrderEvent notification, CancellationToken cancellationToken)
	{
		var path = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\Templates\\OrderCompleted.html";
		var html = File.ReadAllText(path);

		html = html.Replace("%name%", Convert.ToString(notification.Data.Customer.Name));

		var pizzasContent = new StringBuilder();
		foreach (var pizza in notification.Data.Pizzas)
		{
			pizzasContent.AppendLine($"<strong>{pizza.Name}</strong> - {pizza.Description}<br/>");
		}

		html = html.Replace("%pizzas%", pizzasContent.ToString());

		databaseContext.Orders.Add(notification.Data.Map());

		databaseContext.Notifies.Add(new Notify
		{
			CustomerId = notification.Data.Customer.Id,
			CustomerEmail = notification.Data?.Customer?.Email,
			DateSent = null,
			EmailContent = html,
			Sent = false
		});
		await databaseContext.SaveChangesAsync(cancellationToken);
	}
}
```

Add Order Controller

```cs
namespace Api.Controllers;

using Common.Models.Order;
using Core.Order.Commands;

[ApiController]
[Route("[controller]")]
public class OrderController() : ApiController
{
	/// <summary>
	/// Order Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST /Order
	///     {
	///       "customerId": 1,
	///       "pizzaIds": [1, 2, 3, 4, 5]
	///     }
	/// </remarks>
	/// <param name="model">Create Order Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<OrderModel>> Create([FromBody] OrderModel model)
	{
		var result = await this.Mediator.Send(new OrderCommand
		{
			Data = model
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}
}
```

Add Customer Orders to Customer Controller

```cs
/// <summary>
/// Get Customer Orders by Id.
/// </summary>
/// <param name="id">int.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
/// <response code="200">Get customer orders</response>
/// <response code="400">Error getting customer orders</response>
/// <response code="404">Customer orders not found</response>
[HttpGet("{id}/Orders")]
[ProducesResponseType(typeof(ListResult<OrderModel>), 200)]
[ProducesResponseType(typeof(ErrorResult), 400)]
[ProducesResponseType(typeof(ErrorResult), 404)]
public async Task<ActionResult> GetOrders(int id)
{
	var result = await this.Mediator.Send(new GetOrdersQuery { CustomerId = id });
	return ResponseHelper.ResponseOutcome(result, this);
}
```

Add Seed Data for Testing, add has data to DatabaseContext OnModelCreating

```cs
modelBuilder.Entity<Pizza>()
		.HasData(
		new Pizza { Id = 1, Name = "Pepperoni Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow },
		new Pizza { Id = 2, Name = "Meat Pizza", Price = 99, Description = string.Empty, DateCreated = DateTime.UtcNow },
		new Pizza { Id = 3, Name = "Margherita Pizza", Price = 79, Description = string.Empty, DateCreated = DateTime.UtcNow },
		new Pizza { Id = 4, Name = "Hawaiian Pizza", Price = 89, Description = string.Empty, DateCreated = DateTime.UtcNow });
```

To trigger this in Startup.cs in API Project add the end of ConfigureServices

```cs
using (var serviceProvider = services.BuildServiceProvider())
{
	var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();
	dbContext.Database.EnsureCreated();
	dbContext.SaveChanges();
	dbContext.Dispose();
}
```

## **Phase 7 - Create UI's**

Move to Phase 7
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%207)