namespace Pezza.DataAccess.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDatabaseContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
