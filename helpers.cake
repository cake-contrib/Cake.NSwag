IEnumerable<string> GetFrameworks(string s) {
    return s.Split(',', ';').Select(fr => fr.Trim());
}

List<NuSpecContent> GetContent(IEnumerable<string> frameworks, DirectoryPath libDir) {
    return frameworks.SelectMany(f => new[] { 
        //new NuSpecContent() { Source = "lib/" + f + "/*", Target = "lib/" + f, Exclude = "*.pdb"},
        new NuSpecContent() { Source = "lib/" + f + "/**", Exclude = "*.pdb"},
        //new NuSpecContent() { Source = artifacts + "lib/" + f + "/*.xml", Target = "lib/" + f}
    })
    .Select(c => {
        //c.Source = c.Source.Trim('.', '/');
        return c;
    }).ToList();
}

void CopyNetCoreDependencies(DependencyCollection deps, DirectoryPath targetPath) {
    foreach (var pkg in deps.Packages) {
        var files = GetFiles(
            deps.PackagesDirectory +
            "/" +
            pkg.Name + 
            "*/lib/" +
            pkg.Framework +
            "/*.dll"
        );
        CopyFiles(files, targetPath);
    }
}


public class DependencyCollection {
    public List<Package> Packages {get;set;}
    public DirectoryPath PackagesDirectory {get;set;}
}

public class Package {
    public Package() {}
    public Package(string name, string version, string framework) {
        Name = name;
        Version = version;
        Framework = framework;
    }
    public string Name {get;set;}
    public string Version {get;set;}
    public string Framework {get;set;}
}

DependencyCollection GetDependencies() {
    return new DependencyCollection {
        Packages = new List<Package> {
            new Package("Newtonsoft.Json", "9.0.1", "netstandard1.0"),
            new Package("NJsonSchema", "6.5.6190.16910", "netstandard1.0"),
            new Package("NJsonSchema.CodeGeneration", "6.5.6190.16910", "netstandard1.0"),
            new Package("NSwag.Annotations", "8.0.6186.17340", "netstandard1.0"),
            new Package("NSwag.CodeGeneration", "8.0.6186.17339", "netstandard1.0"),
            new Package("NSwag.Core", "8.0.6186.17339", "netstandard1.0"),
            new Package("NSwag.AssemblyLoaderCore", "8.0.0", "netstandard1.6"),
            new Package("NConsole", "3.3.6170.27019", "netstandard1.0"),
            new Package("NSwag.Commands", "8.0.6186.17340", "netstandard1.0"),
            new Package("System.Runtime.InteropServices.RuntimeInformation", "4.3.0", "netstandard1.1")
        },
        PackagesDirectory = "./src/packages"
    };
}

void InstallDependencies(DependencyCollection collection, DirectoryPath targetPath) {
    foreach (var pkg in collection.Packages) {
        NuGetInstall(pkg.Name, new NuGetInstallSettings {
            ExcludeVersion  = true,
            Version = pkg.Version,
            OutputDirectory = targetPath,
            Source = new[] { "http://nuget.org/api/v2", "https://api.nuget.org/v3/index.json" }
            });
    }
}