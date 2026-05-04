namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Restaurant;

[TestFixture]

public class TestRestaurantCore : QueryTestBase
{
    private RestaurantDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = RestaurantTestData.RestaurantDTO;
        var sutCreate = new CreateRestaurantCommandHandler(this.Context);
        var resultCreate = await sutCreate.Handle(
            new CreateRestaurantCommand
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
        var sutGet = new GetRestaurantQueryHandler(this.Context);
        var resultGet = await sutGet.Handle(
            new GetRestaurantQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetRestaurantsQueryHandler(this.Context, this.CachingService);
        var resultGetAll = await sutGetAll.Handle(new GetRestaurantsQuery(), CancellationToken.None);

        Assert.That(resultGetAll!.Data!.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateRestaurantCommandHandler(this.Context);
        var resultUpdate = await sutUpdate.Handle(
            new UpdateRestaurantCommand
            {
                Data = new RestaurantDTO
                {
                    Id = this.dto.Id,
                    Name = "New Restaurant"
                }
            }, CancellationToken.None);

        Assert.That(resultUpdate.Succeeded, Is.True);
    }

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteRestaurantCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteRestaurantCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
