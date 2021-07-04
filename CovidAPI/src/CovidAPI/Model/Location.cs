using System;
using Amazon.DynamoDBv2.DataModel;

namespace CovidAPI.Model
{
    [DynamoDBTable("CovidAPI")]
    public class Location
    {
        public const string LocationPartitionKeyValue = "LOCATION";

        public Location()
        {
            PK = LocationPartitionKeyValue;
        }

        [DynamoDBHashKey("pk")]
        public string PK { get; set; }

        [DynamoDBRangeKey("sk")]
        public string SK { get; set; }

        [DynamoDBProperty("Location")]
        public string LocationName { get; set; }

        [DynamoDBIgnore()]
        public Guid LocationId 
        {
            get 
            {
                return Guid.Parse(SK);
            }
            set
            {
                SK = value.ToString();
            }
        }
    }
}