namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.DataAccess.Data;

    public class TestNotifyDataAccess : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var handler = new NotifyDataAccess(this.Context, Mapper());
            var dto = NotifyTestData.NotifyDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAsync(dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var handler = new NotifyDataAccess(this.Context, Mapper());
            var dto = NotifyTestData.NotifyDTO;
            await handler.SaveAsync(dto);

            var response = await handler.GetAllAsync();
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var handler = new NotifyDataAccess(this.Context, Mapper());
            var dto = NotifyTestData.NotifyDTO;
            var result = await handler.SaveAsync(dto);
            var outcome = result.Id != 0;

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var handler = new NotifyDataAccess(this.Context, Mapper());
            var dto = NotifyTestData.NotifyDTO;
            var originalNotify = dto;
            await handler.SaveAsync(dto);

            dto.Email = new Faker().Person.Email;
            var response = await handler.UpdateAsync(dto);
            var outcome = response.Email.Equals(originalNotify.Email);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var handler = new NotifyDataAccess(this.Context, Mapper());
            var dto = NotifyTestData.NotifyDTO;
            await handler.SaveAsync(dto);

            var response = await handler.DeleteAsync(dto.Id);

            Assert.IsTrue(response);
        }
    }
}
