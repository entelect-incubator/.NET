namespace Pezza.Core.Helpers
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Pezza.Common.Models;
    using Pezza.Common.Models.Base;
    using Pezza.DataAccess;

    public static class CoreHelper<T>
    {
        public static async Task<Result<T>> Outcome(DatabaseContext databaseContext, IMapper mapper, CancellationToken cancellationToken, EntityBase entity, string errorMessage)
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace?.GetFrame(1)?.GetMethod()?.Name;
            methodName = methodName.Replace("Query", string.Empty);
            methodName = methodName.Replace("Command", string.Empty);

            var outcome = await databaseContext.SaveChangesAsync(cancellationToken);
            return (outcome > 0) ? Result<T>.Success(mapper.Map<T>(entity)) : Result<T>.Failure(errorMessage);
        }
    }

    public static class CoreHelper
    {
        public static Result CoreResult(int outcome, string errorMessage)
            => (outcome > 0) ? Result.Success() : Result.Failure(errorMessage);

        public static async Task<Result> Outcome(DatabaseContext databaseContext, CancellationToken cancellationToken, string errorMessage)
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace?.GetFrame(1)?.GetMethod()?.Name;
            methodName = methodName.Replace("Query", string.Empty);
            methodName = methodName.Replace("Command", string.Empty);

            var outcome = await databaseContext.SaveChangesAsync(cancellationToken);
            return (outcome > 0) ? Result.Success() : Result.Failure(errorMessage);
        }
    }
}
