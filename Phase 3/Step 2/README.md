<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Step 2** [![.NET Core - Phase 3 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase3-step2.yml)

<br/><br/>

## **Search Models**

Extend our data DTO's to cater for filtering and pagination. In Pezza.Common Models create PagingArgs.cs

![PagingArgs](Assets/2021-01-15-07-06-25.png)

```cs
namespace Pezza.Common.Models
{
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
}
```

Add Extensions method in Pezza.Common to do the Pagination. Create a new Folder Extension in Pezza.Common with Extensions.cs

```cs
namespace Pezza.Common.Extensions
{
    using System.Linq;
    using Pezza.Common.Models;

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
}
```

Extend the DTO's in Pezza.Common\Models by inheriting from SearchBase. In the cases of RestaurantDTO and ProductDTO  that already inherit from ImageDataBase, let ImageDataBase derive from SearchBase.

![](2021-01-15-07-08-27.png)

Create a new SearchBase.cs in Pezza.Common\Models

```cs
namespace Pezza.Common.DTO.Data
{
    using Pezza.Common.Models;

    public class SearchBase
    {
        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
```

```cs
namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;

    public class CustomerDTO : SearchBase
    {
        public CustomerDTO()
        {
            this.Address = new AddressBase();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public AddressBase Address { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
```

ImageDataBase

```cs
namespace Pezza.Common.Entities
{
    using Pezza.Common.DTO.Data;

    public class ImageDataBase : SearchBase
    {
        public string ImageData { get; set; }
    }
}

```

Product DTO

```cs
namespace Pezza.Common.DTO
{
    using System;
    using Pezza.Common.Entities;

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
}

```

![](2021-01-18-09-58-18.png)

### **Add filtering**

Create a Filter class for every entity, these filters use fluent design for readability. In each filter, you create a rule for every property that you want to filter on. If that property has a value it builds up a query before executing it to the database. See it as building up a SQL WHERE clause.

Create a CustomerFilter.cs in Pezza.Common\Filter

```cs
namespace Pezza.Common.Filter
{
    using System;
    using System.Linq;
    using Pezza.Common.Entities;

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
}
```

You can also copy the other Filters from Step3\Data\Filter

![Filters](Assets/2021-01-15-07-13-03.png)

### **Add Filters to the DataAccess**

Change the IDataAccess interface by adding a parameter of type SearchBase to the GetAllAsync method.

![](Assets/2021-01-15-07-27-11.png)

```
Task<ListResult<T>> GetAllAsync(Entity searchBase);
```

## **Add Searching Capability**

Change all the DataAccess GetAllSync methods to include the new SearchModel and Filtering.

The only exception will be on Restaurant GetAllAsync(). We don't want to add any filtering, because of caching we want to include later on.

```cs
public async Task<ListResult<RestaurantDTO>> GetAllAsync(Entity searchBase)
{
    var entities = this.databaseContext.Restaurants.Select(x => x)
        .AsNoTracking();

    var count = entities.Count();
    var paged = await entities.ApplyPaging(dto.PagingArgs).ToListAsync();

    return ListResult<RestaurantDTO>.Success(this.mapper.Map<List<RestaurantDTO>>(paged), count);
}
``

Open CustomerDataAccess

```cs
public async Task<ListResult<CustomerDTO>> GetAllAsync(CustomerDTO dto)
{
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
    var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ApplyPaging(dto.PagingArgs).ToListAsync());
    return ListResult<CustomerDTO>.Success(paged, count);
}
```

Add all the Filters to the other DataAccess as well or can copy it from Phase 3/Data/DataAccess

### Modify all Queries

Include each entity DTO as a Search Model in all Queries that calls GetAllAsync DataAccess.

Example GetCustomersQuery.cs

```cs
namespace Pezza.Core.Customer.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetCustomersQuery : IRequest<ListResult<CustomerDTO>>
    {
        public CustomerDTO dto;
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerDTO>>
    {
        private readonly IDataAccess<CustomerDTO> DataAccess;

        public GetCustomersQueryHandler(IDataAccess<CustomerDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<ListResult<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAllAsync(request.dto);

            return search;
        }
    }
}
```

### **Modify Controllers**

Modify all the Controllers Search Functions

Example Customer Controller

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
[ProducesResponseType(typeof(Result), 400)]
[Route("Search")]
public async Task<ActionResult> Search(CustomerDTO dto)
{
    var result = await this.Mediator.Send(new GetCustomersQuery
    {
        dto = dto
    });
    return ResponseHelper.ResponseOutcome(result, this);
}
```

### **Modify Unit Test**

Add Search Model to all GetAllAsync Unit Tests

```cs
[Test]
public async Task GetAllAsync()
{
    var response = await this.handler.GetAllAsync(new CustomerDTO
    {
        Name = this.dto.Name
    });
    var outcome = response.Count;

    Assert.IsTrue(outcome == 1);
}
```

Test Customer Core

```cs
[Test]
public async Task GetAllAsync()
{
    var sutGetAll = new GetCustomersQueryHandler(this.dataAccess);
    var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery
    {
        dto = new CustomerDTO
        {
            Name = this.dto.Name
        }
    }, CancellationToken.None);

    Assert.IsTrue(resultGetAll?.Data.Count == 1);
}
```

## **Move to Phase 4**
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%204)