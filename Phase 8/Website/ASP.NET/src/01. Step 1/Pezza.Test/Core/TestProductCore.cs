namespace Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Common.DTO;
    using Core.Product.Commands;
    using Core.Product.Queries;

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
                Assert.IsTrue(false);
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

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetProductsQueryHandler(this.Context, Mapper());
            var resultGetAll = await sutGetAll.Handle(new GetProductsQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public void SaveAsync() => Assert.IsTrue(this.dto != null);

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

            Assert.IsTrue(resultUpdate.Succeeded);
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

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
