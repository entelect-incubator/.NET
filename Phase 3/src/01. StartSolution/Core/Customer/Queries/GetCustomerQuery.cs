namespace Core.Customer.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCustomerQuery : IRequest<Result<CustomerModel>>
{
	public int Id { get; set; }

	public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerModel>>
	{
		private readonly DatabaseContext databaseContext;

		public GetCustomerQueryHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result<CustomerModel>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
		{
			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
			return Result<CustomerModel>.Success((await query(this.databaseContext, request.Id)).Map());
		}
	}
}