namespace Test.Setup;

using System;
using DataAccess;
using LazyCache;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
    public CachingService CachingService = new();

    public DatabaseContext Context => Create();

    public static IObjectMapper Mapper()
    {
        return new ObjectMapper();
    }

    public void Dispose() => Destroy(this.Context);
}