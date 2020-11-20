namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class NotifyTestData
    {
        public static Faker faker = new Faker();

        public static Notify Notify = new Notify()
        {
            CustomerId = 1,
            DateSent = DateTime.Now,
            Email = faker.Person.Email,
            Retry = 0,
            Sent = true
        };

        public static NotifyDTO NotifyDTO = new NotifyDTO()
        {
            CustomerId = 1,
            DateSent = DateTime.Now,
            Email = faker.Person.Email,
            Retry = 0,
            Sent = true
        };

        public static NotifyDataDTO NotifyDataDTO = new NotifyDataDTO()
        {
            CustomerId = 1,
            Email = faker.Person.Email,
            Retry = 0,
            Sent = true
        };
    }

}
