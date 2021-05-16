# AWSCodePipelineExample

This is a non-trivial example of how a Cross-Account CodePipeline should work based on my blog post [AWS CI/CD Tooling](https://purple.telstra.com/blog/aws-ci-cd-tooling). This project requires an AWS Organisation structure with at least 3 accounts
1. Build Account - This is where the CodePipeline will be deployed and run (it can be the AWS Organization account, but should probably be a dedicated build account in a real project).
2. Dev Account - This is where the first stage of the pipeline will deploy to, and run an initial suite of integration tests
3. Production Account - This is where the second stage of the deployment will deply to and run sanity tests.
