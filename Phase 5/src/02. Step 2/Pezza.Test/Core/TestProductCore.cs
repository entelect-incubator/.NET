namespace Pezza.Test
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Product.Commands;
    using Pezza.Core.Product.Queries;
    using Pezza.DataAccess.Data;

    public class TestProductCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new ProductDataAccess(this.Context);

            //Act
            var sutCreate = new CreateProductCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
            {
                Data = ProductTestData.ProductDataDTO
            }, CancellationToken.None);

            //Act
            var sutGet = new GetProductQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetProductQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new ProductDataAccess(this.Context);

            //Act
            var sutCreate = new CreateProductCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
            {
                Data = ProductTestData.ProductDataDTO
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetProductsQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetProductsQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new ProductDataAccess(this.Context);

            //Act
            var sutCreate = new CreateProductCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
            {
                Data = ProductTestData.ProductDataDTO
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var dataAccess = new ProductDataAccess(this.Context);

            //Act
            var sutCreate = new CreateProductCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
            {
                Data = ProductTestData.ProductDataDTO
            }, CancellationToken.None);

            //Act
            var sutUpdate = new UpdateProductCommandHandler(dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateProductCommand
            {
                Id = resultCreate.Data.Id,
                Data = new Common.DTO.ProductDataDTO
                {
                    Name = "New pizza"
                }
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new ProductDataAccess(this.Context);
            //Act
            var sutCreate = new CreateProductCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateProductCommand
            {
                Data = ProductTestData.ProductDataDTO
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteProductCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteProductCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
