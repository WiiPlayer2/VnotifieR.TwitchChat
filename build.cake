///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");

   if(!DirectoryExists("./Build"))
   {
       CreateDirectory("./Build");
   }
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
    CleanDirectory("./Build");

    if(FileExists("./Build.zip"))
    {
        DeleteFile("./Build.zip");
    }
});

Task("Publish")
.IsDependentOn("Clean")
.Does(() => {
    var projectPath = System.IO.Path.GetFullPath(".\\VnotifieR.TwitchChat");
    DotNetCorePublish(projectPath, new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        Runtime = "win10-x64",
        Configuration = configuration,
        OutputDirectory = ".\\Build\\",
    });
});

Task("Copy")
.IsDependentOn("Publish")
.Does(() => {
});

Task("Pack")
.IsDependentOn("Copy")
.Does(() => {
    Zip("./Build", "./Build.zip");
});

RunTarget(target);