# Packages

You can include the addin in your script with:

```
#addin nuget:?package=Cake.NSwag

//or to use the latest development release
#addin nuget:?package=Cake.NSwag&prerelease
```

The NuGet prerelease packages are automatically built and deployed from the `develop` branch so they can be considered bleeding-edge while the non-prerelease packages will be much more stable.

Versioning is predominantly SemVer-compliant so you can set your version constraints if you're worried about changes.

> **NOTE:** These packages include their dependent NSwag libraries at this time, as Cake is currently unable to correctly reference NuGet dependencies.