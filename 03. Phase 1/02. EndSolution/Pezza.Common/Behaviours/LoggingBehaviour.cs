namespace Pezza.Common.Behaviours
{
    using System.Threading;
    using System.Threading.Tasks;
    using Pezza.Common.Interfaces;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;

    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IIdentityService identityService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            this.logger = logger;
            this.identityService = identityService;
            this.currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = this.currentUserService.UserId ?? string.Empty;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await this.identityService.GetUserNameAsync(userId);
            }

            this.logger.LogInformation("Pezza Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
        }
    }
}
