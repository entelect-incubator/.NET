namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateCustomerCommand : ICommand<Result<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}

public sealed class UpdateCustomerCommandHandler(DatabaseContext databaseContext) : ICommandHandler<UpdateCustomerCommand, Result<CustomerDTO>>
{
    public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity is null)
        {
            return null;
        }

        findEntity.Name = !string.IsNullOrEmpty(dto?.Name) ? dto?.Name : findEntity.Name;
        findEntity.Address = !string.IsNullOrEmpty(dto?.Address?.Address) ? dto?.Address?.Address : findEntity.Address;
        findEntity.City = !string.IsNullOrEmpty(dto?.Address?.City) ? dto?.Address?.City : findEntity.City;
        findEntity.Province = !string.IsNullOrEmpty(dto?.Address?.Province) ? dto?.Address?.Province : findEntity.Province;
        findEntity.PostalCode = !string.IsNullOrEmpty(dto?.Address?.PostalCode) ? dto?.Address?.PostalCode : findEntity.PostalCode;
        findEntity.Phone = !string.IsNullOrEmpty(dto?.Phone) ? dto?.Phone : findEntity.Phone;
        findEntity.ContactPerson = !string.IsNullOrEmpty(dto?.ContactPerson) ? dto?.ContactPerson : findEntity.ContactPerson;
        databaseContext.Customers.Update(findEntity);

        return await CoreHelper<CustomerDTO>.Outcome(databaseContext, cancellationToken, findEntity.ToDto(), "Error updating customer");
    }
}