<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# &nbsp;**Pezza - Phase 3 - Step 2**

<br/><br/>

## Step 2: Unit Testing

**Difficulty**: ★★★☆☆ (Intermediate)  
**Estimated Time**: 1.5 - 2 hours  
**Prerequisites**: 

- Completed Step 1 dispatcher scaffolding
- Understanding of NUnit basics

### Learning Outcomes

After completing this step, you will understand:

- Building deterministic test data with Bogus
- Testing dispatcher-based handlers directly (no MediatR)
- Using the in-memory DbContext for fast, isolated tests
- Applying NUnit attributes ([TestFixture], [SetUp], [Test]) and `Assert.That`

---

## What You Build in this Step

1) **Test data** in `Test/Setup/TestData` using Bogus (Customer/Pizza).
2) **Handler tests** under `Test/Core` that call dispatcher handlers directly:
   - `Create*CommandHandler`
   - `Update*CommandHandler`
   - `Delete*CommandHandler`
   - `Get*QueryHandler`
3) **Shared base** (e.g., `QueryTestBase`) configuring EF Core in-memory.

### Key Files (code in `src/02. Step1/Test` for this step)

- `Test.csproj` — NUnit + Bogus + Microsoft.NET.Test.Sdk.
- `Setup/TestData/*` — fake data seeds.
- `Core/*Tests.cs` — command/query handler tests.

### Tips for Writing the Tests

- Instantiate handlers directly with the in-memory `DatabaseContext` from your test base.
- Assert both success paths and failure paths (e.g., not found, null data).
- Reuse Faker-generated data to reduce duplication.
- Keep tests small and focused on handler behavior (no controllers yet).

### Minimal Pattern Example

```csharp
var handler = new CreateCustomerCommandHandler(this.Context);
var result = await handler.Handle(new CreateCustomerCommand
{
    Data = CustomerTestData.CustomerModel
}, CancellationToken.None);

Assert.That(result.Succeeded, Is.True);
Assert.That(result.Data.Id, Is.GreaterThan(0));
```

---

## Checklist

- [ ] Bogus test data created for Customer and Pizza
- [ ] Handlers tested for create, read, update, delete paths
- [ ] In-memory DbContext used for isolation
- [ ] Assertions verify both happy-path and error scenarios

---

## What You Learned

After Step 2, you understand:

- How to test handlers directly without going through the dispatcher
- Why in-memory DbContext creates fast, repeatable tests
- How Bogus generates realistic test data with minimal code
- The value of testing `Result<T>` envelopes instead of exceptions

---

## Next Step

[Go to Step 3 - API Wiring](https://github.com/entelect-incubator/.NET/tree/master/Phase%203/Step%203)
