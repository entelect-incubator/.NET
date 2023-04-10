namespace Common.Behaviour;

using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
	private readonly Stopwatch timer;
	private readonly ILogger logger;

	public PerformanceBehaviour(ILogger logger)
		=> (this.timer, this.logger) = (new Stopwatch(), logger);

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		this.timer.Restart();

		var response = await next();

		this.timer.Stop();

		var elapsedMilliseconds = this.timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 500)
		{
			var requestName = typeof(TRequest).Name;
			this.logger.LogInformation($"CleanArchitecture Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds)", request);
		}

		return response;
	}
}
