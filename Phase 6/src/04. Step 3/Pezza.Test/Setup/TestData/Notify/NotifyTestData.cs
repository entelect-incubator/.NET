namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class NotifyTestData
    {
        public static Faker faker = new Faker();

        public static NotifyDTO NotifyDTO = new NotifyDTO()
        {
            CustomerId = 1,
            Email = faker.Person.Email,
            DateSent = DateTime.Now,
            Retry = 0,
            Sent = true
        };
    }
}