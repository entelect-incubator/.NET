namespace Core.Customer.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using DataAccess;

public class DeleteCustomerCommand : IRequest<Result>
{
	public int? Id { get; set; }

	public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
	{
		private readonly DatabaseContext databaseContext;

		public DeleteCustomerCommandHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
		{
			if (request.Id == null)
			{
				return Result.Failure("Error deleting a Customer");
			}

			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Customers.FirstOrDefault(c => c.Id == id));
			var findEntity = await query(this.databaseContext, request.Id.Value);
			if (findEntity == null)
			{
				return Result.Failure("Customer not found");
			}

			this.databaseContext.Customers.Remove(findEntity);
			var result = await this.databaseContext.SaveChangesAsync();

			return result > 0 ? Result.Success() : Result.Failure("Error deleting a Customer");
		}
	}
}