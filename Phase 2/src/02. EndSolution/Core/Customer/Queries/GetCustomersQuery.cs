namespace Core.Customer.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCustomersQuery : IRequest<ListResult<CustomerModel>>
{
	public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ListResult<CustomerModel>>
	{
		private readonly DatabaseContext databaseContext;

		public GetCustomersQueryHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<ListResult<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
		{
			var entities = this.databaseContext.Customers.Select(x => x).AsNoTracking();

			var count = entities.Count();
			var paged = await entities.ToListAsync(cancellationToken);

			return ListResult<CustomerModel>.Success(paged.Map(), count);
		}
	}
}