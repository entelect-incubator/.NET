namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Stock.Commands;
    using Pezza.Core.Stock.Queries;
    using Pezza.DataAccess.Data;

    [TestFixture]

    public class TestStockCore : QueryTestBase
    {
        private StockDataAccess dataAccess;

        private StockDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dataAccess = new StockDataAccess(this.Context, Mapper());
            this.dto = StockTestData.StockDTO;
            var sutCreate = new CreateStockCommandHandler(this.dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateStockCommand
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
            var sutGet = new GetStockQueryHandler(this.dataAccess);
            var resultGet = await sutGet.Handle(new GetStockQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetStocksQueryHandler(this.dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetStocksQuery
            {
                dto = new StockDTO
                {
                    Name = this.dto.Name
                }
            }, CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public void SaveAsync()
        {
            Assert.IsTrue(this.dto != null);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var sutUpdate = new UpdateStockCommandHandler(this.dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateStockCommand
            {
                Data = new StockDTO
                {
                    Id = this.dto.Id,
                    Quantity = 50
                }
            }, CancellationToken.None);

            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var sutDelete = new DeleteStockCommandHandler(this.dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteStockCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
