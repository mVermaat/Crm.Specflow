using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;

namespace Vermaat.Crm.Specflow.Entities
{
    public class EnvironmentVariable
    {
        public Guid? Id { get; private set; }
        public Guid DefintionId { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public EnvironmentVariableType Type { get; private set; }

        private EnvironmentVariable()
        {

        }

        public void UpdateValue(CrmService svc, string newValue)
        {
            Logger.WriteLine($"Updating Environment Variable {Name} to {newValue}");
            var record = new Entity("environmentvariablevalue");
            record["value"] = newValue;
            Value = record["value"];

            if(Id != null && Id != Guid.Empty)
            {
                record.Id = Id.Value;
                svc.Update(record);
            }
            else
            {
                record["environmentvariabledefinitionid"] = new EntityReference("environmentvariabledefinition", DefintionId);
                svc.CreateRecord(record);
            }
        } 

        public static EnvironmentVariable GetEnvironmentVariable(CrmService svc, string name)
        {
            Logger.WriteLine($"Getting Environment Variable {name}");
            var result = svc.RetrieveMultiple(
                new QueryExpression("environmentvariabledefinition")
                {
                    NoLock = true,
                    ColumnSet = new ColumnSet("type"),
                    Criteria =
                    {
                        Conditions =
                            {
                                new ConditionExpression("schemaname", ConditionOperator.Equal, name)
                            }
                    },
                    LinkEntities =
                    {
                        new LinkEntity("environmentvariabledefinition", "environmentvariablevalue", "environmentvariabledefinitionid",
                            "environmentvariabledefinitionid", JoinOperator.LeftOuter)
                        {
                            EntityAlias = "ev",
                            Columns = new ColumnSet("environmentvariablevalueid", "value"),
                        }
                    }
                }).Entities;

            if (result == null || result.Count == 0)
                throw new TestExecutionException(Constants.ErrorCodes.ENVIRONMENT_VARIABLE_NOT_FOUND, name);

            var variable = new EnvironmentVariable()
            {
                DefintionId = result[0].Id,
                Id = result[0].GetAliasedValue<Guid?>("ev", "environmentvariablevalueid"),
                Name = name,
                Type = (EnvironmentVariableType)result[0].GetAttributeValue<OptionSetValue>("type").Value,
                Value = result[0].GetAliasedValue("ev", "value"),
            };

            Logger.WriteLine($"Found Environment Variable {variable.Id}");

            if (variable.Type == EnvironmentVariableType.Secret || variable.Type == EnvironmentVariableType.DataSource)
                throw new TestExecutionException(Constants.ErrorCodes.ENVIRONMENT_VARIABLE_TYPE_NOT_SUPPORTED, name, variable.Type);

            return variable;
        }


    }
}
