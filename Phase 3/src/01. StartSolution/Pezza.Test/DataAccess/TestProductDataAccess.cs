namespace Pezza.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestProductDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new ProductDataAccess(this.Context);
            var entity = ProductTestData.Product;
            await handler.SaveAsync(entity);

            var response = await handler.GetAsync(entity.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new ProductDataAccess(this.Context);
            var entity = ProductTestData.Product;
            await handler.SaveAsync(entity);

            var response = await handler.GetAllAsync();
            var outcome = response.Count();

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new ProductDataAccess(this.Context);
            var entity = ProductTestData.Product;
            var result = await handler.SaveAsync(entity);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new ProductDataAccess(this.Context);
            var entity = ProductTestData.Product;
            var originalProduct = entity;
            await handler.SaveAsync(entity);

            entity.Name = new Faker().Commerce.Product();
            var response = await handler.UpdateAsync(entity);
            var outcome = response.Name.Equals(originalProduct.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new ProductDataAccess(this.Context);
            var entity = ProductTestData.Product;
            await handler.SaveAsync(entity);
            
            var response = await handler.DeleteAsync(entity.Id);

            Assert.IsTrue(response);
        }
    }
}
