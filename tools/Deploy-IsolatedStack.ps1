[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [string]
    $IsolatedStackName,

    [Parameter(Mandatory=$true)]
    [string]
    $CFNBucket,

    [Parameter()]
    [string]
    $Environment = "Dev"
)


$gitCommit = git rev-parse --short HEAD

$stat = git status -s
if ($stat.Count -gt 0) {
    Write-Verbose "Current branch not clean, there are changes since the last commit"
    $gitCommit += "X"
}
$samTemplate = Join-Path $PSScriptRoot "/../CovidAPI/src/CovidApi/covidapi.yml"

Write-Verbose "Deploying $samTemplate to isolated stack git commit $gitCommit"

dotnet lambda deploy-serverless "$IsolatedStackName-CovidSafe" `
    -t $samTemplate `
    -sb $CFNBucket `
    -sp "$IsolatedStackName/" `
    -tp "Environment=$Environment;IsolatedStackPrefix=$IsolatedStackName-" `
    --msbuild-parameters "/p:VersionSuffix=isolated-stack-$IsolatedStackName-$gitCommt"