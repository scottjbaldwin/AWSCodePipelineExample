using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;

namespace CovidAPI
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