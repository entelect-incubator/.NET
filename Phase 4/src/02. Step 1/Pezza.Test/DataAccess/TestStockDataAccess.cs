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
            var handler = new StockDataAccess(this.Context);
            var entity = StockTestData.Stock;
            await handler.SaveAsync(entity);

            var response = await handler.GetAsync(entity.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new StockDataAccess(this.Context);
            var entity = StockTestData.Stock;
            await handler.SaveAsync(entity);

            var searchModel = new StockDataDTO();
            var response = await handler.GetAllAsync(searchModel);
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new StockDataAccess(this.Context);
            var entity = StockTestData.Stock;
            var result = await handler.SaveAsync(entity);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new StockDataAccess(this.Context);
            var entity = StockTestData.Stock;
            var originalStock = entity;
            await handler.SaveAsync(entity);

            entity.Name = new Faker().Commerce.Product();
            var response = await handler.UpdateAsync(entity);
            var outcome = response.Name.Equals(originalStock.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new StockDataAccess(this.Context);
            var entity = StockTestData.Stock;
            await handler.SaveAsync(entity);
            
            var response = await handler.DeleteAsync(entity.Id);

            Assert.IsTrue(response);
        }
    }
}
