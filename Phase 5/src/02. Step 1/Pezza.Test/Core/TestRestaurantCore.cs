namespace Pezza.Test
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Restaurant.Commands;
    using Pezza.Core.Restaurant.Queries;
    using Pezza.DataAccess.Data;

    public class TestRestaurantCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new RestaurantDataAccess(this.Context, this.CachingService);

            //Act
            var sutCreate = new CreateRestaurantCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateRestaurantCommand
            {
                Data = RestaurantTestData.RestaurantDataDTO
            }, CancellationToken.None);

            //Act
            var sutGet = new GetRestaurantQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetRestaurantQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new RestaurantDataAccess(this.Context, this.CachingService);

            //Act
            var sutCreate = new CreateRestaurantCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateRestaurantCommand
            {
                Data = RestaurantTestData.RestaurantDataDTO
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetRestaurantsQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetRestaurantsQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new RestaurantDataAccess(this.Context, this.CachingService);

            //Act
            var sutCreate = new CreateRestaurantCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateRestaurantCommand
            {
                Data = RestaurantTestData.RestaurantDataDTO
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var dataAccess = new RestaurantDataAccess(this.Context, this.CachingService);

            //Act
            var sutCreate = new CreateRestaurantCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateRestaurantCommand
            {
                Data = RestaurantTestData.RestaurantDataDTO
            }, CancellationToken.None);

            //Act
            var sutUpdate = new UpdateRestaurantCommandHandler(dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateRestaurantCommand
            {
                Id = resultCreate.Data.Id,
                Data = new Common.DTO.RestaurantDataDTO
                {
                    Name = "New Restaurant"
                }
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new RestaurantDataAccess(this.Context, this.CachingService);
            //Act
            var sutCreate = new CreateRestaurantCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateRestaurantCommand
            {
                Data = RestaurantTestData.RestaurantDataDTO
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteRestaurantCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteRestaurantCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
