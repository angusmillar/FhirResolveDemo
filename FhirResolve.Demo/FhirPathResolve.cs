using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
namespace FhirResolve.Demo;

public class FhirPathResolve
{

  //Please note this implementation hard codes the resource name "Patient" and is only for this demo.
  //The real implementation would dynamically resolve the Resource Type and resourceId from the url provided 
  public ITypedElement? Resolver(string url)
  {
    string[] split = url.Split('/');
    string resourceId = split.Last();
    string resourceName = "Patient";
    
    if (url.Contains(resourceName))
    {
      var defaultModelFactory = new Hl7.Fhir.Serialization.DefaultModelFactory();
      Type? type = ModelInfo.GetTypeForFhirType(resourceName);
      if (type is null)
      {
        throw new ApplicationException($"ResourceName of '{resourceName}' can not be converted to a FHIR Type.");
      }
      if (defaultModelFactory.Create(type) is DomainResource domainResource)
      {
        domainResource.Id = resourceId;
        return domainResource.ToTypedElement().ToScopedNode();
      }
      throw new ApplicationException($"Unable to create a domain resource of type '{type.Name}'.");
    }
    return null;
  }
}
