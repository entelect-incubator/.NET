namespace Pezza.Core.Customer.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.DataAccess.Contracts;

    public partial class CreateCustomerCommand : IRequest<Customer>
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

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerDataAccess dataAcess;

        public CreateCustomerCommandHandler(ICustomerDataAccess dataAcess) 
            => this.dataAcess = dataAcess;

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            => await this.dataAcess.SaveAsync(new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                ZipCode = request.ZipCode,
                ContactPerson = request.ContactPerson,
                DateCreated = DateTime.Now
            });
    }
}
