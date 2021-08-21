namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Customer.Commands;
    using Pezza.Core.Customer.Queries;
    using Pezza.DataAccess.Data;

    [TestFixture]
    public class TestCustomerCore : QueryTestBase
    {
        private CustomerDataAccess dataAccess;

        private CustomerDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dataAccess = new CustomerDataAccess(this.Context, Mapper());
            this.dto = CustomerTestData.CustomerDTO;
            var sutCreate = new CreateCustomerCommandHandler(this.dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
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
            var sutGet = new GetCustomerQueryHandler(this.dataAccess);
            var resultGet = await sutGet.Handle(new GetCustomerQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetCustomersQueryHandler(this.dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery(), CancellationToken.None);

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
            var sutUpdate = new UpdateCustomerCommandHandler(this.dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateCustomerCommand
            {
                Data = new CustomerDTO
                {
                    Id = this.dto.Id,
                    Phone = "0721230000"
                }
            }, CancellationToken.None);

            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var sutDelete = new DeleteCustomerCommandHandler(this.dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteCustomerCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
