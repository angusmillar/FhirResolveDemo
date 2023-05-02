using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Specification;
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace FhirResolve.Demo;

public class CustomResolveTypedElement : ITypedElement
{
  public string Name { get; set; }

  public string InstanceType { get; set; }

  public object Value { get; set; }

  public string Location { get; set; }

  public IElementDefinitionSummary Definition { get; set; }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
  public IEnumerable<ITypedElement> Children(string name = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
  {
    throw new NotImplementedException();
  }
}
