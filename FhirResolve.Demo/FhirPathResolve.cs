using Hl7.Fhir.ElementModel;
namespace FhirResolve.Demo;

public class FhirPathResolve
{
  public ITypedElement? Resolver(string url)
  {
    CustomResolveTypedElement resolveElementNavigator = new CustomResolveTypedElement();

    //Please note that this Resolve() implementation is not fit for purpose
    //It is only used for this demo's purpose returning an ITypedElement where the url string contains the string 'Patient' 
    string resourceName = "Patient";
    if (url.Contains(resourceName))
    {
      resolveElementNavigator.Name = resourceName;
      resolveElementNavigator.InstanceType = resourceName;
      resolveElementNavigator.Value = resourceName;
      resolveElementNavigator.Location = url;
      return resolveElementNavigator.ToScopedNode();
    }


#pragma warning disable CS8603 // Possible null reference return.
    return null;
#pragma warning restore CS8603 // Possible null reference return.
  }
}
