namespace Pezza.Test
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Bogus;
    using MediatR;
    using Moq;
    using NUnit.Framework;
    using Pezza.Common.Entities;
    using Pezza.Common.Models;
    using Pezza.Core;
    using Pezza.Core.Stock.Commands;
    using Pezza.Core.Stock.Queries;
    using Pezza.DataAccess.Data;

    public class TestStockCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var result = await mediator.Object.Send(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));

            //Find stock added
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var outcome = await mediator.Object.Send(new GetStockQuery
            {
                Id = result.Data.Id
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));
            Assert.IsTrue(outcome?.Succeeded);
        }

        [Test]
        public async Task GetAllAsync()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var result = await mediator.Object.Send(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));

            //Find stock added
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var outcome = await mediator.Object.Send(new GetStocksQuery());

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));
            Assert.IsTrue(outcome?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var result = await mediator.Object.Send(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var result = await mediator.Object.Send(new CreateStockCommand
            {
                Stock = StockTestData.Stock
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));

            var updateStock = result.Data;
            updateStock.Quantity = 20;

            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<Result<Stock>>(), It.IsAny<CancellationToken>())).ReturnsAsync(It.IsAny<Result<Stock>>());

            //Act
            var outcome = await mediator.Object.Send(new UpdateStockCommand
            {
                Id = updateStock.Id,
                Quantity = 20
            });

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<CreateStockCommand>(), It.IsAny<CancellationToken>()));

            Assert.IsTrue(outcome.Succeeded && outcome.Data.Quantity == updateStock.Quantity);
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
