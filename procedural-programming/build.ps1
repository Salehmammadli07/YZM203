[CmdletBinding()]
param(
    [string]$Output = (Join-Path $PSScriptRoot 'build/prosedurel-programlama.docx'),
    [switch]$Check,
    [switch]$SkipWordFields,
    [switch]$Pdf
)

$ErrorActionPreference = 'Stop'
$builderArguments = @('--root', $PSScriptRoot, '--output', $Output)
if ($Check) { $builderArguments += '--check' }
if ($SkipWordFields) { $builderArguments += '--skip-word-fields' }
if ($Pdf) { $builderArguments += '--pdf' }

dotnet run --project (Join-Path $PSScriptRoot 'tools/WorkbookBuilder') -- @builderArguments
if ($LASTEXITCODE -ne 0) { throw 'C# derlemesi basarisiz.' }
