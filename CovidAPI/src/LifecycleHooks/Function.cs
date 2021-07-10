using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.CodeDeploy;
using Amazon.CodeDeploy.Model;
using LifecycleHooks.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LifecycleHooks
{
    public class Function
    {
        private static List<string> _environmentVariables = new List<string> {
            "APPLICATION_NAME",
            "DEPLOYMENT_ID",
            "DEPLOYMENT_GROUP_NAME",
            "DEPLOYMENT_GROUP_ID",
            "LIFECYCLE_EVENT",
            "env"
        };

        private static AmazonCodeDeployClient _codeDeployClient = null;
        private static AmazonCodeDeployClient CodeDeployClient
        {
            get 
            {
                if (_codeDeployClient == null)
                {
                    _codeDeployClient = new AmazonCodeDeployClient();
                }
                return _codeDeployClient;
            }
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="lifecycleEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(LifeCycleEventHook lifecycleEvent, ILambdaContext context)
        {
            Console.WriteLine($"DeploymentId: {lifecycleEvent.DeploymentId}");
            Console.WriteLine($"LifecycleEventHookExecutionId: {lifecycleEvent.LifecycleEventHookExecutionId}");

            // Let's take a look at all of the environment variables we get access to
            Console.WriteLine("----- Environment Variables ------");
            foreach (var name in _environmentVariables)
            {
                WriteEnvironmentVariable(name);
            }

            await CodeDeployClient.PutLifecycleEventHookExecutionStatusAsync(
                new PutLifecycleEventHookExecutionStatusRequest{
                    DeploymentId = lifecycleEvent.DeploymentId,
                    LifecycleEventHookExecutionId = lifecycleEvent.LifecycleEventHookExecutionId,
                    Status = LifecycleEventStatus.Succeeded
                });
            return "";
        }

        public void WriteEnvironmentVariable(string variableName)
        {
            var envVar = System.Environment.GetEnvironmentVariable(variableName);
            Console.WriteLine($"variableName: {envVar}");
        }
    }
}
