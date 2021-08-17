namespace Pezza.Test.Core
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Core;
    using Pezza.DataAccess.Data;
    using Pezza.Test.Setup;
    using Pezza.Test.Setup.TestData.Stock;

    [TestFixture]
    public class TestStockCore : QueryTestBase
    {

        [Test]
        public async Task GetAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context, Mapper()));
            var stockDTO = StockTestData.StockDTO;
            await handler.SaveAsync(stockDTO);

            var response = await handler.GetAsync(stockDTO.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context, Mapper()));
            var stockDTO = StockTestData.StockDTO;
            await handler.SaveAsync(stockDTO);

            var response = await handler.GetAllAsync();
            var outcome = response.Count();

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context, Mapper()));
            var stockDTO = StockTestData.StockDTO;
            var result = await handler.SaveAsync(stockDTO);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context, Mapper()));
            var stockDTO = StockTestData.StockDTO;
            var originalStock = stockDTO;
            await handler.SaveAsync(stockDTO);

            stockDTO.Name = new Faker().Commerce.Product();
            var response = await handler.UpdateAsync(stockDTO);
            var outcome = response.Name.Equals(originalStock.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context, Mapper()));
            var stockDTO = StockTestData.StockDTO;
            await handler.SaveAsync(stockDTO);

            var response = await handler.DeleteAsync(stockDTO.Id);

            Assert.IsTrue(response);
        }
    }
}
