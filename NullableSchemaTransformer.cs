using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using OpenApiStuff.Controllers;

public class NullableSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        foreach (var property in schema.Properties)
        {
            var jsonProperty = context.JsonTypeInfo.Properties.FirstOrDefault(x => x.Name == property.Key);
            if (jsonProperty == null) continue;
            
            property.Value.Nullable = jsonProperty.IsGetNullable ||
                                      jsonProperty is { Set: not null, IsSetNullable: true };
            
            property.Value.ReadOnly = jsonProperty is { Get: not null, Set: null };

            if (!property.Value.Nullable)
            {
                schema.Required.Add(property.Key);
            }
        }
        
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