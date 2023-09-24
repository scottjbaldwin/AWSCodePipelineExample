using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;

namespace CovidAPI.DynamoDb
{
    public static class DynamoDBHelper
    {
        public static DynamoDBContext CreateDynamoDBContext(this IAmazonDynamoDB client)
        {
            var prefix = Environment.GetEnvironmentVariable("TablePrefix");
            var config = new DynamoDBContextConfig();
            if (!string.IsNullOrEmpty(prefix))
            {
                config.TableNamePrefix = prefix;
            }

            return new DynamoDBContext(client, config);
        }
    }
}