#tool "GitVersion.CommandLine"
#tool "xunit.runner.console"
#addin "Cake.DocFx"
#tool "docfx.console"

#load "helpers.cake"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var framework = Argument<string>("framework", "net45,netstandard1.6");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Cake.NSwag.sln");
var projects = GetProjects(solutionPath);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));
var frameworks = new List<string> { "net45" };
GitVersion versionInfo = null;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	versionInfo = GitVersion();
	Information("Building for version {0}", versionInfo.FullSemVer);
    Information("Building against '{0}'", framework);
	if (FileExists("./Cake.NSwag.temp.nuspec")) DeleteFile("./Cake.NSwag.temp.nuspec");
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	//NuGetRestore(solutionPath);
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	foreach(var framework in frameworks) {
		foreach (var project in projects.SourceProjectPaths) {
			var settings = new DotNetCoreBuildSettings {
				Framework = framework,
				Configuration = configuration,
				NoIncremental = true,
			};
			DotNetCoreBuild(project.FullPath, settings);
		}
	}
});

Task("Generate-Docs").Does(() => {
	DocFxBuild("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Generate-Docs")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	CreateDirectory(artifacts + "modules");
	foreach (var project in projects.SourceProjects) {
		CreateDirectory(artifacts + "build/" + project.Name);
		foreach (var framework in frameworks) {
			var frameworkDir = artifacts + "build/" + project.Name + "/" + framework;
			CreateDirectory(frameworkDir);
			var files = GetFiles(project.Path.GetDirectory() + "/bin/" + configuration + "/" + framework + "/" + project.Name +".*");
			CopyFiles(files, frameworkDir);
		}
	}
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
    CreateDirectory(testResultsPath);
	if (projects.TestProjects.Any()) {

		var settings = new DotNetCoreTestSettings {
			Configuration = configuration
		};

		foreach(var project in projects.TestProjects) {
			DotNetCoreTest(project.Path.FullPath, settings);
		}
	}
});

Task("NuGet")
	.IsDependentOn("Post-Build")
	.Does(() => 
{
	CreateDirectory(artifacts + "package");
	Information("Building NuGet package");
	var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == versionInfo.MajorMinorPatch);
	var content = GetContent(frameworks, projects);
	var settings = new NuGetPackSettings {
		Id				= "Cake.NSwag",
		Version			= versionInfo.NuGetVersionV2,
		Title			= "Cake.NSwag",
		Authors		 	= new[] { "Alistair Chapman" },
		Owners			= new[] { "achapman", "cake-contrib" },
		Description		= "A simple Cake addin powered by NSwag for compiling client code and API definitions from a variety of sources",
		ReleaseNotes	= versionNotes != null ? versionNotes.Notes.ToList() : new List<string>(),
		Summary			= "Add the NSwag toolchain to your Cake builds.",
		ProjectUrl		= new Uri("https://github.com/agc93/Cake.NSwag"),
		IconUrl			= new Uri("https://raw.githubusercontent.com/NSwag/NSwag/master/assets/NuGetIcon.png"),
		LicenseUrl		= new Uri("https://raw.githubusercontent.com/agc93/Cake.NSwag/master/LICENSE"),
		Copyright		= "Alistair Chapman 2017",
		Tags			= new[] { "cake", "build", "script", "swagger", "api", "openapi" },
		OutputDirectory = artifacts + "/package",
		Files			= content,
		//KeepTemporaryNuSpecFile = true
	};

	NuGetPack(settings);
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("NuGet");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
