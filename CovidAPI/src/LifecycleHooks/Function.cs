using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.CodeDeploy;
using Amazon.CodeDeploy.Model;
using LifecycleHooks.Model;
using System.Collections;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LifecycleHooks
{
    public class Function
    {
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
        /// The Pre Traffic CodeDeploy hook
        /// </summary>
        /// <param name="lifecycleEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> PreTrafficHook(LifeCycleEventHook lifecycleEvent, ILambdaContext context)
        {
            Console.WriteLine("****** Pre Traffic Lifecycle Hook ******");
            Console.WriteLine($"DeploymentId: {lifecycleEvent.DeploymentId}");
            Console.WriteLine($"LifecycleEventHookExecutionId: {lifecycleEvent.LifecycleEventHookExecutionId}");

            // Let's take a look at all of the environment variables we get access to
            Console.WriteLine("----- Environment Variables ------");
            WriteEnvironmentVariables();

            await CodeDeployClient.PutLifecycleEventHookExecutionStatusAsync(
                new PutLifecycleEventHookExecutionStatusRequest{
                    DeploymentId = lifecycleEvent.DeploymentId,
                    LifecycleEventHookExecutionId = lifecycleEvent.LifecycleEventHookExecutionId,
                    Status = LifecycleEventStatus.Succeeded
                });
            return "";
        }

        /// <summary>
        /// The Post Traffic CodeDeploy hook
        /// </summary>
        /// <param name="lifecycleEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> PostTrafficHook(LifeCycleEventHook lifecycleEvent, ILambdaContext context)
        {
            Console.WriteLine("****** Post Traffic Lifecycle Hook ******");
            Console.WriteLine($"DeploymentId: {lifecycleEvent.DeploymentId}");
            Console.WriteLine($"LifecycleEventHookExecutionId: {lifecycleEvent.LifecycleEventHookExecutionId}");

            // Let's take a look at all of the environment variables we get access to
            Console.WriteLine("----- Environment Variables ------");
            WriteEnvironmentVariables();

            await CodeDeployClient.PutLifecycleEventHookExecutionStatusAsync(
                new PutLifecycleEventHookExecutionStatusRequest{
                    DeploymentId = lifecycleEvent.DeploymentId,
                    LifecycleEventHookExecutionId = lifecycleEvent.LifecycleEventHookExecutionId,
                    Status = LifecycleEventStatus.Succeeded
                });
            return "";
        }

        public void WriteEnvironmentVariables()
        {
            foreach (DictionaryEntry envVar in System.Environment.GetEnvironmentVariables())
            {
                string variableValue = "";
                switch (envVar.Key)
                {
                    case "AWS_ACCESS_KEY":
                    case "AWS_SECRET_ACCESS_KEY":
                    case "AWS_SESSION_TOKEN":
                        variableValue = "***************";
                        break;
                    default:
                        variableValue = (string)envVar.Value;
                        break;
                }
                Console.WriteLine($"{envVar.Key}: {variableValue}");
            }
        }
    }
}
