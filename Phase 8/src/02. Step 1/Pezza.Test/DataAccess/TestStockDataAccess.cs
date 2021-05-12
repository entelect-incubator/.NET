namespace Pezza.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.DataAccess.Data;

    public class TestStockDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new StockDataAccess(this.Context, Mapper());
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);

            var response = await handler.GetAsync(stock.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new StockDataAccess(this.Context, Mapper());
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);

            var response = await handler.GetAllAsync(new StockDTO());
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new StockDataAccess(this.Context, Mapper());
            var stock = StockTestData.StockDTO;
            var result = await handler.SaveAsync(stock);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new StockDataAccess(this.Context, Mapper());
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
            var handler = new StockDataAccess(this.Context, Mapper());
            var stock = StockTestData.StockDTO;
            await handler.SaveAsync(stock);

            var response = await handler.DeleteAsync(stock.Id);

            Assert.IsTrue(response);
        }
    }
}
