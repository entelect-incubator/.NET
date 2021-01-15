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

        public UpdateCustomerCommandHandler(IDataAccess<Common.Entities.Customer> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var findEntity = await this.dataAcess.GetAsync(request.Id);
            findEntity.Name = !string.IsNullOrEmpty(request.Data?.Name) ? request.Data?.Name : findEntity.Name;
            findEntity.Email = !string.IsNullOrEmpty(request.Data?.Email) ? request.Data?.Email : findEntity.Email;
            findEntity.Address = !string.IsNullOrEmpty(request.Data?.Address?.Address) ? request.Data?.Address?.Address : findEntity.Address;
            findEntity.City = !string.IsNullOrEmpty(request.Data?.Address?.City) ? request.Data?.Address?.City : findEntity.City;
            findEntity.Province = !string.IsNullOrEmpty(request.Data?.Address?.Province) ? request.Data?.Address?.Province : findEntity.Province;
            findEntity.ZipCode = !string.IsNullOrEmpty(request.Data?.Address?.ZipCode) ? request.Data?.Address?.ZipCode : findEntity.ZipCode;
            findEntity.Phone = !string.IsNullOrEmpty(request.Data?.Phone) ? request.Data?.Phone : findEntity.Phone;
            findEntity.ContactPerson = !string.IsNullOrEmpty(request.Data?.ContactPerson) ? request.Data?.ContactPerson : findEntity.ContactPerson;
            var outcome = await this.dataAcess.UpdateAsync(findEntity);

            return (outcome != null) ? Result<CustomerDTO>.Success(outcome.Map()) : Result<CustomerDTO>.Failure("Error updating a Customer");
        }
    }
}