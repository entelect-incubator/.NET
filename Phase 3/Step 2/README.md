<img align="left" width="116" height="116" src="../logo.png" />

# &nbsp;**E List - Phase 3 - Step 2** [![.NET - Phase 3 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml)

<br/><br/>

## **Search Models**

Let's extend our Models to cater for filtering and pagination. In Common\Models create PagingArgs.cs

```cs
namespace Common.Models;

public class PagingArgs
{
    private int limit = 20;

    public static PagingArgs NoPaging => new PagingArgs { UsePaging = false };

    public static PagingArgs Default => new PagingArgs { UsePaging = true, Limit = 20, Offset = 0 };

    public static PagingArgs FirstItem => new PagingArgs { UsePaging = true, Limit = 1, Offset = 0 };

    public int Offset { get; set; }

    public int Limit
    {
        get => this.limit;

        set
        {
            if (value == 0)
            {
                value = 20;
            }

            this.limit = value;
        }
    }

    public bool UsePaging { get; set; }
}
```

Add an extension method in Common to do the Pagination. Create a new folder called Extensions in Common and add Extensions.cs

```cs
namespace Common.Extensions;

public static class Extensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingArgs pagingArgs)
    {
        var myPagingArgs = pagingArgs;

        if (pagingArgs == null)
        {
            myPagingArgs = PagingArgs.Default;
        }

        return myPagingArgs.UsePaging ? query.Skip(myPagingArgs.Offset).Take(myPagingArgs.Limit) : query;
    }
}
```

Add paging option to all the Search Models

```cs
public string? OrderBy { get; set; }

public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;
```

## **Add filtering**

Filter classes are created for every entity. These filters will make use of fluent design for readability. In each filter, you create a rule for every property that you want to filter on. If that property has a value, it builds up a query before executing it to the database. See it as building up a SQL WHERE clause.

Create a folder called Filters in DataAccess and add CustomerFilter.cs.

```cs
namespace Common.Filters;

public static class CustomerFilter
{
	public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return query;
		}

		return query.Where(x => x.Name.Contains(name));
	}

	public static IQueryable<Customer> FilterByAddress(this IQueryable<Customer> query, string address)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			return query;
		}

		return query.Where(x => x.Address.Contains(address));
	}

	public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string cellphone)
	{
		if (string.IsNullOrWhiteSpace(cellphone))
		{
			return query;
		}

		return query.Where(x => x.Cellphone.Contains(cellphone));
	}

	public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			return query;
		}

		return query.Where(x => x.Email.Contains(email));
	}

	public static IQueryable<Customer> FilterByDateCreated(this IQueryable<Customer> query, DateTime? dateCreated)
	{
		if (!dateCreated.HasValue)
		{
			return query;
		}

		return query.Where(x => x.DateCreated == dateCreated.Value);
	}
}
```

Add another filter for Pizza called PizzaFilter.cs

### **Modifying Core**

Modify the get all query for Customer and Pizza in the Core Project.

The implementations of all Queries methods need to be modified to include filtering. Remember to install System.Linq.Dynamic.Core Nuget Package on the Core Project.

Modify GlobalUsings.cs

```cs
global using System.Linq.Dynamic.Core;
global using Common.Extensions;
global using Common.Filters;
global using Common.Mappers;
global using Common.Models;
global using Core.Pizza.Commands;
global using DataAccess;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
```

```cs
namespace Core.Customer.Queries;

public class GetCustomersQuery : IRequest<ListResult<CustomerModel>>
{
	public SearchCustomerModel Data { get; set; }

	public class GetCustomersQueryHandler(DatabaseContext databaseContext) : IRequestHandler<GetCustomersQuery, ListResult<CustomerModel>>
	{
		public async Task<ListResult<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
		{
			var entity = request.Data;
			if (string.IsNullOrEmpty(entity.OrderBy))
			{
				entity.OrderBy = "DateCreated desc";
			}
			var entities = databaseContext.Customers
				.Select(x => x)
				.AsNoTracking()
				.FilterByName(entity.Name)
				.FilterByAddress(entity.Address)
				.FilterByPhone(entity.Cellphone)
				.FilterByEmail(entity.Email)
				.OrderBy(entity.OrderBy);

			var count = entities.Count();
			var paged = await entities.ApplyPaging(entity.PagingArgs).ToListAsync(cancellationToken);

			return ListResult<CustomerModel>.Success(paged.Map(), count);
		}
	}
}
```

Make sure GetPizzasQuery has also been modified.

### **Modifying Controllers**

Modify all the Search Action Methods in the controllers.

For example, modify CustomerController.cs as follows.

```cs
/// <summary>
	/// Get all Customers.
	/// </summary>
	/// <returns>A <see cref="Task"/> repres
	/// enting the asynchronous operation.</returns>
	/// <response code="200">Customer Search</response>
	/// <response code="400">Error searching for clients</response>
	[HttpPost]
	[ProducesResponseType(typeof(ListResult<CustomerModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[Route("Search")]
	public async Task<ActionResult> Search(SearchCustomerModel data)
	{
		var result = await this.Mediator.Send(new GetCustomersQuery()
		{
			Data = data
		});
		return ResponseHelper.ResponseOutcome(result, this);
	}
```

Make sure to modify PizzaController as well

### **Modify Unit Tests**

Modify unit tests to incorporate our changes.

## **Move to Phase 4**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%204)
