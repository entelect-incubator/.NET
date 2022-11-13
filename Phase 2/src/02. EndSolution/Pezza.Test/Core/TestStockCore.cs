namespace Pezza.Test.Core;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Pezza.Common.DTO;
using Pezza.Core.Stock.Commands;
using Pezza.Core.Stock.Queries;
using Pezza.Test.Setup;
using Pezza.Test.Setup.TestData.Stock;

[TestFixture]

public class TestStockCore : QueryTestBase
{
    private StockDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = StockTestData.StockDTO;
        var sutCreate = new CreateStockCommandHandler(this.Context, Mapper());
        var resultCreate = await sutCreate.Handle(
            new CreateStockCommand
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
        var sutGet = new GetStockQueryHandler(this.Context, Mapper());
        var resultGet = await sutGet.Handle(
            new GetStockQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(resultGet?.Data != null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetStocksQueryHandler(this.Context, Mapper());
        var resultGetAll = await sutGetAll.Handle(new GetStocksQuery(), CancellationToken.None);

        Assert.IsTrue(resultGetAll?.Data.Count == 1);
    }

    [Test]
    public void SaveAsync() => Assert.IsTrue(this.dto != null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateStockCommandHandler(this.Context, Mapper());
        var resultUpdate = await sutUpdate.Handle(
            new UpdateStockCommand
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
        var sutDelete = new DeleteStockCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteStockCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(outcomeDelete.Succeeded);
    }
}
