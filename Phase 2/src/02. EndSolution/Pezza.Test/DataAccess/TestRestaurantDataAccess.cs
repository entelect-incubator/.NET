namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestRestaurantDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new RestaurantDataAccess(this.Context, Mapper());
            var entity = RestaurantTestData.RestaurantDTO;
            await handler.SaveAsync(entity);

            var response = await handler.GetAsync(entity.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new RestaurantDataAccess(this.Context, Mapper());
            var entity = RestaurantTestData.RestaurantDTO;
            await handler.SaveAsync(entity);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new RestaurantDataAccess(this.Context, Mapper());
            var entity = RestaurantTestData.RestaurantDTO;
            var result = await handler.SaveAsync(entity);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new RestaurantDataAccess(this.Context, Mapper());
            var entity = RestaurantTestData.RestaurantDTO;
            var originalRestaurant = entity;
            await handler.SaveAsync(entity);

            entity.Name = new Faker().Commerce.Department();
            var response = await handler.UpdateAsync(entity);
            var outcome = response.Name.Equals(originalRestaurant.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new RestaurantDataAccess(this.Context, Mapper());
            var entity = RestaurantTestData.RestaurantDTO;
            await handler.SaveAsync(entity);

            var response = await handler.DeleteAsync(entity.Id);

            Assert.IsTrue(response);
        }
    }
}
