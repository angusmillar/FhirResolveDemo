using FhirResolve.Demo;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.FhirPath;
using Hl7.FhirPath;

Console.WriteLine("FHIRPath Resolve() demo");

//Create a very simple Observation resource with a Observation.subject reference to a Patient resource
Observation resource = new Observation();
resource.Id = "Obs1";
resource.Status = ObservationStatus.Final;
resource.Code = new CodeableConcept("http://loinc.org", "15074-8", "Glucose [Moles/volume] in Blood");
resource.Value = new Quantity(6.3m, "mmol/l", "http://unitsofmeasure.org");
//Note the setting of the subject reference to reference a Patent resource with an Id of 1 
resource.Subject = new ResourceReference("Patient/1");

//If I use this non-Resolve() fhirpath expression then the demo works as expected
//string expression = "Observation.subject";

//However, this expression that utilised Resolve() was failing to match on the Observation resource, however this code below now works.
string expression = "Observation.subject.where(resolve() is Patient)";

try
{

  ElementNavFhirExtensions.PrepareFhirSymbolTableFunctions();
  
  ScopedNode resourceModel = new ScopedNode(resource.ToTypedElement());
  
  FhirPathResolve fhirPathResolve = new FhirPathResolve();

  IEnumerable<ITypedElement> typedElementList = resourceModel.Select(expression, new FhirEvaluationContext(resourceModel) { ElementResolver = fhirPathResolve.Resolver });

  bool isMatchFound = false;
  foreach (ITypedElement typedElement in typedElementList)
  {
    isMatchFound = true;
    if (typedElement is ScopedNode scopedNode)
    {
      if (scopedNode.Current is IFhirValueProvider fhirValueProvider1)
      {
        Console.WriteLine($"IFhirValueProvider FhirValue Type:  {fhirValueProvider1.FhirValue.GetType().Name}"); 
        if (fhirValueProvider1.FhirValue is ResourceReference resourceReference)
        {
          Console.WriteLine($"Resource Reference Uri found : {resourceReference.Url.OriginalString}");
        }
      }
    }
  }
  Console.WriteLine($"FhirPath expression match found : {isMatchFound}");
}
catch (Exception exception)
{
  Console.WriteLine(exception.Message);
}
