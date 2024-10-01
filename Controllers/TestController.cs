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
