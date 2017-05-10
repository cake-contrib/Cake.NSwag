IEnumerable<string> GetFrameworks(string s) {
    return s.Split(',', ';').Select(fr => fr.Trim());
}

List<NuSpecContent> GetContent(IEnumerable<string> frameworks, DirectoryPath libDir) {
    return frameworks.SelectMany(f => {
        return (GetFiles(libDir + "/" + f + "/*.dll") + GetFiles(libDir + "/" + f + "/*.xml"))
            .Where(file => file.GetFilenameWithoutExtension().ToString() != "Cake.Core")
            .Select(file => new NuSpecContent() { Source = file.ToString(), Target = "lib/" + f});
    })
    .Select(c => {
        //c.Source = c.Source.Trim('.', '/');
        return c;
    }).ToList();
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
            new Package("System.Runtime", "4.3.0", "net45")
        },
        PackagesDirectory = "./src/packages"
    };
}

void InstallDependencies(DependencyCollection collection, DirectoryPath targetPath) {
    foreach (var pkg in collection.Packages) {
        NuGetInstall(pkg.Name, new NuGetInstallSettings {
            Version = pkg.Version,
            OutputDirectory = targetPath,
            NoCache = true,
            Source = new[] { "http://nuget.org/api/v2", "https://api.nuget.org/v3/index.json" }
            });
    }
}