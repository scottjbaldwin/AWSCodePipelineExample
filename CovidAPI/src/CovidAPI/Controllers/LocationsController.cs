
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CovidAPI.Model;
using CovidAPI.DynamoDb;

namespace CovidAPI.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private IDynamoDbContextCreator _dynamoDbContextCreator;
        public LocationsController(IDynamoDbContextCreator dynamoDbContextCreator)
        {
            _dynamoDbContextCreator = dynamoDbContextCreator;
        }

        // GET api/locations
        [HttpGet]
        public async Task<IEnumerable<Location>> Get()
        {
            using (var context = _dynamoDbContextCreator.CreateContext())
            {
                return await context.QueryAsync<Location>(Location.LocationPartitionKeyValue).GetRemainingAsync(); 
            }
        }

        // GET api/locations/79427303-2d11-4f4f-a4f2-abc410e99e82
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var context = _dynamoDbContextCreator.CreateContext())
            {
                var location = (await context.QueryAsync<Location>(Location.LocationPartitionKeyValue, 
                    QueryOperator.Equal, 
                    new [] {id.ToString()}).GetRemainingAsync()).FirstOrDefault();

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
        }

        // POST api/locations
        [HttpPost]
        public async Task Post([FromBody]LocationPost locationPost)
        {
            using (var context = _dynamoDbContextCreator.CreateContext())
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
            using (var context = _dynamoDbContextCreator.CreateContext())
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