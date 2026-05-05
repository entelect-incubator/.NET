namespace Test;

public class TestBase : DatabaseContextTest
{
	public TestBase() : base(new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase("EListDb").Options)
	{
	}
}
