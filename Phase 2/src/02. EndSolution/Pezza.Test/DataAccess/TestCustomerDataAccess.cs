namespace Pezza.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.DataAccess.Data;

    public class TestCustomerDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var entity = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(entity);

            var response = await handler.GetAsync(entity.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var entity = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(entity);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var entity = CustomerTestData.CustomerDTO;
            var result = await handler.SaveAsync(entity);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var entity = CustomerTestData.CustomerDTO;
            var originalCustomer = entity;
            await handler.SaveAsync(entity);

            entity.Name = new Faker().Person.FirstName;
            var response = await handler.UpdateAsync(entity);
            var outcome = response.Name.Equals(originalCustomer.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var entity = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(entity);

            var response = await handler.DeleteAsync(entity.Id);

            Assert.IsTrue(response);
        }
    }
}
