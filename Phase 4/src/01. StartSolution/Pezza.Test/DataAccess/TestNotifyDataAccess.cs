namespace Pezza.Test.DataAccess
{
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.DataAccess.Data;

    [TestFixture]
    public class TestNotifyDataAccess : QueryTestBase
    {
        private NotifyDataAccess handler;

        private NotifyDTO dto;

        [SetUp]
        public async Task SetUp()
        {
            this.handler = new NotifyDataAccess(this.Context, Mapper());
            this.dto = NotifyTestData.NotifyDTO;
            this.dto = await this.handler.SaveAsync(this.dto);
        }

        [Test]
        public async Task GetAsync()
        {
            var response = await this.handler.GetAsync(this.dto.Id);

            Assert.IsTrue(response != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var response = await this.handler.GetAllAsync(new NotifyDTO
            {
                Email = this.dto.Email
            });
            var outcome = response.Count;

            Assert.IsTrue(outcome == 1);
        }

        [Test]
        public void SaveAsync()
        {
            Assert.IsTrue(this.dto != null);
        }

        [Test]
        public async Task UpdateAsync()
        {
            this.dto.Email = new Faker().Person.Email;
            var response = await this.handler.UpdateAsync(this.dto);
            var outcome = response.Email.Equals(this.dto.Email);

            Assert.IsTrue(outcome);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var response = await this.handler.DeleteAsync(this.dto.Id);

            Assert.IsTrue(response);
        }
    }
}
