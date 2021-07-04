[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [String]
    $ApiUri
)
$rooms = 1..12 | ForEach-Object {"Cubicle $_"}

$rooms += @("Board Room", `
    "Meeting Room Ford Prefect", `
    "Meeting Room Trillian", `
    "Meeting Room Vogon", `
    "Lunch Room", `
    "Kitchen")

$rooms | Foreach-Object {
    & $PSScriptRoot/New-Location.ps1 -ApiUri $ApiUri -LocationName $_
}