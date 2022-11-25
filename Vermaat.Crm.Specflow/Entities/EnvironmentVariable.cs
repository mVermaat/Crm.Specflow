using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;

namespace Vermaat.Crm.Specflow.Entities
{
    public class EnvironmentVariable
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public EnvironmentVariableType Type { get; private set; }

        private EnvironmentVariable()
        {

        }

        public void UpdateValue(CrmService svc, string newValue)
        {
            Logger.WriteLine($"Updating Environment Variable {Name} to {newValue}");
            var toUpdate = new Entity("environmentvariablevalue", Id);
            toUpdate["value"] = ParseValue(newValue);
            Value = toUpdate["value"];
            svc.Update(toUpdate);
        }

        private object ParseValue(string newValue)
        {
            switch(Type)
            {
                case EnvironmentVariableType.Boolean:
                    return bool.Parse(newValue);
                case EnvironmentVariableType.Number:
                    return decimal.Parse(newValue);
                default: 
                    return Value;
            }
        }

        public static EnvironmentVariable GetEnvironmentVariable(CrmService svc, string name)
        {
            Logger.WriteLine($"Getting Environment Variable {name}");
            var result = svc.RetrieveMultiple(
                new QueryExpression("environmentvariablevalue")
            {
                NoLock = true,
                ColumnSet = new ColumnSet("value"),
                LinkEntities =
                {
                    new LinkEntity("environmentvariablevalue", "environmentvariabledefinition", "environmentvariabledefinitionid", 
                        "environmentvariabledefinitionid", JoinOperator.Inner)
                    {
                        EntityAlias = "ed",
                        Columns = new ColumnSet("type"),
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("schemaname", ConditionOperator.Equal, name)
                            }
                        }
                    }
                }
            }).Entities;

            if (result == null)
                throw new TestExecutionException(Constants.ErrorCodes.ENVIRONMENT_VARIABLE_NOT_FOUND, name);

            var variable = new EnvironmentVariable()
            {
                Id = result[0].Id,
                Name = name,
                Type = (EnvironmentVariableType)result[0].GetAliasedValue<int>("ed", "type"),
                Value = result[0]["value"]
            };

            Logger.WriteLine($"Found Environment Variable {variable.Id}");

            if (variable.Type == EnvironmentVariableType.Secret || variable.Type == EnvironmentVariableType.DataSource)
                throw new TestExecutionException(Constants.ErrorCodes.ENVIRONMENT_VARIABLE_TYPE_NOT_SUPPORTED, name, variable.Type);

            return variable;
        }


    }
}
