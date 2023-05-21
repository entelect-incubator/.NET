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

			var result = await this.databaseContext.Customers
				.Where(u => u.Id == request.Id)
				.ExecuteDeleteAsync();

			return result > 0 ? Result.Success() : Result.Failure("Error deleting a Customer");

		}
	}
}