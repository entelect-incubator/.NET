namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestOrderDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new OrderDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAsync(dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new OrderDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new OrderDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderDTO;
            var result = await handler.SaveAsync(dto);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new OrderDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderDTO;
            var originalOrder = dto;
            await handler.SaveAsync(dto);

            dto.Amount = new Faker().Finance.Amount();
            var response = await handler.UpdateAsync(dto);
            var outcome = response.Amount.Equals(originalOrder.Amount);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new OrderDataAccess(this.Context, Mapper());
            var dto = OrderTestData.OrderDTO;
            await handler.SaveAsync(dto);

            var response = await handler.DeleteAsync(dto.Id);

            Assert.IsTrue(response);
        }
    }
}
