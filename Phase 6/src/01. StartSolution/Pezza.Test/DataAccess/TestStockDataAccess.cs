namespace Pezza.Test.DataAccess
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.DataAccess.Data;

    [TestFixture]
    public class TestStockDataAccess : QueryTestBase
    {
        private StockDataAccess handler;

        private StockDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.handler = new StockDataAccess(this.Context, Mapper());
            this.dto = StockTestData.StockDTO;
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
            var response = await this.handler.GetAllAsync(new StockDTO
            {
                Name = this.dto.Name
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
            var originalDTO = this.dto;
            this.dto.Name = new Faker().Commerce.Product();
            var response = await this.handler.UpdateAsync(this.dto);
            var outcome = response.Name.Equals(originalDTO.Name);

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
