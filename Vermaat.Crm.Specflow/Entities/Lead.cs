using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Entities
{
    public class Lead
    {
        private readonly Entity _lead;

        public Lead(Entity leadEntity)
        {
            _lead = leadEntity;
        }

        public Guid Id => _lead.Id;
        public EntityReference Currency => _lead.GetAttributeValue<EntityReference>(Fields.TransactionCurrencyId);
        public EntityReference Customer => _lead.GetAttributeValue<EntityReference>(Fields.CustomerId);
        public EntityReference Campaign => _lead.GetAttributeValue<EntityReference>(Fields.CampaignId);

        public QualifyLeadRequest CreateQualifyLeadRequest(bool createAccount, bool createContact, bool createOpportunity)
        {
            var req = new QualifyLeadRequest()
            {
                CreateAccount = createAccount,
                CreateContact = createContact,
                CreateOpportunity = createOpportunity,
                LeadId = _lead.ToEntityReference(),
                OpportunityCurrencyId = Currency,
                OpportunityCustomerId = Customer,
                SourceCampaignId = Campaign,
                Status = new OptionSetValue((int)Lead_StatusCode.Qualified)
            };
            req.Parameters.Add("SuppressDuplicateDetection", true);
            return req;
        }

        public static class Fields
        {
            public const string AccountId = "accountid";
            public const string Address1_AddressId = "address1_addressid";
            public const string Address1_AddressTypeCode = "address1_addresstypecode";
            public const string Address1_City = "address1_city";
            public const string Address1_Composite = "address1_composite";
            public const string Address1_Country = "address1_country";
            public const string Address1_County = "address1_county";
            public const string Address1_Fax = "address1_fax";
            public const string Address1_Latitude = "address1_latitude";
            public const string Address1_Line1 = "address1_line1";
            public const string Address1_Line2 = "address1_line2";
            public const string Address1_Line3 = "address1_line3";
            public const string Address1_Longitude = "address1_longitude";
            public const string Address1_Name = "address1_name";
            public const string Address1_PostalCode = "address1_postalcode";
            public const string Address1_PostOfficeBox = "address1_postofficebox";
            public const string Address1_ShippingMethodCode = "address1_shippingmethodcode";
            public const string Address1_StateOrProvince = "address1_stateorprovince";
            public const string Address1_Telephone1 = "address1_telephone1";
            public const string Address1_Telephone2 = "address1_telephone2";
            public const string Address1_Telephone3 = "address1_telephone3";
            public const string Address1_UPSZone = "address1_upszone";
            public const string Address1_UTCOffset = "address1_utcoffset";
            public const string Address2_AddressId = "address2_addressid";
            public const string Address2_AddressTypeCode = "address2_addresstypecode";
            public const string Address2_City = "address2_city";
            public const string Address2_Composite = "address2_composite";
            public const string Address2_Country = "address2_country";
            public const string Address2_County = "address2_county";
            public const string Address2_Fax = "address2_fax";
            public const string Address2_Latitude = "address2_latitude";
            public const string Address2_Line1 = "address2_line1";
            public const string Address2_Line2 = "address2_line2";
            public const string Address2_Line3 = "address2_line3";
            public const string Address2_Longitude = "address2_longitude";
            public const string Address2_Name = "address2_name";
            public const string Address2_PostalCode = "address2_postalcode";
            public const string Address2_PostOfficeBox = "address2_postofficebox";
            public const string Address2_ShippingMethodCode = "address2_shippingmethodcode";
            public const string Address2_StateOrProvince = "address2_stateorprovince";
            public const string Address2_Telephone1 = "address2_telephone1";
            public const string Address2_Telephone2 = "address2_telephone2";
            public const string Address2_Telephone3 = "address2_telephone3";
            public const string Address2_UPSZone = "address2_upszone";
            public const string Address2_UTCOffset = "address2_utcoffset";
            public const string BudgetAmount = "budgetamount";
            public const string BudgetAmount_Base = "budgetamount_base";
            public const string BudgetStatus = "budgetstatus";
            public const string CampaignId = "campaignid";
            public const string CompanyName = "companyname";
            public const string ConfirmInterest = "confirminterest";
            public const string ContactId = "contactid";
            public const string CreatedBy = "createdby";
            public const string CreatedOn = "createdon";
            public const string CreatedOnBehalfBy = "createdonbehalfby";
            public const string CustomerId = "customerid";
            public const string DecisionMaker = "decisionmaker";
            public const string Description = "description";
            public const string DoNotBulkEMail = "donotbulkemail";
            public const string DoNotEMail = "donotemail";
            public const string DoNotFax = "donotfax";
            public const string DoNotPhone = "donotphone";
            public const string DoNotPostalMail = "donotpostalmail";
            public const string DoNotSendMM = "donotsendmm";
            public const string EMailAddress1 = "emailaddress1";
            public const string EMailAddress2 = "emailaddress2";
            public const string EMailAddress3 = "emailaddress3";
            public const string EntityImage = "entityimage";
            public const string EntityImage_Timestamp = "entityimage_timestamp";
            public const string EntityImage_URL = "entityimage_url";
            public const string EntityImageId = "entityimageid";
            public const string EstimatedAmount = "estimatedamount";
            public const string EstimatedAmount_Base = "estimatedamount_base";
            public const string EstimatedCloseDate = "estimatedclosedate";
            public const string EstimatedValue = "estimatedvalue";
            public const string EvaluateFit = "evaluatefit";
            public const string ExchangeRate = "exchangerate";
            public const string Fax = "fax";
            public const string FirstName = "firstname";
            public const string FollowEmail = "followemail";
            public const string FullName = "fullname";
            public const string ImportSequenceNumber = "importsequencenumber";
            public const string IndustryCode = "industrycode";
            public const string InitialCommunication = "initialcommunication";
            public const string JobTitle = "jobtitle";
            public const string LastName = "lastname";
            public const string LastOnHoldTime = "lastonholdtime";
            public const string LastUsedInCampaign = "lastusedincampaign";
            public const string LeadId = "leadid";
            public const string Id = "leadid";
            public const string LeadQualityCode = "leadqualitycode";
            public const string LeadSourceCode = "leadsourcecode";
            public const string MasterId = "masterid";
            public const string Merged = "merged";
            public const string MiddleName = "middlename";
            public const string MobilePhone = "mobilephone";
            public const string ModifiedBy = "modifiedby";
            public const string ModifiedOn = "modifiedon";
            public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
            public const string msdyn_gdproptout = "msdyn_gdproptout";
            public const string Need = "need";
            public const string NumberOfEmployees = "numberofemployees";
            public const string OnHoldTime = "onholdtime";
            public const string OriginatingCaseId = "originatingcaseid";
            public const string OverriddenCreatedOn = "overriddencreatedon";
            public const string OwnerId = "ownerid";
            public const string OwningBusinessUnit = "owningbusinessunit";
            public const string OwningTeam = "owningteam";
            public const string OwningUser = "owninguser";
            public const string Pager = "pager";
            public const string ParentAccountId = "parentaccountid";
            public const string ParentContactId = "parentcontactid";
            public const string ParticipatesInWorkflow = "participatesinworkflow";
            public const string PreferredContactMethodCode = "preferredcontactmethodcode";
            public const string PriorityCode = "prioritycode";
            public const string ProcessId = "processid";
            public const string PurchaseProcess = "purchaseprocess";
            public const string PurchaseTimeFrame = "purchasetimeframe";
            public const string QualificationComments = "qualificationcomments";
            public const string QualifyingOpportunityId = "qualifyingopportunityid";
            public const string RelatedObjectId = "relatedobjectid";
            public const string Revenue = "revenue";
            public const string Revenue_Base = "revenue_base";
            public const string SalesStage = "salesstage";
            public const string SalesStageCode = "salesstagecode";
            public const string Salutation = "salutation";
            public const string ScheduleFollowUp_Prospect = "schedulefollowup_prospect";
            public const string ScheduleFollowUp_Qualify = "schedulefollowup_qualify";
            public const string SIC = "sic";
            public const string SLAId = "slaid";
            public const string SLAInvokedId = "slainvokedid";
            public const string StageId = "stageid";
            public const string StateCode = "statecode";
            public const string StatusCode = "statuscode";
            public const string Subject = "subject";
            public const string TeamsFollowed = "teamsfollowed";
            public const string Telephone1 = "telephone1";
            public const string Telephone2 = "telephone2";
            public const string Telephone3 = "telephone3";
            public const string TimeSpentByMeOnEmailAndMeetings = "timespentbymeonemailandmeetings";
            public const string TimeZoneRuleVersionNumber = "timezoneruleversionnumber";
            public const string TransactionCurrencyId = "transactioncurrencyid";
            public const string TraversedPath = "traversedpath";
            public const string UTCConversionTimeZoneCode = "utcconversiontimezonecode";
            public const string VersionNumber = "versionnumber";
            public const string WebSiteUrl = "websiteurl";
            public const string YomiCompanyName = "yomicompanyname";
            public const string YomiFirstName = "yomifirstname";
            public const string YomiFullName = "yomifullname";
            public const string YomiLastName = "yomilastname";
            public const string YomiMiddleName = "yomimiddlename";
            public const string Referencinglead_master_lead = "lead_master_lead";
        }

    }

    
    public enum Lead_StatusCode
    {
        Canceled = 7,
        CannotContact = 5,
        Contacted = 2,
        Lost = 4,
        New = 1,
        NoLongerInterested = 6,
        Qualified = 3,
    }

}
