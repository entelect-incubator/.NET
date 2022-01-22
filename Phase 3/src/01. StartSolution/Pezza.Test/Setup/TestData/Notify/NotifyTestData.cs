namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;

    public static class NotifyTestData
    {
        public static Faker faker = new ();

        public static NotifyDTO NotifyDTO = new ()
        {
            CustomerId = 1,
            Email = faker.Person.Email,
            DateSent = DateTime.Now,
            Retry = 0,
            Sent = true
        };
    }
}
