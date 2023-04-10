namespace Test.Setup.TestData.Notify;

using System;
using Bogus;
using Common.DTO;

public static class NotifyTestData
{
    public static Faker faker = new();

    public static NotifyDTO NotifyDTO = new()
    {
        CustomerId = 1,
        Email = faker.Person.Email,
        DateSent = DateTime.Now,
        Retry = 0,
        Sent = true
    };
}
