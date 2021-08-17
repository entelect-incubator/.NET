namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestCustomerDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var dto = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAsync(dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var dto = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var dto = CustomerTestData.CustomerDTO;
            var result = await handler.SaveAsync(dto);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var dto = CustomerTestData.CustomerDTO;
            var originalCustomer = dto;
            await handler.SaveAsync(dto);

            dto.Name = new Faker().Person.FirstName;
            var response = await handler.UpdateAsync(dto);
            var outcome = response.Name.Equals(originalCustomer.Name);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new CustomerDataAccess(this.Context, Mapper());
            var dto = CustomerTestData.CustomerDTO;
            await handler.SaveAsync(dto);

            var response = await handler.DeleteAsync(dto.Id);

            Assert.IsTrue(response);
        }
    }
}
