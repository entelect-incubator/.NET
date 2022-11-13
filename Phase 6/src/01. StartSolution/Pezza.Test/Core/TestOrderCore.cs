namespace Pezza.Test.Core;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Pezza.Common.DTO;
using Pezza.Core.Order.Commands;
using Pezza.Core.Order.Queries;

[TestFixture]

public class TestOrderCore : QueryTestBase
{
    private OrderDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = OrderTestData.OrderDTO;
        var sutCreate = new CreateOrderCommandHandler(Context, Mapper());
        var resultCreate = await sutCreate.Handle(
            new CreateOrderCommand
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
        var sutGet = new GetOrderQueryHandler(Context, Mapper());
        var resultGet = await sutGet.Handle(
            new GetOrderQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(resultGet?.Data != null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetOrdersQueryHandler(Context, Mapper());
        var resultGetAll = await sutGetAll.Handle(new GetOrdersQuery(), CancellationToken.None);

        Assert.IsTrue(resultGetAll?.Data.Count == 1);
    }

    [Test]
    public void SaveAsync() => Assert.IsTrue(this.dto != null);

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteOrderCommandHandler(Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteOrderCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(outcomeDelete.Succeeded);
    }
}
