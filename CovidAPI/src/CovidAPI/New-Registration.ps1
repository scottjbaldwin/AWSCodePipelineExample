
[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [String]
    $ApiUri,
    
    [Parameter(Mandatory=$true)]
    [String]
    $LocationId,

    [Parameter(Mandatory=$true)]
    [String]
    $Name,

    [Parameter(Mandatory=$true)]
    [String]
    $PhoneNumber,

    [Parameter(Mandatory=$true)]
    [String]
    $Email
)

$registration = @{`
    LocationId=$LocationId;`
    Name=$Name;`
    PhoneNumber=$PhoneNumber;`
    Email=$Email`
} | ConvertTo-Json

$postUri = $ApiUri + "api/registrations"
Invoke-WebRequest -Uri $postUri -Method Post -Body $registration -ContentType "application/json"
