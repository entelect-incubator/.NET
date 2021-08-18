namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Order.Commands;
    using Pezza.Core.Order.Queries;
    using Pezza.DataAccess.Data;

    [TestFixture]

    public class TestOrderCore : QueryTestBase
    {
        private OrderDataAccess dataAccess;

        private OrderDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dataAccess = new OrderDataAccess(this.Context, Mapper());
            this.dto = OrderTestData.OrderDTO;
            var sutCreate = new CreateOrderCommandHandler(this.dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateOrderCommand
            {
                Data = this.dto
            }, CancellationToken.None);

            if (!resultCreate.Succeeded)
            {
                Assert.IsTrue(false);
            }

            this.dto = resultCreate.Data;
        }

        [Test]
        public async Task GetAsync()
        {
            var sutGet = new GetOrderQueryHandler(this.dataAccess);
            var resultGet = await sutGet.Handle(new GetOrderQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetOrdersQueryHandler(this.dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetOrdersQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public void SaveAsync()
        {
            Assert.IsTrue(this.dto != null);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var sutDelete = new DeleteOrderCommandHandler(this.dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteOrderCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
