using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace OpenApiStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Entity> Get()
    {
        return
        [
            new Company
            {
                Name = "Microsoft"
            },
            new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male
            }
        ];
    }
    
    [HttpGet("section")]
    public SectionDto GetSection()
    {
        return new SectionDto();
    }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Person), typeDiscriminator: "Person")]
[JsonDerivedType(typeof(Company), typeDiscriminator: "Company")]
public abstract class Entity
{
    public List<string> Values => [];
}

public class Person : Entity
{
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required Gender Gender { get; init; }
}

public class Company : Entity
{
    public required string Name { get; init; }
}

public enum Gender
{
    Male,
    Female
}


[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TextDto), typeDiscriminator: "text")]
[JsonDerivedType(typeof(TableDto), typeDiscriminator: "table")]
[JsonDerivedType(typeof(SectionDto), typeDiscriminator: "section")]
public abstract class ContentDto;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TableDto), typeDiscriminator: "table")]
[JsonDerivedType(typeof(SectionDto), typeDiscriminator: "section")]
public abstract class ComponentDto : ContentDto;

public class TextDto : ContentDto
{
    public required string Value { get; init; }
}

public class TableDto : ComponentDto
{
    public required string Title { get; init; }

    public IEnumerable<ContentDto> Contents { get; init; } = [];
}

public class SectionDto : ComponentDto
{
    public IEnumerable<ComponentDto> Components { get; init; } = [];
}