# AWSCodePipelineExample

This is a non-trivial example of how a Cross-Account CodePipeline should work based on my blog post [AWS CI/CD Tooling](https://purple.telstra.com/blog/aws-ci-cd-tooling). This project requires at least 3 accounts separate AWS accounts.

1. Build Account - This is where the CodePipeline will be deployed and run (it can be the AWS Organization account, but should probably be a dedicated build account in a real project).
1. Dev Account - This is where the first stage of the pipeline will deploy to, and run an initial suite of integration tests
1. Production Account - This is where the second stage of the deployment will deply to and run sanity tests.

In order for this example to run, the infrastructure must be set up oin the following order:

1. The Build Account base infrastructure must be provisioned by executing `BuildAccountBaseInfrastructure.yml` cloudformation script in the Build Account
1. The dev and prod deployment roles need to be provisioned by executing the `CrossAccountCFNRole.yml` cloudformation script, and passing in the arn of the KMS key provisioned in the previous step
1. The CodePipeline can then be provisioned using the `CodePipeline.yml` again passing in the arn of the KMS key to use for artefact encryption which was provisioned as part of the build account base infratructure

This project uses conventions based on Projectname in order to minimize the number of parameters required to pass to the respective cloudformation templates. For example, the codepipeline assumes
that there are roles named `${ProjectName}-{AWS::Region}-DeploymentRole` in both the prod and dev accounts, rather than passing explicit arns in. These roles are set up by the cloudformation scripts as described above.