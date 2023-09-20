
using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace CovidAPI.DynamoDb
{
    public class DynamoDbContextCreator : IDynamoDbContextCreator
    {
        IAmazonDynamoDB _client;
        IDynamoDBContext _context;
        public DynamoDbContextCreator(IAmazonDynamoDB client)
        {
            _client = client;
        }
        public IDynamoDBContext CreateContext()
        {
            if (_context == null)
            {
                _context = _client.CreateDynamoDBContext();
            }

            return _context;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}