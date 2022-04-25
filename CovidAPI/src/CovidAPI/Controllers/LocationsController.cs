
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CovidAPI.Model;

namespace CovidAPI.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private IAmazonDynamoDB _dynamoDBClient;
        public LocationsController(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        // GET api/locations
        [HttpGet]
        public async Task<IEnumerable<Location>> Get()
        {
            using (var context = new DynamoDBContext(_dynamoDBClient))
            {
                return await context.QueryAsync<Location>(Location.LocationPartitionKeyValue).GetRemainingAsync(); 
            }
        }

        // GET api/locations/79427303-2d11-4f4f-a4f2-abc410e99e82
        [HttpGet("{id}")]
        public async Task<Location> Get(Guid id)
        {
            using (var context = new DynamoDBContext(_dynamoDBClient))
            {
                var location = await context.QueryAsync<Location>(Location.LocationPartitionKeyValue, 
                    QueryOperator.Equal, 
                    new [] {id.ToString()}).GetRemainingAsync();

                return location.FirstOrDefault();
            }
        }

        // POST api/locations
        [HttpPost]
        public async Task Post([FromBody]LocationPost locationPost)
        {
            using (var context = new DynamoDBContext(_dynamoDBClient))
            {
                var location = new Location
                {
                    LocationId = Guid.NewGuid(),
                    LocationName = locationPost.LocationName
                };

                Console.WriteLine($"Saving Location {location.LocationName} to table");
                await context.SaveAsync(location);
            }
        }

        // DELETE api/locations/79427303-2d11-4f4f-a4f2-abc410e99e82
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            using (var context = new DynamoDBContext(_dynamoDBClient))
            {
                var location = new Location
                {
                    PK = Location.LocationPartitionKeyValue,
                    LocationId = id
                };
                context.DeleteAsync<Location>(location);
            }
        }
    }
}