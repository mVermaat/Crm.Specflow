﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Interfaces
{
    public interface ICrmService
    {
        
        void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);
        void Create(Entity entity, string alias, AliasedRecordCache recordCache);
        void Delete(string entityName, Guid id);
        void Delete(EntityReference entityReference);
        void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);
        T Execute<T>(OrganizationRequest request) where T : OrganizationResponse;
        Entity Retrieve(string entityName, Guid id, ColumnSet columnSet);
        Entity Retrieve(EntityReference entityReference, ColumnSet columnSet);
        EntityCollection RetrieveMultiple(QueryBase query);
        void Update(Entity entity);
    }
}
