namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateCustomerCommand : IRequest<Result<CustomerDTO>>
    {
        public int Id { get; set; }

        public CustomerDataDTO Data { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDTO>>
    {
        private readonly IDataAccess<Common.Entities.Customer> DataAccess;

        public UpdateCustomerCommandHandler(IDataAccess<Common.Entities.Customer> DataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAccess.GetAsync(request.Id);
            findEntity.Name = !string.IsNullOrEmpty(request.Data?.Name) ? request.Data?.Name : findEntity.Name;
            findEntity.Address = !string.IsNullOrEmpty(request.Data?.Address?.Address) ? request.Data?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(request.Data?.Address?.City) ? request.Data?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(request.Data?.Address?.Province) ? request.Data?.Address?.Province : findEntity.Province;
            findEntity.PostalCode = !string.IsNullOrEmpty(request.Data?.Address?.PostalCode) ? request.Data?.Address?.PostalCode : findEntity.PostalCode;
            findEntity.Phone = !string.IsNullOrEmpty(request.Data?.Phone) ? request.Data?.Phone : findEntity.Phone;
            findEntity.ContactPerson = !string.IsNullOrEmpty(request.Data?.ContactPerson) ? request.Data?.ContactPerson : findEntity.ContactPerson;
            var outcome = await this.dataAccess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<CustomerDTO>.Success(outcome.Map()) : Result<CustomerDTO>.Failure("Error updating a Customer");
        }
    }
}