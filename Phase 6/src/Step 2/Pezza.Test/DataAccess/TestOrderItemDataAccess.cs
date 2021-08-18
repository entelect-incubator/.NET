namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.DataAccess.Data;

    [TestFixture]
    public class TestOrderItemDataAccess : QueryTestBase
    {
        private OrderItemDataAccess handler;

        private OrderItemDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.handler = new OrderItemDataAccess(this.Context, Mapper());
            this.dto = OrderTestData.OrderItemDTO;
            this.dto = await this.handler.SaveAsync(this.dto);
        }

        [Test]
        public async Task GetAsync()
        {
            var response = await this.handler.GetAsync(this.dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var response = await this.handler.GetAllAsync(new OrderItemDTO
            {
                OrderId = this.dto.OrderId
            });
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public void SaveAsync()
        {
            Assert.IsTrue(this.dto != null);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var productHandler = new ProductDataAccess(this.Context, Mapper());
            var product = ProductTestData.ProductDTO;
            await productHandler.SaveAsync(product);

            this.dto.ProductId = product.Id;
            var response = await this.handler.UpdateAsync(this.dto);
            var outcome = response.ProductId.Equals(this.dto.ProductId);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var response = await this.handler.DeleteAsync(this.dto.Id);

            Assert.IsTrue(response);
        }
    }
}
