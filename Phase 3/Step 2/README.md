<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Step 2** [![.NET 7 - Phase 3 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml)

<br/><br/>

## **Search Models**

Let's extend our data DTO's to cater for filtering and pagination. In Common\Models create PagingArgs.cs

![PagingArgs](Assets/2021-01-15-07-06-25.png)


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

using System.Linq;
using Common.Models;

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

Add paging option to all the DTO's

```cs
public string OrderBy { get; set; }

public PagingArgs PagingArgs { get; set; }
```

Extend the DTO's in Common\DTO by inheriting from EntityBase. In the cases of RestaurantDTO and ProductDTO  that already inherit from ImageDataBase, let ImageDataBase derive from EnityBase.

![](2021-01-15-07-08-27.png)

```cs
namespace Common.DTO;

using Common.Models;
using Common.Models.Base;

public class CustomerDTO : EntityBase
{
    public CustomerDTO()
    {
        this.Address = new AddressBase();
        this.DateCreated = DateTime.Now;
    }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string ContactPerson { get; set; }

    public AddressBase Address { get; set; }

    public DateTime? DateCreated { get; set; }

    public PagingArgs MyProperty { get; set; }

    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
}
```

ImageDataBase

```cs
namespace Common.Entities;

using Common.DTO.Data;

public class ImageDataBase : EntityBase
{
    public string ImageData { get; set; }
}
```

Product DTO

```cs
namespace Common.DTO;

using Common.Entities;

public class ProductDTO : ImageDataBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string PictureUrl { get; set; }

    public decimal? Price { get; set; }

    public bool? Special { get; set; }

    public DateTime? OfferEndDate { get; set; }

    public decimal? OfferPrice { get; set; }

    public bool? IsActive { get; set; }

    public DateTime DateCreated { get; set; }
}
```

![](Assets\2021-01-18-09-58-18.png)

## **Add filtering**

Filter classes are created for every entity. These filters will make use of fluent design for readability. In each filter, you create a rule for every property that you want to filter on. If that property has a value, it builds up a query before executing it to the database. See it as building up a SQL WHERE clause.

Create a folder called Filters in Common and add CustomerFilter.cs.

```cs
namespace Common.Filters;

using System.Linq;
using Common.Entities;

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

    public static IQueryable<Customer> FilterByCity(this IQueryable<Customer> query, string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return query;
        }

        return query.Where(x => x.City.Contains(city));
    }

    public static IQueryable<Customer> FilterByProvince(this IQueryable<Customer> query, string province)
    {
        if (string.IsNullOrWhiteSpace(province))
        {
            return query;
        }

        return query.Where(x => x.Province.Contains(province));
    }

    public static IQueryable<Customer> FilterByPostalCode(this IQueryable<Customer> query, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            return query;
        }

        return query.Where(x => x.PostalCode.Contains(postalCode));
    }

    public static IQueryable<Customer> FilterByPhone(this IQueryable<Customer> query, string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return query;
        }

        return query.Where(x => x.Phone.Contains(phone));
    }

    public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return query;
        }

        return query.Where(x => x.Email.Contains(email));
    }

    public static IQueryable<Customer> FilterByContactPerson(this IQueryable<Customer> query, string contactPerson)
    {
        if (string.IsNullOrWhiteSpace(contactPerson))
        {
            return query;
        }

        return query.Where(x => x.ContactPerson.Contains(contactPerson));
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

You can copy the other Filters from **Phase 3\src\03. Step2\Common\Filters**

![Filters](Assets/2021-01-15-07-13-03.png)

### **Modifying Core**

The GetAllAsync method of IDataAccess gets enhanced in two ways. Firstly, a generic type parameter of type that is intended for passing through DTOs. Secondly, enhance the response by using ListResult<T> as the type argument of the Task return type.

![](Assets/2021-01-15-07-27-11.png)

```cs
Task<ListResult<T>> GetAllAsync(T dto);
```

The implementations of all Queries methods need to be modified to include filtering. Remember to install System.Linq.Dynamic.Core on the Core Project.

Modify the GetAllAsync method in GetCustomersQuery.cs as follows.

Add DTO Data object in the request

```cs
public class GetCustomersQuery : IRequest<ListResult<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}
```

Together

```cs
namespace Core.Customer.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Filters;
using Common.Models;
using DataAccess;

public class GetCustomersQuery : IRequest<ListResult<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetCustomersQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        if (string.IsNullOrEmpty(dto.OrderBy))
        {
            dto.OrderBy = "DateCreated desc";
        }

        var entities = this.databaseContext.Customers.Select(x => x)
            .AsNoTracking()
            .FilterByName(dto.Name)
            .FilterByAddress(dto.Address?.Address)
            .FilterByCity(dto.Address?.City)
            .FilterByProvince(dto.Address?.Province)
            .FilterByPostalCode(dto.Address?.PostalCode)
            .FilterByPhone(dto.Phone)
            .FilterByEmail(dto.Email)
            .FilterByContactPerson(dto.ContactPerson)
            .OrderBy(dto.OrderBy);

        var count = entities.Count();
        var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<CustomerDTO>.Success(paged, count);
    }
}
```

The only exception will be on the GetRestaurantsQuery implementation for Restaurant. We don't want to add any filtering, because of caching we will include later on.

Modify GetRestaurantsQuery.cs as follows.

```cs
namespace Core.Restaurant.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using DataAccess;

public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
{
}

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var entities = this.databaseContext.Restaurants.Select(x => x).AsNoTracking();

        var count = entities.Count();
        var paged = this.mapper.Map<List<RestaurantDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<RestaurantDTO>.Success(paged, count);
    }
}
```

Add filters to the other Core Queries implementations as well or copy it from **Phase 3\src\03. Step2\Core**

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
/// <response code="400">Error searching for customers</response>
[HttpPost]
[ProducesResponseType(typeof(ListResult<CustomerDTO>), 200)]
[ProducesResponseType(typeof(ErrorResult), 400)]
[Route("Search")]
public async Task<ActionResult> Search(CustomerDTO dto)
{
    var result = await this.Mediator.Send(new GetCustomersQuery
    {
        Data = dto
    });
    return ResponseHelper.ResponseOutcome(result, this);
}
```

### **Modify Unit Tests**

Modify unit tests to incorporate our changes.

## **Move to Phase 4**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%204)