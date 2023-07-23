namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Common.DTO;
using Core.Stock.Commands;
using Core.Stock.Queries;

[TestFixture]

public class TestPizzaCore : QueryTestBase
{
    private PizzaModel dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = PizzaTestData.PizzaModel;
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
                Data = new PizzaModel
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
