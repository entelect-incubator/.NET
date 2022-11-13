namespace Pezza.Test;

using System;
using AutoMapper;
using LazyCache;
using Pezza.Common.Profiles;
using Pezza.DataAccess;
using static DatabaseContextFactory;

public class QueryTestBase : IDisposable
{
    public CachingService CachingService = new();

    public static DatabaseContext Context => Create();

    public static IMapper Mapper()
    {
        var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
        return mappingConfig.CreateMapper();
    }

    public void Dispose() => Destroy(Context);
}