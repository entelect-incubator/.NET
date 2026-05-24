namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Entities;
using Core.Helpers;
using DataAccess;

public sealed class CreateCustomerCommand : ICommand<Result<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}

public sealed class CreateCustomerCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateCustomerCommand, Result<CustomerDTO>>
{
    public async Task<Result<CustomerDTO>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer
        {
            Address = $"{request.Data.Address.Address}, {request.Data.Address.City}, {request.Data.Address.Province}, {request.Data.Address.PostalCode}",
            Name = request.Data.Name,
            Phone = request.Data.Phone,
            City = request.Data.Address.City,
            PostalCode = request.Data.Address.PostalCode,
            Province = request.Data.Address.Province,
            ContactPerson = request.Data.ContactPerson,
            Email = request.Data.Email,
            DateCreated = DateTime.UtcNow,
        };
        databaseContext.Customers.Add(entity);
        var outcome = await CoreHelper<CustomerDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a customer");
        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
        }

        return outcome;
    }
}
