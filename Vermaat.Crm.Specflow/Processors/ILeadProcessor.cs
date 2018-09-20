using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Processors
{
    public interface ILeadProcessor
    {
        EntityReferenceCollection Qualify(EntityReference leadReference, bool createAccount, bool createContact, bool createOpportunity);
    }
}