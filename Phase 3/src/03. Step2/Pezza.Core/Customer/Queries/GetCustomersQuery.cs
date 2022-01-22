namespace Pezza.Core.Customer.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using System.Linq.Dynamic.Core;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Filters;
    using Pezza.Common.Models;
    using Pezza.DataAccess;

    public class GetCustomersQuery : IRequest<ListResult<CustomerDTO>>
    {
        public CustomerDTO Data { get; set; }
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public GetCustomersQueryHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ListResult<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var dto = request.Data;

            if (string.IsNullOrEmpty(dto.OrderBy))
            {
                dto.OrderBy = "DateCreated desc";
            }

            var entities = this.databaseContext.Customers.Select(x => x)
                .AsNoTracking()
                .FilterByName(dto.Name)
                .FilterByAddress(dto.Address?.Address)
                .FilterByCity(dto.Address?.City)
                .FilterByProvince(dto.Address?.Province)
                .FilterByPostalCode(dto.Address?.PostalCode)
                .FilterByPhone(dto.Phone)
                .FilterByEmail(dto.Email)
                .FilterByContactPerson(dto.ContactPerson)
                .OrderBy(dto.OrderBy);

            var count = entities.Count();
            var paged = this.mapper.Map<List<CustomerDTO>>(await entities.ToListAsync(cancellationToken));

            return ListResult<CustomerDTO>.Success(paged, count);
        }
    }
}
