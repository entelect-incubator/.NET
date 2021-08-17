namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Order.Commands;
    using Pezza.Core.Order.Queries;
    using Pezza.DataAccess.Data;

    public class TestOrderCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new OrderDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateOrderCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateOrderCommand
            {
                Data = OrderTestData.OrderDTO
            }, CancellationToken.None);

            //Act
            var sutGet = new GetOrderQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetOrderQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new OrderDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateOrderCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateOrderCommand
            {
                Data = OrderTestData.OrderDTO
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetOrdersQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetOrdersQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new OrderDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateOrderCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateOrderCommand
            {
                Data = OrderTestData.OrderDTO
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new OrderDataAccess(this.Context, Mapper());
            //Act
            var sutCreate = new CreateOrderCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateOrderCommand
            {
                Data = OrderTestData.OrderDTO
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteOrderCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteOrderCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
