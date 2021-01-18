namespace Pezza.Core.Customer.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateCustomerCommand : IRequest<Result<CustomerDTO>>
    {
        public CustomerDTO Data { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDTO>>
    {
        private readonly IDataAccess<CustomerDTO> dataAcess;

        public UpdateCustomerCommandHandler(IDataAccess<CustomerDTO> dataAcess) => this.dataAcess = dataAcess ?? throw new System.ArgumentNullException(nameof(dataAcess));

        public async Task<Result<CustomerDTO>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {            
            var outcome = await this.dataAcess.UpdateAsync(request.Data);
            return (outcome != null) ? Result<CustomerDTO>.Success(outcome) : Result<CustomerDTO>.Failure("Error updating a Customer");
        }
    }
}