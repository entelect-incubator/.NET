namespace LiteBus.Queries.Abstractions
{
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for IQueryMediator to support SendAsync pattern for consistency with MediatR
	/// </summary>
	public static class QueryMediatorExtensions
	{
		/// <summary>
		/// Send a query and get a result (wrapper around QueryAsync for API consistency)
		/// </summary>
		public static async Task<TResponse> SendAsync<TResponse>(
			this IQueryMediator mediator,
			IQuery<TResponse> query,
			CancellationToken cancellationToken = default)
		{
			// Wrapper that calls the LiteBus QueryAsync method
			return await mediator.QueryAsync(query, cancellationToken);
		}
	}
}
