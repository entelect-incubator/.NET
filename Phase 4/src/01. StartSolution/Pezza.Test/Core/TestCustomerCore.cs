namespace Pezza.Test
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Customer.Commands;
    using Pezza.Core.Customer.Queries;
    using Pezza.DataAccess.Data;

    public class TestCustomerCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new CustomerDataAccess(this.Context);

            //Act
            var sutCreate = new CreateCustomerCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
            {
                Data = CustomerTestData.CustomerDataDTO
            }, CancellationToken.None);

            //Act
            var sutGet = new GetCustomerQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetCustomerQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new CustomerDataAccess(this.Context);

            //Act
            var sutCreate = new CreateCustomerCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
            {
                Data = CustomerTestData.CustomerDataDTO
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetCustomersQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new CustomerDataAccess(this.Context);

            //Act
            var sutCreate = new CreateCustomerCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
            {
                Data = CustomerTestData.CustomerDataDTO
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var dataAccess = new CustomerDataAccess(this.Context);

            //Act
            var sutCreate = new CreateCustomerCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
            {
                Data = CustomerTestData.CustomerDataDTO
            }, CancellationToken.None);

            //Act
            var sutUpdate = new UpdateCustomerCommandHandler(dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateCustomerCommand
            {
                Id = resultCreate.Data.Id,
                Data = new Common.DTO.CustomerDataDTO
                {
                    Phone = "0721230000"
                }
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new CustomerDataAccess(this.Context);
            //Act
            var sutCreate = new CreateCustomerCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateCustomerCommand
            {
                Data = CustomerTestData.CustomerDataDTO
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteCustomerCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteCustomerCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
