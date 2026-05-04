namespace Test.Setup;

using System;
using Common.Profiles;
using DataAccess;
using LazyCache;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
    public CachingService CachingService = new();

    public DatabaseContext Context => Create();

    public static IMapper Mapper()
    {
        var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
        return mappingConfig.CreateMapper();
    }

    public void Dispose() => Destroy(this.Context);
}