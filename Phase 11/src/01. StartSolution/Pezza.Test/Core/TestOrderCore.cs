namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Pezza.Pezza.Common.DTO;
using Core.Order.Commands;
using Core.Order.Queries;

[TestFixture]

public class TestOrderCore : QueryTestBase
{
    private OrderDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = OrderTestData.OrderDTO;
        var sutCreate = new CreateOrderCommandHandler(this.Context, Mapper());
        var resultCreate = await sutCreate.HandleAsync(
            new CreateOrderCommand
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
        var sutGet = new GetOrderQueryHandler(this.Context, Mapper());
        var resultGet = await sutGet.HandleAsync(
            new GetOrderQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data , Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetOrdersQueryHandler(this.Context, Mapper());
        var resultGetAll = await sutGetAll.HandleAsync(new GetOrdersQuery(), CancellationToken.None);

        Assert.That(resultGetAll?.Data.Count , Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto , Is.Not.Null);

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteOrderCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.HandleAsync(
            new DeleteOrderCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}

