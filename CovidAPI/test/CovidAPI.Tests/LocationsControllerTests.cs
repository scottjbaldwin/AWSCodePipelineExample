using Xunit;
using Moq;
using ThirdParty.LitJson;
using CovidAPI.DynamoDb;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CovidAPI.Controllers;
using CovidAPI.Model;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CovidAPI.Tests
{
    public class MockAsyncSearch<T> : AsyncSearch<T>
    {
        private List<T> _data;
        public MockAsyncSearch(IEnumerable<T> data) : base()
        {
            _data = new List<T>(data);
        }

        public override Task<List<T>> GetRemainingAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(_data);
        }
    }
    public class LocationsControllerTest
    {
        [Fact]
        public async Task BasicGetTest()
        {
            // Arrange
            var contextCreator = new Mock<IDynamoDbContextCreator>();
            var dynamoDbContext = new Mock<IDynamoDBContext>();

            var locationId = Guid.NewGuid();

            dynamoDbContext.Setup(d => d.QueryAsync<Location>(
                It.IsAny<object>(), 
                It.IsAny<QueryOperator>(), 
                It.IsAny<IEnumerable<object>>(), 
                It.IsAny<DynamoDBOperationConfig>())).Returns(
                    new MockAsyncSearch<Location>(
                        new List<Location>{
                            new Location
                            {
                                LocationId = locationId,
                                LocationName = "Test Location"
                            }
                        }
                    )
                );

            contextCreator.Setup(c => c.CreateContext()).Returns(dynamoDbContext.Object);
            var controller = new LocationsController(contextCreator.Object);
        
            // When
            var result = await controller.Get(locationId);
        
            // Then
            var objectResult = result as OkObjectResult;
            Assert.NotNull(objectResult);
            
            var location = objectResult.Value as Location;
            Assert.NotNull(location);
            Assert.Equal("Test Location", location.LocationName);
        }

        [Fact]
        public async Task NotFoundTest()
        {
            // Arrange
            var contextCreator = new Mock<IDynamoDbContextCreator>();
            var dynamoDbContext = new Mock<IDynamoDBContext>();

            var locationId = Guid.NewGuid();

            dynamoDbContext.Setup(d => d.QueryAsync<Location>(
                It.IsAny<object>(), 
                It.IsAny<QueryOperator>(), 
                It.IsAny<IEnumerable<object>>(), 
                It.IsAny<DynamoDBOperationConfig>())).Returns(
                    new MockAsyncSearch<Location>(
                        new List<Location>()
                    )
                );

            contextCreator.Setup(c => c.CreateContext()).Returns(dynamoDbContext.Object);
            var controller = new LocationsController(contextCreator.Object);
        
            // When
            var result = await controller.Get(locationId);
        
            // Then
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
        }
    }
}