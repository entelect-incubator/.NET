namespace Pezza.Test
{
    using System;
    using LazyCache;
    using Pezza.DataAccess;
    using static DatabaseContextFactory;

    public class QueryTestBase : IDisposable
    {
        public CachingService CachingService = new CachingService();

        public DatabaseContext Context => Create();

        public void Dispose() => Destroy(this.Context);
    }
}