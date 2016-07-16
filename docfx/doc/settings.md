# Settings

This Cake addin includes a simplified object model for settings compared to the full NSwag settings model. This is in order to maintain the simplicity of your build scripts and expose the most commonly used settings directly, without requiring complex configuration. 

What does this look like in practice? 

```
NSwag.FromWebApiAssembly("./web.assembly.dll").ToSwaggerSpecification("./swagger.json");
// or using the basic settings 
NSwag.FromWebApiAssembly("./web.assembly.dll").ToSwaggerSpecification("./swagger.json", s => s.EnableInterfaces());
```

## Fluent interface

The majority of the settings used by `Cake.NSwag` are available through a simple fluent API. 

```
NSwag.FromWebApiAssembly("./apicontroller.dll")
    .ToSwaggerSpecification("./api.json", s =>
        s.SearchAssemblies("./reference.dll")
            .UseBasePath("api")
            .UseStringEnums()
            .WithTitle("Sample API"));
```            

## Using NSwag settings

NSwag itself includes a number of settings objects much more flexible and capable than `Cake.NSwag`'s own. These can be included using the `WithSettings()` method from any of the generator settings. For example,
```
NSwag.FromSwaggerSpecification("./swagger.json")
    .ToTypeScriptClient("./client.ts", s =>
        s.WithSettings(new SwaggerToTypeScriptClientGeneratorSettings
        {
            Template = TypeScriptTemplate.Angular2,
            PromiseType = PromiseType.Promise
        }));
 ```
## All settings

You can also mix and match these two settings to perform highly advanced code generation with a single call.

```
NSwag.FromSwaggerSpecification("./swagger.json")
    .ToTypeScriptClient("./client.ts", s =>
        s.WithClassName("ApiClient")
            .WithModuleName("SwaggerApi")
            .WithSettings(new SwaggerToTypeScriptClientGeneratorSettings
            {
                PromiseType = PromiseType.Promise
            }));
```