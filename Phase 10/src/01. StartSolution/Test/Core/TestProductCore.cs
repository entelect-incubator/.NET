namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Product;

[TestFixture]

public class TestProductCore : QueryTestBase
{
    private ProductDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = ProductTestData.ProductDTO;
        var sutCreate = new CreateProductCommandHandler(this.Context, Mapper());
        var resultCreate = await sutCreate.Handle(
            new CreateProductCommand
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
        var sutGet = new GetProductQueryHandler(this.Context, Mapper());
        var resultGet = await sutGet.Handle(
            new GetProductQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetProductsQueryHandler(this.Context, Mapper());
        var resultGetAll = await sutGetAll.Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.That(resultGetAll?.Data.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateProductCommandHandler(this.Context, Mapper());
        var resultUpdate = await sutUpdate.Handle(
            new UpdateProductCommand
            {
                Data = new ProductDTO
                {
                    Id = this.dto.Id,
                    Name = "New pizza"
                }
            }, CancellationToken.None);

        Assert.That(resultUpdate.Succeeded, Is.True);
    }

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteProductCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteProductCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
