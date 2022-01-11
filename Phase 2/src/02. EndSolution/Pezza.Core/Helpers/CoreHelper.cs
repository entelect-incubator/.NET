namespace Pezza.Core.Helpers
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Pezza.Common.Models;
    using Pezza.Common.Models.Base;
    using Pezza.DataAccess;

    public class CoreHelper<T>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public CoreHelper(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<Result<T>> Outcome(CancellationToken cancellationToken, EntityBase entity)
        {
            var stackTrace = new StackTrace();
            var methodName = stackTrace?.GetFrame(1)?.GetMethod()?.Name;
            methodName = methodName.Replace("Query", string.Empty);
            methodName = methodName.Replace("Command", string.Empty);

            var outcome = await this.databaseContext.SaveChangesAsync(cancellationToken);
            return (outcome > 0) ? Result<T>.Success(this.mapper.Map<T>(entity)) : Result<T>.Failure(errorMessgae);
        }
    }

    public static class CoreHelper
    {
        public static Result CoreResult(int outcome, string errorMessgae)
            => (outcome > 0) ? Result.Success() : Result.Failure(errorMessgae);
    }
}
