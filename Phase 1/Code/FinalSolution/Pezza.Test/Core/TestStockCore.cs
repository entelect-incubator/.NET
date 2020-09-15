namespace Pezza.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Core;
    using Pezza.DataAccess.Data;

    public class TestStockCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context));
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);

            var response = await handler.GetAsync(stock.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context));
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);

            var response = await handler.GetAllAsync();
            var outcome = response.Count();

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context));
            var stock = StockTestData.StockDTO;
            var result = await handler.SaveAsync(stock);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context));
            var stock = StockTestData.StockDTO;
            var originalStock = stock;
            await handler.SaveAsync(stock);

            stock.Name = new Faker().Commerce.Product();
            var response = await handler.UpdateAsync(stock);
            var outcome = response.Name.Equals(originalStock.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new StockCore(new StockDataAccess(this.Context));
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);
            
            var response = await handler.DeleteAsync(stock.Id);

            Assert.IsTrue(response);
        }
    }
}
