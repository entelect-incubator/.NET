namespace Pezza.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestOrderItemDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new OrderItemDataAccess(this.Context);
            var entity = OrderTestData.OrderItem;
            await handler.SaveAsync(entity);

            var response = await handler.GetAsync(entity.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new OrderItemDataAccess(this.Context);
            var entity = OrderTestData.OrderItem;
            await handler.SaveAsync(entity);

            var response = await handler.GetAllAsync();
            var outcome = response.Count();

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new OrderItemDataAccess(this.Context);
            var entity = OrderTestData.OrderItem;
            var result = await handler.SaveAsync(entity);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new OrderItemDataAccess(this.Context);
            var entity = OrderTestData.OrderItem;
            var originalOrderItem = entity;
            await handler.SaveAsync(entity);

            var productHandler = new ProductDataAccess(this.Context);
            var product = ProductTestData.Product;
            await productHandler.SaveAsync(product);

            entity.ProductId = product.Id;
            var response = await handler.UpdateAsync(entity);
            var outcome = response.ProductId.Equals(originalOrderItem.ProductId);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new OrderItemDataAccess(this.Context);
            var entity = OrderTestData.OrderItem;
            await handler.SaveAsync(entity);

            var response = await handler.DeleteAsync(entity.Id);

            Assert.IsTrue(response);
        }
    }
}
