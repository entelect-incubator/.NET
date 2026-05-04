namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Customer;

[TestFixture]
public class TestCustomerCore : QueryTestBase
{
    private CustomerDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = CustomerTestData.CustomerDTO;
        var sutCreate = new CreateCustomerCommandHandler(this.Context);
        var resultCreate = await sutCreate.Handle(
            new CreateCustomerCommand
            {
                Data = this.dto
            }, CancellationToken.None);

        if (!resultCreate.Succeeded)
        {
            Assert.That(false, Is.True);
        }

        this.dto = resultCreate.Data;
    }

    [Test]
    public async Task GetAsync()
    {
        var sutGet = new GetCustomerQueryHandler(this.Context);
        var resultGet = await sutGet.Handle(
            new GetCustomerQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetCustomersQueryHandler(this.Context);
        var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery(), CancellationToken.None);

        Assert.That(resultGetAll!.Data!.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateCustomerCommandHandler(this.Context);
        var resultUpdate = await sutUpdate.Handle(
            new UpdateCustomerCommand
            {
                Data = new CustomerDTO
                {
                    Id = this.dto.Id,
                    Phone = "0721230000"
                }
            }, CancellationToken.None);

        Assert.That(resultUpdate.Succeeded, Is.True);
    }

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteCustomerCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteCustomerCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
