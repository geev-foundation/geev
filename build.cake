#tool nuget:?package=vswhere
#tool "nuget:?package=xunit.runner.console&version=2.3.0-beta5-build3769"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var projectName = "Geev";
var solution = "./" + projectName + ".sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"tools");
var branch = Argument("branch", EnvironmentVariable("APPVEYOR_REPO_BRANCH"));

var vsLatest  = VSWhereLatest();
var msBuildPathX64 = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");

var testProjects = new List<Tuple<string, string[]>>
                {
                    new Tuple<string, string[]>("Geev.AspNetCore.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.AutoMapper.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.Castle.Log4Net.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.Dapper.NHibernate.Tests", new[] { "net461"}),
                    new Tuple<string, string[]>("Geev.Dapper.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.EntityFramework.GraphDiff.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.EntityFramework.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.EntityFrameworkCore.Dapper.Tests", new[] { "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.EntityFrameworkCore.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.MailKit.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.MemoryDb.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.NHibernate.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Quartz.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.RedisCache.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.TestBase.SampleApplication.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.Web.Api.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Web.Common.Tests", new[] { "net461", "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.Web.Mvc.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Web.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Zero.SampleApp.NHibernateTests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.Zero.SampleApp.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Geev.ZeroCore.Tests", new[] { "netcoreapp2.2" }),
                    new Tuple<string, string[]>("Geev.ZeroCore.IdentityServer4.Tests", new[] { "netcoreapp2.2" })
                };
                      

 
var nugetPath = toolpath + "/nuget.exe";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        Information("Current Branch is:" + branch);
        CleanDirectories("./src/**/bin");
        CleanDirectories("./src/**/obj");
        CleanDirectories("./test/**/bin");
        CleanDirectories("./test/**/obj");
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solution);
        DotNetCoreRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild(solution, new MSBuildSettings(){Configuration = configuration, ToolPath = msBuildPathX64}
                                               .WithProperty("SourceLinkCreate","true"));
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .DoesForEach(testProjects, testProject =>
    {
      foreach (string targetFramework in testProject.Item2)
      {
        Information($"Test execution started for target frameowork: {targetFramework}...");
        var testProj = GetFiles($"./test/**/*{testProject.Item1}.csproj").First();
        DotNetCoreTest(testProj.FullPath, new DotNetCoreTestSettings { Configuration = "Release", Framework = targetFramework });                              
      }
    })
    .DeferOnError();
    

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
