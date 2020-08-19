<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 1 - Step 1**

<br/><br/>

This Phase might feel a bit tedious, but it puts down a strong foundation to build of from for the entire incubator.

### **Entitites**
![Entitites Setup](../Assets/phase1-setup-entities.png)

### **Mapping**
![Mapping Setup](../Assets/phase1-setup-mapping.png)

### **05 Database > Pezza.DataAccess.Contracts > IDatabaseContext.cs**
![Database Context Interface Setup](../Assets/phase1-setup-db-context-interface.png)


### Create Basic Filtering classes for every entity that will be used. Using customer filter as an example, add basic filtering to each filter where applicable i.e filterByName

05 Database > Pezza.DataAccess > Filter

The basic sample Filter class

```
namespace Pezza.DataAccess.Filter
{
    public static class NotifyFilter
    {
    }
}
```

Customer Filter class

```
namespace Pezza.DataAccess.Filter
{
    using System.Linq;
    using Pezza.Common.Entities;

    public static class CustomerFilter
    {
        public static IQueryable<Customer> FilterByName(this IQueryable<Customer> query, string name)
            => string.IsNullOrEmpty(name) ? query : query.Where(x => x.Name.ToLower().Contains(name.ToLower()));

        public static IQueryable<Customer> FilterByEmail(this IQueryable<Customer> query, string email)
            => string.IsNullOrEmpty(email) ? query : query.Where(x => x.Email.ToLower().Contains(email.ToLower()));
    }
}
```

It should look like this when you are done
![Data Access Filters](../Assets/phase1-setup-filters.png)

### Create a DataAccess class and interface for every Entity

Sample Interface

```
namespace Pezza.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pezza.Common.Entities;

    public interface ICustomerDataAccess
    {
        Task<Customer> GetAsync(int id);

        Task<List<Customer>> GetAllAsync(CustomerSearchModel searchModel);

        Task<Customer> UpdateAsync(Customer entity);

        Task<Customer> SaveAsync(Customer entity);

        Task<bool> DeleteAsync(Customer entity);
    }
}
```

Sample Data Access Class
```
namespace Pezza.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class NotifyDataAccess : INotifyDataAccess
    {
        private readonly IDatabaseContext databaseContext;

        public NotifyDataAccess(IDatabaseContext databaseContext) => this.databaseContext = databaseContext;

        public async Task<Common.Entities.Notify> GetAsync(int id)
        {
            return await this.databaseContext.Notifies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Common.Entities.Notify>> GetAllAsync(NotifySearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateSent desc";
            }

            var entities = this.databaseContext.Notifies.Select(x => x)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();
            return paged;
        }

        public async Task<Common.Entities.Notify> SaveAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Add(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Common.Entities.Notify> UpdateAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Update(entity);
            await this.databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Common.Entities.Notify entity)
        {
            this.databaseContext.Notifies.Remove(entity);
            var result = await this.databaseContext.SaveChangesAsync();
            return (result == 1);
        }
    }
}
```

Customer Data Access Class add the Filter Class function

```
public async Task<Common.Entities.Order> GetAsync(int id) => await this.databaseContext.Orders
            .Include(i => i.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == id); 

public async Task<List<Common.Entities.Customer>> GetAllAsync(CustomerSearchModel searchModel)
{
    if (string.IsNullOrEmpty(searchModel.OrderBy))
    {
        searchModel.OrderBy = "DateCreated desc";
    }

    var entities = this.databaseContext.Customers.Select(x => x)
        .FilterByName(searchModel.Name)
        .FilterByEmail(searchModel.Email)
        .AsNoTracking()
        .OrderBy(searchModel.OrderBy);

    var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();
    return paged;
}
```

For Foreign Keys dependencies remember to use .Include i.e. an Order should return the Order Items with as well. Rememebr you can only create a Order not removeUpdate and Delete should 

```
public async Task<List<Common.Entities.Order>> GetAllAsync(OrderSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateSent desc";
            }

            var entities = this.databaseContext.Orders.Select(x => x)
                .FilterByCustomerId(searchModel.CustomerId)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy)
                .Include(i => i.OrderItems);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();
            return paged;        
        }
```

### When you are done it should look like this

![Data Access Contracts](../Assets/phase1-setup-data-access-contracts.png)

![Data Access](../Assets/phase1-setup-data-access.png)

### **05 Database > Pezza.DataAccess > DatabaseContext.cs**
![Database Context Setup](../Assets/phase1-setup-db-context.png)

## Create Search Models for all entities

Search Models is DTO that contains properties to search for an entity by. We also use the PagingArgs class provided in the Clean Code Architecture to standarise and make it easy to apply pagination or sorting for all queries.

```
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

In 04 Common > Pezza.Common > Models > SearchModels

Create a **Search Model** for each Entity

Include all the properties that you might need to use to search for an entity with. All fields that is not a string should be made nullable.

Make sure all Search Models includes the following. We will be covering this in more detail in Phase 2

```
    public string OrderBy { get; set; }

    public PagingArgs PagingArgs { get; set; }
```

For the Filter Class you will need Nuget

```
Package System.Linq.Dynamic.Core
```

```
namespace Pezza.Common.Models
{
    using System;

    public class CustomerSearchModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public DateTime? DateCreated { get; set; }

        public string OrderBy { get; set; }

        public PagingArgs PagingArgs { get; set; }
    }
}
```

![Customer Search Model](../Assets/phase1-setup-customer-search-model.png)


### 03 Core > Pezza.Core > Create Command and Queries

Update DependencyInjection.cs to include data access layer
```
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

    services.AddTransient(typeof(IOrderDataAccess), typeof(OrderDataAccess));
    services.AddTransient(typeof(IStockDataAccess), typeof(StockDataAccess));
    services.AddTransient(typeof(INotifyDataAccess), typeof(NotifyDataAccess));
    services.AddTransient(typeof(IProductDataAccess), typeof(ProductDataAccess));
    services.AddTransient(typeof(ICustomerDataAccess), typeof(CustomerDataAccess));
    services.AddTransient(typeof(IRestaurantDataAccess), typeof(RestaurantDataAccess));

    return services;
}
```

### Create commands for every Entity

Sample Create Command - CreateCustomerCommand

```
namespace Pezza.Core.Customer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerDataAccess dataAcess;

        public CreateCustomerCommandHandler(ICustomerDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(request.Customer);
    }
}
```

Sample Delete Command - DeleteCustomerCommand

```
namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class DeleteCustomerCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerDataAccess dataAcess;

        public DeleteCustomerCommandHandler(ICustomerDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);
            var outcome = await this.dataAcess.DeleteAsync(findEntity);

            return (outcome) ? Result.Success() : Result.Failure("Error deleting a Customer");
        }
    }
}
```

Sample Update Command - UpdateCustomerCommand

When updating a customer we might not know what the customer id is. To solve this we are going to use the customer search model class in the common project.

Update Customer Data Access Interface and Customer Data Access

```
Task<List<Customer>> GetAllAsync(CustomerSearchModel searchModel);
```

```
public async Task<List<Common.Entities.Customer>> GetAllAsync(CustomerSearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.OrderBy))
            {
                searchModel.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Customers.Select(x => x)
                .FilterByName(searchModel.Name)
                .FilterByEmail(searchModel.Email)
                .AsNoTracking()
                .OrderBy(searchModel.OrderBy);

            var paged = await entities.ApplyPaging(searchModel.PagingArgs).ToListAsync();
            return paged;
        }
```

Update Command Sample

```
namespace Pezza.Core.Customer.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateCustomerCommand : IRequest<Result<Common.Entities.Customer>>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<Common.Entities.Customer>>
    {
        private readonly ICustomerDataAccess dataAcess;

        public UpdateCustomerCommandHandler(ICustomerDataAccess dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<Common.Entities.Customer>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(new CustomerSearchModel
            {
                Email = request.Email
            });
            var findEntity = search.FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Name))
            {
                findEntity.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                findEntity.Address = request.Address;
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                findEntity.City = request.City;
            }

            if (!string.IsNullOrEmpty(request.Province))
            {
                findEntity.Province = request.Province;
            }

            if (!string.IsNullOrEmpty(request.ZipCode))
            {
                findEntity.ZipCode = request.ZipCode;
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                findEntity.Phone = request.Phone;
            }

            if (!string.IsNullOrEmpty(request.ContactPerson))
            {
                findEntity.ContactPerson = request.ContactPerson;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<Common.Entities.Customer>.Success(outcome) : Result<Common.Entities.Customer>.Failure("Error updating a Customer");
        }
    }
}
``` 

### Query Sample

```
namespace Pezza.Core.Customer.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetCustomerQuery : IRequest<ListResult<Common.Entities.Customer>>
    {
        public CustomerSearchModel CustomerSearchModel { get; set; }
    }

    public class GetTodosQueryHandler : IRequestHandler<GetCustomerQuery, ListResult<Common.Entities.Customer>>
    {
        private readonly ICustomerDataAccess dataAcess;

        public GetTodosQueryHandler(ICustomerDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Customer>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.CustomerSearchModel);

            return ListResult<Common.Entities.Customer>.Success(search);
        }
    }
}
```