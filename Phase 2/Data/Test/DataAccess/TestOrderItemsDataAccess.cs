namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestOrderItemDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new OrderItemDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderItemDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAsync(dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new OrderItemDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderItemDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new OrderItemDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderItemDTO;
            var result = await handler.SaveAsync(dto);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new OrderItemDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderItemDTO;
            var originalOrderItem = dto;
            await handler.SaveAsync(dto);

            var productHandler = new ProductDataAccess(this.Context, Mapper());
            var product = ProductTestData.ProductDTO;
            await productHandler.SaveAsync(product);

            dto.ProductId = product.Id;
            var response = await handler.UpdateAsync(dto);
            var outcome = response.ProductId.Equals(originalOrderItem.ProductId);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new OrderItemDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderItemDTO;
            await handler.SaveAsync(dto);

            var response = await handler.DeleteAsync(dto.Id);

            Assert.IsTrue(response);
        }
    }
}
