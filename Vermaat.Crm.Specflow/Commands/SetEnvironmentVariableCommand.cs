using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    internal class SetEnvironmentVariableCommand : ApiOnlyCommand
    {
        private readonly string _name;
        private readonly string _value;

        public SetEnvironmentVariableCommand(CrmTestingContext crmContext, string name, string value) : base(crmContext)
        {
            _name = name;
            _value = value;
        }

        public override void Execute()
        {
            var envVar = EnvironmentVariable.GetEnvironmentVariable(GlobalTestingContext.ConnectionManager.AdminConnection, _name);
            envVar.UpdateValue(GlobalTestingContext.ConnectionManager.AdminConnection, _value);
        }
    }
}
