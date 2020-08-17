namespace Pezza.Core.Customer.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
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