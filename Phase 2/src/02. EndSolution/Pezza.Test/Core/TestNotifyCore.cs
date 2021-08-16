namespace Pezza.Test.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Pezza.Core.Notify.Commands;
    using Pezza.Core.Notify.Queries;
    using Pezza.DataAccess.Data;

    public class TestNotifyCore : QueryTestBase
    {
        [Test]
        public async Task GetAsync()
        {
            var dataAccess = new NotifyDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateNotifyCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateNotifyCommand
            {
                Data = NotifyTestData.NotifyDTO
            }, CancellationToken.None);

            //Act
            var sutGet = new GetNotifyQueryHandler(dataAccess);
            var resultGet = await sutGet.Handle(new GetNotifyQuery
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            Assert.IsTrue(resultGet?.Data != null);
        }

        [Test]
        public async Task GetAllAsync()
        {
            var dataAccess = new NotifyDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateNotifyCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateNotifyCommand
            {
                Data = NotifyTestData.NotifyDTO
            }, CancellationToken.None);

            //Act
            var sutGetAll = new GetNotifiesQueryHandler(dataAccess);
            var resultGetAll = await sutGetAll.Handle(new GetNotifiesQuery(), CancellationToken.None);

            Assert.IsTrue(resultGetAll?.Data.Count == 1);
        }

        [Test]
        public async Task SaveAsync()
        {
            var dataAccess = new NotifyDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateNotifyCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateNotifyCommand
            {
                Data = NotifyTestData.NotifyDTO
            }, CancellationToken.None);

            Assert.IsTrue(resultCreate.Succeeded);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var dataAccess = new NotifyDataAccess(this.Context, Mapper());

            //Act
            var sutCreate = new CreateNotifyCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateNotifyCommand
            {
                Data = NotifyTestData.NotifyDTO
            }, CancellationToken.None);

            //Act
            var sutUpdate = new UpdateNotifyCommandHandler(dataAccess);
            var resultUpdate = await sutUpdate.Handle(new UpdateNotifyCommand
            {
                Data = new Common.DTO.NotifyDTO
                {
                    Id = resultCreate.Data.Id,
                    Email = "test@pezza.co.za"
                }
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(resultUpdate.Succeeded);
        }

        [Test]
        public async Task DeleteAsync()
        {
            var dataAccess = new NotifyDataAccess(this.Context, Mapper());
            //Act
            var sutCreate = new CreateNotifyCommandHandler(dataAccess);
            var resultCreate = await sutCreate.Handle(new CreateNotifyCommand
            {
                Data = NotifyTestData.NotifyDTO
            }, CancellationToken.None);


            //Act
            var sutDelete = new DeleteNotifyCommandHandler(dataAccess);
            var outcomeDelete = await sutDelete.Handle(new DeleteNotifyCommand
            {
                Id = resultCreate.Data.Id
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(outcomeDelete.Succeeded);
        }
    }
}
