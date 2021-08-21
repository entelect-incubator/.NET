namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Common.DTO;
    using Pezza.Core.Product.Commands;
    using Pezza.Core.Product.Queries;
    using Pezza.DataAccess.Data;

    [TestFixture]

    public class TestProductCore : QueryTestBase
    {
        private ProductDataAccess dataAccess;

        private ProductDTO dto;

        [SetUp]
        public async Task SetUp()
        {
            this.dataAccess = new ProductDataAccess(this.Context, Mapper());
            this.dto = ProductTestData.ProductDTO;
            var sutCreate = new CreateProductCommandHandler(this.dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
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
            var sutGet = new GetProductQueryHandler(this.dataAccess);
            var resultGet = await sutGet.Handle(new GetProductQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var sutGetAll = new GetProductsQueryHandler(this.dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetProductsQuery
            {
                dto = new ProductDTO
                {
                    Name = this.dto.Name
                }
            }, CancellationToken.None);

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
            var sutUpdate = new UpdateProductCommandHandler(this.dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateProductCommand
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
            var sutDelete = new DeleteProductCommandHandler(this.dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteProductCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
