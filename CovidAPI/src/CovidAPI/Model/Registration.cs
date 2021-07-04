using System;
using Amazon.DynamoDBv2.DataModel;

namespace CovidAPI.Model
{
    [DynamoDBTable("CovidAPI")]
    public class Registration
    {
        public const string RegistratinPrefix = "Reg_";
        public Registration()
        {
        }
        public Registration(string locationName, RegistrationPost registrationPost)
        {
            LocationName = locationName;
            var creationDate = DateTime.Now;
            RegistrationDate = RegistratinPrefix + creationDate.ToString("yyyy-MM-dd");
            RegistrationSortKey = creationDate.ToString("HH:mm:ss.ffffzzz");
            LocationId = registrationPost.LocationId;
            Name = registrationPost.Name;
            PhoneNumber = registrationPost.PhoneNumber;
            Email = registrationPost.Email;
            RegistrationDateTime = creationDate.ToString("o");
        }
        [DynamoDBHashKey("pk")]
        public string RegistrationDate { get; set; }

        [DynamoDBRangeKey("sk")]
        public string RegistrationSortKey { get; set; }

        public Guid LocationId { get; set; }

        [DynamoDBProperty("Location")]
        public string LocationName { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string RegistrationDateTime { get; set; }
    }
}