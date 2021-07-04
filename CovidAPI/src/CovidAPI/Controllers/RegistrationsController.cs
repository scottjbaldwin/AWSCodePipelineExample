using System;
using System.Globalization;
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
    public class RegistrationsController : ControllerBase
    {
        private IAmazonDynamoDB _dynamoDBClient;

        public RegistrationsController(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }
        // POST api/registrations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationPost registrationPost)
        {
            using(var context = new DynamoDBContext(_dynamoDBClient))
            {
                Console.WriteLine($"Looking up location Id {registrationPost.LocationId}");
                var location = await context.QueryAsync<Location>(Location.LocationPartitionKeyValue, 
                    QueryOperator.Equal, 
                    new [] {registrationPost.LocationId.ToString()}).GetRemainingAsync();

                var checkinLocation = location.FirstOrDefault();
                if (checkinLocation == null)
                {
                    return NotFound();
                }

                var registration = new Registration(checkinLocation.LocationName, registrationPost);

                await context.SaveAsync(registration);

                return Ok();
            }
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Registration>>> GetByDateAndLocation(string dt, string locationId = "")
        {
            Console.WriteLine($"Attempting to parse date {dt}");

            DateTime registrationDate;
            if (!DateTime.TryParseExact(dt, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out registrationDate))
            {
                return BadRequest();
            }

            var useLocationId = false;
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(locationId))
            {
                if (!Guid.TryParse(locationId, out id))
                {
                    return BadRequest();
                }
                Console.WriteLine($"Looking for registrations in location {locationId}");
                useLocationId = true;
            }

            using (var context = new DynamoDBContext(_dynamoDBClient))
            {
                Console.WriteLine($"Looking up registrations for date {registrationDate}");

                var registrations = await context.QueryAsync<Registration>($"{Registration.RegistratinPrefix}{dt}").GetRemainingAsync();

                if (useLocationId)
                {
                    registrations = registrations.Where(r => r.LocationId == id).ToList();
                }

                return Ok(registrations);
            }
        }
    }
}