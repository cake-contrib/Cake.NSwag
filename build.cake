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
var solution = ParseSolution(solutionPath);
var projects = solution.Projects.Where(p => p.Type != "{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
var projectPaths = projects.Select(p => p.Path.GetDirectory());
var frameworks = GetFrameworks(framework);
var testAssemblies = projects.Where(p => p.Name.Contains(".Tests")).Select(p => p.Path.GetDirectory() + "/bin/" + configuration + "/" + p.Name + ".dll");
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));
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
	foreach(var path in projectPaths)
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
	NuGetRestore(solutionPath);
    foreach(var project in projects) {
        DotNetCoreRestore(project.Path.GetDirectory() + "/project.json");
    }
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
    CreateDirectory(artifacts + "build/");
    foreach(var project in projects) {
        foreach(var f in frameworks) {
            CreateDirectory(artifacts + "build/" + f);
            DotNetCoreBuild(project.Path.GetDirectory().FullPath, new DotNetCoreBuildSettings {
                Framework = f,
                Configuration = configuration,
                OutputDirectory = artifacts + "build/" + f
            });
        }
    }
});

Task("Generate-Docs").Does(() => {
	DocFx("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.IsDependentOn("Generate-Docs")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	var libDir = artifacts + "lib/";
    foreach (var f in frameworks) {
        var frameworkDir = libDir + f;
        CreateDirectory(frameworkDir);
        CopyFiles(GetFiles("./src/Cake.NSwag/bin/" + configuration + "/" + f + "/*.dll"), frameworkDir);
		CopyFiles(GetFiles("./src/Cake.NSwag/bin/" + configuration + "/" + f + "/*.xml"), frameworkDir);
        if (FileExists(frameworkDir + "Cake.Core.dll")) DeleteFile(frameworkDir + "Cake.Core.dll");
    }
});

Task("Copy-Core-Dependencies")
	.IsDependentOn("Post-Build")
	.WithCriteria(() => frameworks.Any(f => f.Contains("netstandard")))
	.Does(() => {
		foreach (var f in frameworks.Where(f => f.Contains("netstandard"))) {
			var deps = GetDependencies();
			InstallDependencies(deps, "./src/packages");
			CopyNetCoreDependencies(deps, artifacts + "lib/" + f);
		}
	});

Task("Copy-Net45-Dependencies")
.IsDependentOn("Post-Build")
.WithCriteria(() => frameworks.Any(f => f.Contains("net4")))
.Does(() => {
	foreach (var f in frameworks.Where(f => f.Contains("net4"))) {
		var frameworkDir = artifacts + "lib/" + f;
		CopyFiles(GetFiles(artifacts + "build/" + f + "/*.dll"), frameworkDir);
		CopyFiles(GetFiles(artifacts + "build/" + f + "/*.xml"), frameworkDir);
		if (FileExists(frameworkDir + "/Cake.Core.dll")) DeleteFile(frameworkDir + "/Cake.Core.dll");
	}
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
    .WithCriteria(() => testAssemblies.Any())
	.Does(() =>
{
    CreateDirectory(testResultsPath);
    var settings = new XUnit2Settings {
        NoAppDomain = true,
        XmlReport = true,
        HtmlReport = true,
        OutputDirectory = testResultsPath,
    };
    settings.ExcludeTrait("Category", "Integration");
    XUnit2(testAssemblies, settings);
});

Task("NuGet")
	.IsDependentOn("Post-Build")
	.IsDependentOn("Copy-Core-Dependencies")
	.IsDependentOn("Copy-Net45-Dependencies")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() => {
		CreateDirectory(artifacts + "package/");
		Information("Building NuGet package");
		var nuspecFiles = GetFiles("./*.nuspec");
		var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == versionInfo.MajorMinorPatch);
        var content = GetContent(frameworks, artifacts + "build/");
		NuGetPack(nuspecFiles, new NuGetPackSettings() {
			BasePath = artifacts,
			Version = versionInfo.NuGetVersionV2,
			ReleaseNotes = versionNotes != null ? versionNotes.Notes.ToList() : new List<string>(),
			OutputDirectory = artifacts + "/package",
            Files = content,
            KeepTemporaryNuSpecFile = true
			});
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
