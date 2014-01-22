public class AssemblyRefs
{
// HACK: this is done intentionally to force the Azure Tools to include the assembly within the package (they are discovered and inclyded dynamically)
    private string ref1 = System.Data.Entity.SqlServer.SqlProviderServices.Instance.GetType().ToString();    

    private AssemblyRefs()
    {   
    }
}