Write-Host "Adding secret store..."

$csprojPath = ".\*.csproj"
$csprojXml = [Xml](Get-Content $csprojPath)
$userSecretsId = $csprojXml.Project.PropertyGroup.UserSecretsId

if ($null -eq $userSecretsId)
{
    $csprojName = $( Get-Item $csprojPath ).Basename
    Write-Host "UserSecretsId property not found in ${csprojName}"
    Write-Host "Initializing user secrets..."
    dotnet user-secrets init

    $csprojXml = [Xml](Get-Content $csprojPath)
    $userSecretsId = $csprojXml.Project.PropertyGroup.UserSecretsId
}

if ($null -ne $userSecretsId)
{
    Write-Host "UserSecretsId:" $userSecretsId

    $secretsPath = "${env:APPDATA}\Microsoft\UserSecrets\${userSecretsId}\secrets.json"
    if (Test-Path $secretsPath)
    {
        Write-Host "User secrets file found:" $secretsPath
        Write-Host "Skipping initialization."
    }
    else
    {
        Write-Host "Initializing user secrets file..."
        New-Item -Path $secretsPath -ItemType File -Force | Out-Null
        Set-Content -Path $secretsPath -Value "{}"
    }

    $localSecretsPath = ".\localSecrets.json"
    if (Test-Path $localSecretsPath)
    {
        Write-Host "Copying local secrets to user secrets..."
        Get-Content $localSecretsPath | dotnet user-secrets set
    }
}

Write-Host "Done."
