using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

public class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (!type.IsEnum) return Task.CompletedTask;
        
        var values = Enum.GetValues(type);

        foreach (var value in values)
        {
            schema.Enum.Add(new OpenApiString(value.ToString()));
        }

        schema.Type = "string";

        return Task.CompletedTask;
    }
}