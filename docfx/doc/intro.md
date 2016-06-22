# Getting Started

The Cake addin ships with all of the NSwag libraries bundled in the package, so you shouldn't need to install anything first.

## Including the addin

At the top of your script, just add the following to install the addin:

```
#addin nuget:?package=Cake.NSwag
```

## Usage

The addin exposes a single property alias `NSwag` with all of the NSwag functionality available as methods.

The general process of using the alias is to get a source (a `GenerationSource` implementation) and then output it to any number of generated targets. So, generating a Swagger spec from a Web API assembly is simply:

```
NSwag.FromWebApiAssembly("./web.assembly.dll").ToSwaggerDefinition("./swagger.json");
```

Or creating a TypeScript client from a JSON Schema:

```
NSwag.FromJsonSchema("./schema.json").ToTypeScriptClient("./client.ts");
```

The supported sources are:
- .NET assembly (`FromAssembly()`)
- ASP.NET Web API assembly (`FromWebApiAssembly()`)
- JSON Schema (`FromJsonSchema()`)
- Swagger specification (`FromSwaggerSpec()`)

Each source will have its own possible destinations that are covered in the documentation for those source types (see the *Reference* tab above).

## Settings

Some targets allow for including a settings action to fine-tune the code generation process. This is highly specific to each generation target and you will need to check the documentation to confirm the available options.