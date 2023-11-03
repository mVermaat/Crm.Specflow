using Microsoft.Xrm.Sdk.Metadata;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public abstract class FormField : Field
    {

        protected FormControl Control { get; private set; }

        public FormField(UCIApp app, AttributeMetadata attributeMetadata, FormControl control)
            : base(app, attributeMetadata)
        {
            Control = control;
        }

        public virtual RequiredState GetRequiredState(FormState formState)
        {
            return SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateCheckFieldRequiredStateCommand(LogicalName));
        }

        public virtual bool IsVisible(FormState formState)
        {
            return SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateCheckFieldVisibilityCommand(Control.ControlName));
        }

        public virtual bool IsLocked(FormState formState)
        {
            return SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateCheckFieldLockedStateCommand(Control.ControlName));
        }
    }
}
