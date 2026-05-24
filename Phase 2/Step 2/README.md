<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# **Pezza - Phase 2 - Step 2**

<br/><br/>

## Step 2: Unit Testing

**Difficulty**: 3/5 (Intermediate)  
**Estimated Time**: 1.5 - 2 hours  
**Prerequisites**:

- Step 1 scaffolding complete
- NUnit basics
- Familiarity with in-memory EF Core

### Learning Outcomes

- Create realistic test data with Bogus
- Test command/query handlers directly via `ExecuteAsync`
- Use in-memory `DatabaseContext` for isolated CRUD tests
- Apply NUnit attributes: `[TestFixture]`, `[SetUp]`, `[Test]`
- Assert results via the Result pattern (`HasError`, `Data`, `Count`)

---

## What You Build in this Step

1) Test data builders using Bogus (Customer + Pizza).  
2) A shared test base that seeds an in-memory `DatabaseContext`.  
3) CRUD tests for command/query classes (`Create`, `Get`, `GetAll`, `Update`, `Delete`).  
4) Result-based assertions instead of exceptions.

Reference implementation: [src/02. EndSolution/Test](../src/02.%20EndSolution/Test).

---

## 1) Test Data

Place Bogus-based builders in [src/02. EndSolution/Test/Setup/TestData](../src/02.%20EndSolution/Test/Setup/TestData).

```csharp
public static class CustomerTestData
{
    public static Faker Faker = new("en_ZA");

    public static CustomerModel CustomerModel => new()
    {
        Id = 1,
        Name = Faker.Person.FullName,
        Address = Faker.Address.FullAddress(),
        Cellphone = Faker.Phone.PhoneNumber(),
        Email = Faker.Person.Email,
        DateCreated = DateTime.UtcNow
    };
}
```

---

## 2) Test Base (In-Memory EF)

`QueryTestBase` in [src/02. EndSolution/Test/Setup](../src/02.%20EndSolution/Test/Setup) spins up an in-memory `DatabaseContext`. Inherit from it for all handler tests so each test gets a fresh context.

Key points:

- Use `CancellationToken.None` (or a token from the test) in every async call.
- Seed data in `[SetUp]` using the real commands/queries, not direct DbContext mutations.

---

## 3) Testing Command/Query Classes

Pattern: arrange handler with the in-memory context, act via `ExecuteAsync`, assert Result.

```csharp
[TestFixture]
public class TestCustomerCore : QueryTestBase
{
    private CustomerModel model;

    [SetUp]
    public async Task Init()
    {
        this.model = CustomerTestData.CustomerModel;
        var create = new CreateCustomerCommand(this.Context);
        var created = await create.ExecuteAsync(
            new CreateCustomerModel
            {
                Name = this.model.Name,
                Email = this.model.Email,
                Address = this.model.Address,
                Cellphone = this.model.Cellphone
            }, CancellationToken.None);

        Assert.That(created.HasError, Is.False);
        this.model = created.Data;
    }

    [Test]
    public async Task GetAsync()
    {
        var query = new GetCustomerQuery(this.Context);
        var result = await query.ExecuteAsync(this.model.Id, CancellationToken.None);
        Assert.That(result.Data, Is.Not.Null);
    }
}
```

Apply the same pattern for `GetCustomersQuery`, `UpdateCustomerCommand`, `DeleteCustomerCommand`, and the Pizza equivalents.

---

## 4) Assertions with Result Pattern

- Success: `Assert.That(result.HasError, Is.False);`
- Data: `Assert.That(result.Data, Is.Not.Null);`
- Counts: `Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));`
- Failure scenarios: create explicit negative tests (e.g., missing ID) and assert `HasError`.

---

## Checklist

- [ ] Test data builders created with Bogus for Customer and Pizza
- [ ] QueryTestBase (in-memory context) inherited by all tests
- [ ] CRUD handlers tested via `ExecuteAsync(..., CancellationToken)`
- [ ] Assertions check `HasError`, `Data`, and `Count`
- [ ] All tests passing: `dotnet test "Phase 2/src/01. StartSolution/Test/Test.csproj"`

---

## Next Step

Move to [Step 3 - API](https://github.com/entelect-incubator/.NET/tree/master/Phase%202/Step%203) to wire controllers to these handlers using dependency injection and the shared `ResponseHelper`.
