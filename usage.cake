#r "dist/lib/Cake.NSwag.dll"

Task("Sample")
.Does(() => {
    CreateDirectory("./dist/sample");
    #break
    NSwag.FromSwaggerSpecification("./samples/swagger.json")
        .ToCSharpClient("./client.cs", "Swagger.Client")
        .ToTypeScriptClient("./client.ts", s => s.WithClassName("Client").WithModuleName("Swagger"));
});

Task("Full-Settings")
.Does(() => {
    NSwag.FromSwaggerSpecification("./samples/swagger.json")
    .ToTypeScriptClient("./client.ts", s =>
        s.WithClassName("ApiClient")
            .WithModuleName("SwaggerApi")
            .WithSettings(new SwaggerToTypeScriptClientGeneratorSettings
            {
                PromiseType = PromiseType.Promise
            }));
});

RunTarget("Sample");

