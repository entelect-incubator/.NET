namespace Pezza.Test.Core;

using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Pezza.Common.DTO;
using Pezza.Core.Notify.Commands;
using Pezza.Core.Notify.Queries;
using Pezza.Test.Setup;
using Pezza.Test.Setup.TestData.Notify;

[TestFixture]

public class TestNotifyCore : QueryTestBase
{
    private NotifyDTO dto;

    [SetUp]
    public async Task Init()
    {
        this.dto = NotifyTestData.NotifyDTO;
        var sutCreate = new CreateNotifyCommandHandler(this.Context, Mapper());
        var resultCreate = await sutCreate.Handle(
            new CreateNotifyCommand
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
        var sutGet = new GetNotifyQueryHandler(this.Context, Mapper());
        var resultGet = await sutGet.Handle(
            new GetNotifyQuery
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(resultGet?.Data != null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetNotifiesQueryHandler(this.Context, Mapper());
        var resultGetAll = await sutGetAll.Handle(new GetNotifiesQuery(), CancellationToken.None);

        Assert.IsTrue(resultGetAll?.Data.Count == 1);
    }

    [Test]
    public void SaveAsync() => Assert.IsTrue(this.dto != null);

    [Test]
    public async Task UpdateAsync()
    {
        var sutUpdate = new UpdateNotifyCommandHandler(this.Context, Mapper());
        var resultUpdate = await sutUpdate.Handle(
            new UpdateNotifyCommand
            {
                Data = new Common.DTO.NotifyDTO
                {
                    Id = this.dto.Id,
                    Email = "test@pezza.co.za"
                }
            }, CancellationToken.None);

        ////Assert
        Assert.IsTrue(resultUpdate.Succeeded);
    }

    [Test]
    public async Task DeleteAsync()
    {
        var sutDelete = new DeleteNotifyCommandHandler(this.Context);
        var outcomeDelete = await sutDelete.Handle(
            new DeleteNotifyCommand
            {
                Id = this.dto.Id
            }, CancellationToken.None);

        Assert.IsTrue(outcomeDelete.Succeeded);
    }
}
