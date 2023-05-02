using FhirResolve.Demo;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.FhirPath;
using Hl7.FhirPath;
using Hl7.FhirPath.Expressions;

Console.WriteLine("FHIRPath Resolve() demo");

//Create a very simple Observation resource with a Observation.subject reference to a Patient resource
Observation Obs = new Observation();
Obs.Status = ObservationStatus.Final;
Obs.Code = new CodeableConcept("http://loinc.org", "15074-8", "Glucose [Moles/volume] in Blood");
Obs.Value = new Quantity(6.3m, "mmol/l", "http://unitsofmeasure.org");
//Note the setting of the subject reference to reference a Patent resource with an Id of 1 
Obs.Subject = new ResourceReference("Patient/1");

//If I use this non-Resolve() fhirpath expression then the demo works as expected
//string fhirPathExpression = "Observation.subject";

//However, this expression that utilised Resolve() fails to match on the Observation resource
string fhirPathExpression = "Observation.subject.where(resolve() is Patient)";

try
{ 
  var symbolTable = new SymbolTable()
                    .AddStandardFP()
                    .AddFhirExtensions();
  var newCompiler = new FhirPathCompiler(symbolTable);
  
  //Add my custom Resolve() delegate to the FhirEvaluationContext
  var oFhirEvaluationContext = new FhirEvaluationContext();
  FhirPathResolve fhirPathResolve = new FhirPathResolve();
  oFhirEvaluationContext.ElementResolver = fhirPathResolve.Resolver;
  
  CompiledExpression compiledExpression = newCompiler.Compile(fhirPathExpression);

  IEnumerable<ITypedElement> typedElementList = compiledExpression.Invoke(Obs.ToTypedElement(), oFhirEvaluationContext);

  bool isMatchFound = false;
  foreach (ITypedElement typedElement in typedElementList)
  {
    isMatchFound = true;
    if (typedElement is IFhirValueProvider fhirValueProvider && fhirValueProvider.FhirValue != null)
    {
      if (fhirValueProvider.FhirValue is ResourceReference resourceReference)
      {
        Console.WriteLine($"Resource Reference Uri found : {resourceReference.Url}");
      }
    }
  }
  Console.WriteLine($"FhirPath expression match found : {isMatchFound}");
  
}
catch (Exception exception)
{
  Console.WriteLine(exception.Message);
}

