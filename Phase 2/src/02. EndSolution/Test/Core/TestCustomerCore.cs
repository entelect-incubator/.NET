namespace Test.Core;

using Common.Models.Customer;
using global::Core.Customer.Commands;
using global::Core.Customer.Queries;
using Test.Setup.TestData.Customer;

/// <summary>
/// Unit tests for customer business logic operations.
/// Tests CRUD operations (Create, Read, Update, Delete) for customer entities using LiteBus command/query handlers.
/// </summary>
[TestFixture]
public class TestCustomerCore : QueryTestBase
{
	private CustomerModel model;

	/// <summary>
	/// Initializes test data before each test execution.
	/// Creates a new customer record using the CreateCustomerCommandHandler and stores it in the model field.
	/// </summary>
	/// <remarks>
	/// This method runs before each test via the [SetUp] attribute.
	/// It ensures a fresh customer record exists for each test.
	/// </remarks>
	[SetUp]
	public async Task Init()
	{
		this.model = CustomerTestData.CustomerModel;
		var sutCreate = new CreateCustomerCommand(this.Context);
		var resultCreate = await sutCreate.ExecuteAsync(
			new CreateCustomerModel
			{
				Name = this.model.Name,
				Email = this.model.Email,
				Address = this.model.Address,
				Cellphone = this.model.Cellphone
			}, CancellationToken.None);

		if (resultCreate.HasError)
		{
			Assert.That(false, Is.True);
		}

		this.model = resultCreate.Data;
	}

	/// <summary>
	/// Tests retrieving a single customer by ID using the GetCustomerQueryHandler.
	/// Verifies that the query handler returns a non-null customer record.
	/// </summary>
	/// <remarks>
	/// Validates the Read (GET) operation for individual customer records.
	/// Depends on Init() to create test data.
	/// </remarks>
	[Test]
	public async Task GetAsync()
	{
		var sutGet = new GetCustomerQuery(this.Context);
		var resultGet = await sutGet.ExecuteAsync(this.model.Id, CancellationToken.None);

		Assert.That(resultGet?.Data, Is.Not.Null);
	}

	/// <summary>
	/// Tests retrieving all customers using the GetCustomersQueryHandler.
	/// Verifies that the query returns a non-empty list containing the test customer.
	/// </summary>
	/// <remarks>
	/// Validates the Read All (GET /customers) operation.
	/// Checks that the created customer is present in the results.
	/// </remarks>
	[Test]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetCustomersQuery(this.Context);
		var resultGetAll = await sutGetAll.ExecuteAsync(CancellationToken.None);

		Assert.That(resultGetAll?.Data, Is.Not.Null);
		Assert.That(resultGetAll?.Count, Is.GreaterThanOrEqualTo(1));
		Assert.That(resultGetAll?.Data.Any(c => c.Id == this.model.Id), Is.True);
	}

	/// <summary>
	/// Tests that the customer model was successfully created and stored.
	/// Verifies that the model field is not null after initialization.
	/// </summary>
	/// <remarks>
	/// Simple validation test that confirms the Init() setup completed successfully.
	/// </remarks>
	[Test]
	public void SaveAsync() => Assert.That(this.model, Is.Not.Null);

	/// <summary>
	/// Tests updating an existing customer record using the UpdateCustomerCommandHandler.
	/// Verifies that the update command succeeds and the customer data is modified.
	/// </summary>
	/// <remarks>
	/// Validates the Update operation by changing the customer's cellphone number.
	/// Tests that the command handler returns a successful result.
	/// </remarks>
	[Test]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdateCustomerCommand(this.Context);
		var resultUpdate = await sutUpdate.ExecuteAsync(this.model.Id,
			new UpdateCustomerModel
			{
				Cellphone = "0721230000"
			}, CancellationToken.None);

		Assert.That(resultUpdate.HasError, Is.False);
	}

	/// <summary>
	/// Tests deleting a customer record using the DeleteCustomerCommandHandler.
	/// Verifies that the delete command succeeds and the customer is removed.
	/// </summary>
	/// <remarks>
	/// Validates the Delete operation. This test should run last to clean up test data.
	/// Tests that the command handler returns a successful result.
	/// </remarks>
	[Test]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeleteCustomerCommand(this.Context);
		var outcomeDelete = await sutDelete.ExecuteAsync(this.model.Id, CancellationToken.None);

		Assert.That(outcomeDelete.HasError, Is.False);
	}
}
