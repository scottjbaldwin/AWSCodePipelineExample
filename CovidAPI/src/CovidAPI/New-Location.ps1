[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [String]
    $ApiUri,
    
    [Parameter(Mandatory=$true)]
    [String]
    $LocationName
)

$loc = @{LocationName=$LocationName} | ConvertTo-Json

$postUri = $ApiUri + "api/Locations"

Invoke-WebRequest -Method POST -Uri $postUri -Body $loc -ContentType "application/json"

