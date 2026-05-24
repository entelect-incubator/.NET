namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Stock;

[TestFixture]

public class TestPizzaCore : QueryTestBase
{
    private PizzaModel dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = PizzaTestData.PizzaModel;
        var sutCreate = new CreateStockCommandHandler(this.Context);
        var resultCreate = await sutCreate.Handle(
            new CreateStockCommand
            {
                Data = this.dto
            }, CancellationToken.None);

        if (!resultCreate.Succeeded)
        {
            Assert.That(false, Is.True);
        }

        this.dto = resultCreate.Data;
    }

    [Test]
    public async Task GetAsync()
    {
        var sutGet = new GetStockQueryHandler(this.Context);
        var resultGet = await sutGet.Handle(
            new GetStockQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetStocksQueryHandler(this.Context);
        var resultGetAll = await sutGetAll.Handle(new GetStocksQuery(), CancellationToken.None);

        Assert.That(resultGetAll!.Data!.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateStockCommandHandler(this.Context);
        var resultUpdate = await sutUpdate.Handle(
            new UpdateStockCommand
            {
                Data = new PizzaModel
                {
                    Id = this.dto.Id,
                    Quantity = 50
                }
            }, CancellationToken.None);

        Assert.That(resultUpdate.Succeeded, Is.True);
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

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
