using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow
{
    static class ExtensionMethods
    {
        public static EntityReference ToEntityReference(this Entity entity, string primaryFieldAttribute)
        {
            return new EntityReference(entity.LogicalName, entity.Id) { Name = entity.GetAttributeValue<string>(primaryFieldAttribute) };
        }

        public static string GetFormattedValue(this Entity entity, string columnName)
        {
            return entity.FormattedValues.ContainsKey(columnName) ? entity.FormattedValues[columnName] : null;
        }

        /// <summary>
        /// Retrieve typed value from aliased entity
        /// </summary>
        /// <param name="fieldName">Name of the field to retrieve the value from</param>
        /// <param name="alias">Entity alias</param>
        /// <returns></returns>
        public static T GetAliasedValue<T>(this Entity entity, string fieldName, string alias)
        {
            var aliasedValue = entity.GetAttributeValue<AliasedValue>($"{alias}.{fieldName}");

            if (aliasedValue == null || aliasedValue.Value == null)
                return default(T);

            return (T)aliasedValue.Value;
        }

        public static object GetAliasedValue(this Entity entity, string fieldName, string alias)
        {
            var aliasedValue = entity.GetAttributeValue<AliasedValue>($"{alias}.{fieldName}");

            if (aliasedValue == null || aliasedValue.Value == null)
                return null;

            return aliasedValue.Value;
        }
    }
}
