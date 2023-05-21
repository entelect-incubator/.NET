namespace Test;

public class TestBase : DatabaseContextTest
{
	public TestBase() : base(new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase("PezzaDb").Options)
	{
	}
}
