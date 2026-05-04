namespace Test.Core;

using Common.Models.Customer;
using global::Core.Customer.Commands;
using global::Core.Customer.Queries;
using Test.Setup.TestData.Customer;
using static global::Core.Customer.Commands.CreateCustomerCommand;

[TestFixture]
public class TestCustomerCore : QueryTestBase
{
	private CustomerModel model;

	[SetUp]
	public async Task Init()
	{
		this.model = CustomerTestData.CustomerModel;
		var sutCreate = new CreateCustomerCommandHandler(this.Context);
		var resultCreate = await sutCreate.Handle(
			new CreateCustomerCommand
			{
				Data = new CreateCustomerModel
				{
					Name = this.model.Name,
					Email = this.model.Email,
					Address = this.model.Address,
					Cellphone = this.model.Cellphone
				}
			}, CancellationToken.None);

		if (resultCreate.HasError)
		{
			Assert.That(false, Is.True);
		}

		this.model = resultCreate.Data;
	}

	[Test]
	public async Task GetAsync()
	{
		var sutGet = new GetCustomerQueryHandler(this.Context);
		var resultGet = await sutGet.Handle(
			new GetCustomerQuery
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.That(resultGet?.Data, Is.Not.Null);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetCustomersQueryHandler(this.Context);
		var resultGetAll = await sutGetAll.Handle(new GetCustomersQuery(), CancellationToken.None);

		Assert.That(resultGetAll?.Data, Is.Not.Null);
		Assert.That(resultGetAll?.Data.Count(), Is.GreaterThanOrEqualTo(1));
		Assert.That(resultGetAll?.Data.Any(c => c.Id == this.model.Id), Is.True);
	}

	[Test]
	public void SaveAsync() => Assert.That(this.model, Is.Not.Null);

	[Test]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdateCustomerCommandHandler(this.Context);
		var resultUpdate = await sutUpdate.Handle(
			new UpdateCustomerCommand
			{
				Id = this.model.Id,
				Data = new UpdateCustomerModel
				{
					Cellphone = "0721230000"
				}
			}, CancellationToken.None);

		Assert.That(resultUpdate.HasError, Is.False);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeleteCustomerCommandHandler(this.Context);
		var outcomeDelete = await sutDelete.Handle(
			new DeleteCustomerCommand
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.That(outcomeDelete.HasError, Is.False);
	}
}
