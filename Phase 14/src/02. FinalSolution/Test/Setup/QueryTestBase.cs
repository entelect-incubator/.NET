namespace Test.Setup;

using System;
using DataAccess;
using LazyCache;
using NUnit.Framework;
using static DatabaseContextFactory;

public class QueryTestBase
{
    private DatabaseContext context;

    public CachingService CachingService { get; private set; }

    public DatabaseContext Context => this.context;

    [SetUp]
    public void BaseSetUp()
    {
        this.context = Create();
        this.CachingService = new CachingService();
    }

    [TearDown]
    public void BaseTearDown()
    {
        if (this.context is not null)
        {
            Destroy(this.context);
            this.context = null;
        }
    }
}
