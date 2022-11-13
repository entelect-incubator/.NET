namespace Pezza.Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.DTO;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class UpdateCustomerCommand : IRequest<Result<CustomerDTO>>
{
    public CustomerDTO Data { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public UpdateCustomerCommandHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;
        var findEntity = await this.databaseContext.Customers.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
        if (findEntity == null)
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
        var outcome = this.databaseContext.Customers.Update(findEntity);

        return await CoreHelper<CustomerDTO>.Outcome(this.databaseContext, this.mapper, cancellationToken, findEntity, "Error updating customer");
    }
}