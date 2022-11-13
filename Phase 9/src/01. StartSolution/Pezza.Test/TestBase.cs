namespace Pezza.Test;

using Microsoft.EntityFrameworkCore;

public class TestBase : DatabaseContextTest
{
    public TestBase()
    : base(
        new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase("PezzaDb")
            .Options)
    {
    }
}
