namespace LiteBus.Queries.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;

	public static class QueryMediatorExtensions
	{
		public static async Task<TResponse> SendAsync<TResponse>(
			this IQueryMediator mediator,
			IQuery<TResponse> query,
			CancellationToken cancellationToken = default)
		{
			return await mediator.QueryAsync(query, cancellationToken);
		}
	}
}