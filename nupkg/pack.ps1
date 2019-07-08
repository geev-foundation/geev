# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "Geev",
    "Geev.AspNetCore",
    "Geev.AspNetCore.OData",
    "Geev.AspNetCore.SignalR",
    "Geev.AspNetCore.TestBase",
    "Geev.AutoMapper",
    "Geev.Castle.Log4Net",
    "Geev.Dapper",
    "Geev.EntityFramework",
    "Geev.EntityFramework.Common",
    "Geev.EntityFramework.GraphDiff",
    "Geev.EntityFrameworkCore",
	"Geev.EntityFrameworkCore.EFPlus",
    "Geev.FluentMigrator",
	"Geev.FluentValidation",
    "Geev.HangFire",
    "Geev.HangFire.AspNetCore",
    "Geev.MailKit",
    "Geev.MemoryDb",
    "Geev.MongoDB",
    "Geev.NHibernate",
    "Geev.Owin",
    "Geev.RedisCache",
    "Geev.RedisCache.ProtoBuf",
    "Geev.Quartz",
    "Geev.TestBase",
    "Geev.Web",
    "Geev.Web.Api",
    "Geev.Web.Api.OData",
    "Geev.Web.Common",
    "Geev.Web.Mvc",
    "Geev.Web.SignalR",
    "Geev.Web.Resources",
    "Geev.Zero",
    "Geev.Zero.AspNetCore",
    "Geev.Zero.Common",
    "Geev.Zero.EntityFramework",
    "Geev.Zero.EntityFrameworkCore",
    "Geev.Zero.Ldap",
    "Geev.Zero.NHibernate",
    "Geev.Zero.Owin",
    "Geev.ZeroCore",
    "Geev.ZeroCore.EntityFramework",
    "Geev.ZeroCore.EntityFrameworkCore",
    "Geev.ZeroCore.IdentityServer4",
    "Geev.ZeroCore.IdentityServer4.EntityFrameworkCore"    
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the pack folder
Set-Location $packFolder