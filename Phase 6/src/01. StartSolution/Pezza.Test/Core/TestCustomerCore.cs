namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Customer.Commands;
    using Pezza.Core.Customer.Queries;

    [TestFixture]
    public class TestCustomerCore : QueryTestBase
    {
        private CustomerDTO dto;

        [SetUp]
        public async Task Init()
        {
            this.dto = CustomerTestData.CustomerDTO;
            var sutCreate = new CreateCustomerCommandHandler(Context, Mapper());
            var resultCreate = await sutCreate.Handle(
                new CreateCustomerCommand
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
            var sutGet = new GetCustomerQueryHandler(Context, Mapper());
            var resultGet = await sutGet.Handle(
                new GetCustomerQuery
                {
                    Id = this.dto.Id
                }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetCustomersQueryHandler(Context, Mapper());
            var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public void SaveAsync() => Assert.IsTrue(this.dto != null);

        [Test]
        public async Task UpdateAsync()
        {
            var sutUpdate = new UpdateCustomerCommandHandler(Context, Mapper());
            var resultUpdate = await sutUpdate.Handle(
                new UpdateCustomerCommand
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
            var sutDelete = new DeleteCustomerCommandHandler(Context);
            var outcomeDelete = await sutDelete.Handle(
                new DeleteCustomerCommand
                {
                    Id = this.dto.Id
                }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
