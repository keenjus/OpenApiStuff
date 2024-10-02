using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace OpenApiStuff.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("component")]
    public ComponentDto GetComponent()
    {
        return new SectionDto();
    }
    
    [HttpGet("section")]
    public SectionDto GetSection()
    {
        return new SectionDto();
    }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SectionDto), typeDiscriminator: "section")]
public abstract class ComponentDto;

public class SectionDto : ComponentDto
{
    public IEnumerable<ComponentDto> Components { get; init; } = [];
}