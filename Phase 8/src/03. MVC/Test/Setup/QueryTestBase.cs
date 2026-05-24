namespace Test.Setup;

using LazyCache;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
	public CachingService CachingService = new();

	public DatabaseContext Context => Create();

	public void Dispose() => Destroy(this.Context);
}