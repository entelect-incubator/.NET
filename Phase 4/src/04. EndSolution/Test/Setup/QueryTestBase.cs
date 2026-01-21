namespace Test.Setup;

using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
	public DatabaseContext Context => Create();

	public void Dispose() => Destroy(this.Context);
}