namespace Test.Setup;

using DataAccess;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
	public DatabaseContext Context => Create();

	public void Dispose() => Destroy(this.Context);
}