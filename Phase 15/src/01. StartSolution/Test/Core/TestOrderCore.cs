namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Imposter.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Order;
using DeliveryNs = global::Core.Delivery;

[TestFixture]

public class TestOrderCore : QueryTestBase
{
    private OrderDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = OrderTestData.OrderDTO;

        var deliveryImposter = new global::Core.Delivery.IDeliveryServiceImposter();
        deliveryImposter
            .CreateDeliveryAsync(Arg<DeliveryNs.Models.CreateDeliveryRequest>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync((DeliveryNs.Models.CreateDeliveryResponse?)null);

        var configImposter = new global::Microsoft.Extensions.Configuration.IConfigurationImposter();

        var sutCreate = new CreateOrderCommandHandler(
            this.Context,
            deliveryImposter.Instance(),
            configImposter.Instance(),
            NullLogger<CreateOrderCommandHandler>.Instance);
        var resultCreate = await sutCreate.Handle(
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
        var sutGet = new GetOrderQueryHandler(this.Context);
        var resultGet = await sutGet.Handle(
            new GetOrderQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetOrdersQueryHandler(this.Context);
        var resultGetAll = await sutGetAll.Handle(new GetOrdersQuery(), CancellationToken.None);

        Assert.That(resultGetAll!.Data!.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteOrderCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteOrderCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
