namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Restaurant.Commands;
    using Pezza.Core.Restaurant.Queries;
    using Pezza.DataAccess.Data;

    [TestFixture]

    public class TestRestaurantCore : QueryTestBase
    {
        private RestaurantDataAccess dataAccess;

        private RestaurantDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dataAccess = new RestaurantDataAccess(this.Context, Mapper(), this.CachingService);
            this.dto = RestaurantTestData.RestaurantDTO;
            var sutCreate = new CreateRestaurantCommandHandler(this.dataAccess);
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
            var sutGet = new GetRestaurantQueryHandler(this.dataAccess);
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
            var sutGetAll = new GetRestaurantsQueryHandler(this.dataAccess);
            var resultGetAll = await sutGetAll.Handle(
                new GetRestaurantsQuery
                {
                    dto = new RestaurantDTO
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
            var sutUpdate = new UpdateRestaurantCommandHandler(this.dataAccess);
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
            var sutDelete = new DeleteRestaurantCommandHandler(this.dataAccess);
            var outcomeDelete = await sutDelete.Handle(
                new DeleteRestaurantCommand
                {
                    Id = this.dto.Id
                }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
