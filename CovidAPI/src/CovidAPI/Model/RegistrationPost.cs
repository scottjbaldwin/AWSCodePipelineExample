using System;

namespace CovidAPI.Model
{
    public class RegistrationPost
    {
        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}