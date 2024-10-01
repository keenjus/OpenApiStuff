using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.AllowOutOfOrderMetadataProperties = true;
    x.JsonSerializerOptions.TypeInfoResolver = new InheritedPolymorphismResolver();
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Microsoft.AspNetCore.OpenApi.OpenApiSchemaService
    options.AddSchemaTransformer<EnumSchemaTransformer>();
    options.AddSchemaTransformer<NullableSchemaTransformer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// A custom JsonTypeInfoResolver to always include the type discriminator
/// https://github.com/dotnet/runtime/issues/77532
/// </summary>
public class InheritedPolymorphismResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var typeInfo = base.GetTypeInfo(type, options);

        // Only handles class hierarchies -- interface hierarchies left out intentionally here
        if (type.IsSealed || typeInfo.PolymorphismOptions is not null || type.BaseType == null) return typeInfo;

        // recursively resolve metadata for the base type and extract any derived type declarations that overlap with the current type
        var basePolymorphismOptions = GetTypeInfo(type.BaseType, options).PolymorphismOptions;
        if (basePolymorphismOptions == null) return typeInfo;

        foreach (var derivedType in basePolymorphismOptions.DerivedTypes)
        {
            if (!type.IsAssignableFrom(derivedType.DerivedType)) continue;

            typeInfo.PolymorphismOptions ??= new JsonPolymorphismOptions
            {
                IgnoreUnrecognizedTypeDiscriminators = basePolymorphismOptions.IgnoreUnrecognizedTypeDiscriminators,
                TypeDiscriminatorPropertyName = basePolymorphismOptions.TypeDiscriminatorPropertyName,
                UnknownDerivedTypeHandling = basePolymorphismOptions.UnknownDerivedTypeHandling,
            };

            typeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
        }

        return typeInfo;
    }
}