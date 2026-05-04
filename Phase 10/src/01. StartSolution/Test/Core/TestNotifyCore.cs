namespace Test.Core;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using NUnit.Framework;
using Test.Setup;
using Test.Setup.TestData.Notify;

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
            Assert.That(false, Is.True);
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

        Assert.That(resultGet?.Data, Is.Not.Null);
    }

    [Test]
    public async Task GetAllAsync()
    {
        var sutGetAll = new GetNotifiesQueryHandler(this.Context, Mapper());
        var resultGetAll = await sutGetAll.Handle(new GetNotifiesQuery(), CancellationToken.None);

        Assert.That(resultGetAll?.Data.Count, Is.EqualTo(1));
    }

    [Test]
    public void SaveAsync() => Assert.That(this.dto, Is.Not.Null);

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
        Assert.That(resultUpdate.Succeeded, Is.True);
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

        Assert.That(outcomeDelete.Succeeded, Is.True);
    }
}
