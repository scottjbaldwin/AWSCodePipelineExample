[CmdletBinding()]
param (
    [String]
    $ApiUri=$env:API_URL
)

$locationsAPI = $ApiUri + "api/locations"
$response = Invoke-WebRequest -Uri $locationsAPI

$locations = $response.Content | ConvertFrom-Json

if ($locations.Count -eq 0) {
    & $PSScriptRoot/SeedData.ps1 -ApiUri $ApiUri
}
$names = @(`
    "Bob Smith", 
    "Sally Potter", 
    "Pablo Veron", 
    "Carlos Gardel", 
    "Anibal Troilo", 
    "Osvaldo Pugliese")

1..10 | ForEach-Object {
    $locationId = $locations[(Get-Random -Maximum $locations.Count)].locationId
    $name = $names[(Get-Random -Maximum $names.Count)]
    $nameParts = $name -split " "
    $email = "$($nameParts[0].ToLower()).$($nameParts[1].ToLower())@example.com"
    $phone = Get-Random -Minimum 10000000 -Maximum 100000000
    
    # Test adding a new registration
    & $PSScriptRoot/New-Registration.ps1 -ApiUri $ApiUri -Name $name -Email $email -Phonenumber $phone -LocationId $locationId

    # Test getting the Location Id
    $loc = (Invoke-WebRequest -Uri "$($locationsAPI)/$locationId").Content | ConvertFrom-Json

    if ($loc.LocationId -ne $locationId) {
        throw "Location id does not match"
    }
}

$registrationsApi = $ApiUri + "api/registrations"
$date = (Get-Date -Format "yyyy-MM-dd")
$results = Invoke-WebRequest -uri "$($registrationsApi)?dt=$date"

$results.Content | Set-Content .\registrations.json