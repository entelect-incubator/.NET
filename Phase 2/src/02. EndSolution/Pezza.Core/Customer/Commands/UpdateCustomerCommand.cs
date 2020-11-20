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
        private readonly IDataAccess<Common.Entities.Customer> dataAcess;

        public UpdateCustomerCommandHandler(IDataAccess<Common.Entities.Customer> dataAcess)
            => this.dataAcess = dataAcess;

        public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);

            if (!string.IsNullOrEmpty(request.Data?.Name))
            {
                findEntity.Name = request.Data?.Name;
            }

            if (!string.IsNullOrEmpty(request.Data?.AddressBase?.Address))
            {
                findEntity.Address = request.Data?.AddressBase?.Address;
            }

            if (!string.IsNullOrEmpty(request.Data?.AddressBase?.City))
            {
                findEntity.City = request.Data?.AddressBase?.City;
            }

            if (!string.IsNullOrEmpty(request.Data?.AddressBase?.Province))
            {
                findEntity.Province = request.Data?.AddressBase?.Province;
            }

            if (!string.IsNullOrEmpty(request.Data?.AddressBase?.ZipCode))
            {
                findEntity.ZipCode = request.Data?.AddressBase?.ZipCode;
            }

            if (!string.IsNullOrEmpty(request.Data?.Phone))
            {
                findEntity.Phone = request.Data?.Phone;
            }

            if (!string.IsNullOrEmpty(request.Data?.ContactPerson))
            {
                findEntity.ContactPerson = request.Data?.ContactPerson;
            }

            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<CustomerDTO>.Success(outcome.Map()) : Result<CustomerDTO>.Failure("Error updating a Customer");
        }
    }
}