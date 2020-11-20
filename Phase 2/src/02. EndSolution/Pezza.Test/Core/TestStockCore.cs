namespace Pezza.Test
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Stock.Commands;
    using Pezza.Core.Stock.Queries;
    using Pezza.DataAccess.Data;

    public class TestStockCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new StockDataAccess(this.Context);

            //Act
            var sutCreate = new CreateStockCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            }, CancellationToken.None);

            //Act
            var sutGet = new GetStockQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetStockQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new StockDataAccess(this.Context);

            //Act
            var sutCreate = new CreateStockCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetStocksQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetStocksQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new StockDataAccess(this.Context);

            //Act
            var sutCreate = new CreateStockCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var dataAccess = new StockDataAccess(this.Context);

            //Act
            var sutCreate = new CreateStockCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            }, CancellationToken.None);

            //Act
            var sutUpdate = new UpdateStockCommandHandler(dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateStockCommand
            {
                Id = resultCreate.Data.Id,
                Quantity = 50
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new StockDataAccess(this.Context);
            //Act
            var sutCreate = new CreateStockCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteStockCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteStockCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
