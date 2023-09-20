using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace CovidAPI.DynamoDb
{
    public interface IDynamoDbContextCreator : IDisposable
    {
        public IDynamoDBContext CreateContext();
    }
}