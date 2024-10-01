using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public class NullableSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        
        var jsonPolymorphicAttribute = FindJsonPolymorphicAttribute(type);
        if (jsonPolymorphicAttribute?.TypeDiscriminatorPropertyName != null)
        {
            schema.Required.Add(jsonPolymorphicAttribute.TypeDiscriminatorPropertyName);
        }

        return Task.CompletedTask;
    }

    private static JsonPolymorphicAttribute? FindJsonPolymorphicAttribute(Type type)
    {
        var currentType = type;

        while (currentType.BaseType != null)
        {
            var attribute = currentType.GetCustomAttributes<JsonPolymorphicAttribute>(false).FirstOrDefault();
            if (attribute != null) return attribute;
            
            currentType = currentType.BaseType;
        }

        return null;
    }
}