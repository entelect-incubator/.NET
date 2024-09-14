namespace Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Common.DTO;
    using Core.Restaurant.Commands;
    using Core.Restaurant.Queries;
    using Test.Setup.TestData.Restaurant;

    [TestFixture]

    public class TestRestaurantCore : QueryTestBase
    {
        private RestaurantDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dto = RestaurantTestData.RestaurantDTO;
            var sutCreate = new CreateRestaurantCommandHandler(this.Context, Mapper());
            var resultCreate = await sutCreate.Handle(
                new CreateRestaurantCommand
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
            var sutGet = new GetRestaurantQueryHandler(this.Context, Mapper());
            var resultGet = await sutGet.Handle(
                new GetRestaurantQuery
                {
                    Id = this.dto.Id
                }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetRestaurantsQueryHandler(this.Context, Mapper(), this.CachingService);
            var resultGetAll = await sutGetAll.Handle(new GetRestaurantsQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public void SaveAsync() => Assert.IsTrue(this.dto != null);

        [Test]
        public async Task UpdateAsync()
        {
            var sutUpdate = new UpdateRestaurantCommandHandler(this.Context, Mapper());
            var resultUpdate = await sutUpdate.Handle(
                new UpdateRestaurantCommand
                {
                    Data = new RestaurantDTO
                    {
                        Id = this.dto.Id,
                        Name = "New Restaurant"
                    }
                }, CancellationToken.None);

            Assert.IsTrue(resultUpdate.Succeeded);
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

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
