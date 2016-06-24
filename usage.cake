#r "dist/lib/Cake.NSwag.dll"

Task("Sample")
.Does(() => {
    CreateDirectory("./dist/sample");
    #break
    NSwag.FromSwaggerSpecification("./sample/swagger.json")
        .ToCSharpClient("./client.cs", "Swagger.Client")
        .ToTypeScriptClient("./client.ts", s => s.WithClassName("Client").WithModuleName("Swagger"));
});

RunTarget("Sample");
