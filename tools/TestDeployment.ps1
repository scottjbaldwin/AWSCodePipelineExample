[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Environment = "Prod",

    [Parameter()]
    [string]
    $AwsProfilePrefix = "sbaldwin",

    [Parameter()]
    [string]
    $IsolatedStackName = ""
)

$awsProfile = "$AwsProfilePrefix`:$($Environment.ToLower())"
$stackName = "CovidSafe-$Environment-Stack"
if ($IsolatedStackName -ne ""){
    $stackName = "$IsolatedStackName-CovidSafe"
}
$apiUrl = ((aws cloudformation describe-stacks --stack-name $stackName --profile $awsProfile | ConvertFrom-Json).Stacks[0].Outputs | Where-Object OutputKey -eq "ApiURL").OutputValue

$versionUrl = "$apiUrl/api/Version"
$valuesurl = "$apiUrl/api/Values"

Write-Verbose "Stack Name: $stackName, AWS Profile: $awsProfile, API URL: $apiUrl"
$errorCount = 0
$currentVersion = ""
$counter = 0
$bgColor = [System.ConsoleColor]::Blue
if ($Environment -eq "Prod") {
    $bgColor = [System.ConsoleColor]::Red
}
$fgColors = @([System.ConsoleColor]::Green, [System.ConsoleColor]::Magenta)
$versionCount = 0
$col = @{}
$fg = [System.ConsoleColor]::White
for ($i = 0; $i -lt 1000000; $i++) {
    $versionResult = Invoke-WebRequest -Uri $versionUrl
    $valuesResult = Invoke-WebRequest -Uri $valuesurl -SkipHttpErrorCheck

    if ($versionResult.StatusCode -eq 200){
        if ($currentVersion -ne $versionResult.Content) {
            $counter = 0
            $currentVersion = $versionResult.Content
            if (-not $col.ContainsKey($currentVersion)) {
                $col[$currentVersion] = $fgColors[$versionCount++]
            }
            $fg = $col[$currentVersion]
            Write-Host ""
            Write-Host "$Environment -" -NoNewline -BackgroundColor $bgColor
            Write-Host " Version: $currentVersion" -ForegroundColor $fg -NoNewline
        } else {
            $counter += 1
            Write-Host "." -NoNewline -ForegroundColor $fg
            if ($counter % 10 -eq 0) {
                $currentVersion = ""
            }
        }
    }

    if ($valuesResult.StatusCode -ne 200) {
        $errorCount += 1
        Write-Host "E" -ForegroundColor Red -NoNewline
    }
}

Write-Host "#"

Write-Host "Version hash table contains $($versionHash.Count) versions"