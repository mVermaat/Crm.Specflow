using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Web
{
    class WebFormFiller : IFormFiller
    {
        private readonly Microsoft.Dynamics365.UIAutomation.Api.Entity _entity;
        private readonly Browser _browser;

        public WebFormFiller(Browser browser)
        {
            _entity = browser.Entity;
            _browser = browser;
        }

        public void SetCompositeField(string parentField, IEnumerable<(string fieldName, string fieldValue)> fields)
        {
            var composite = new CompositeControl
            {
                Id = parentField,
                Fields = fields.Select(f => new Field { Id = f.fieldName, Value = f.fieldValue }).ToList()
            };
            _entity.SetValue(composite, true);
            _entity.SwitchToContentFrame();
        }

        public void SetDateTimeField(string fieldName, DateTime fieldValue)
        {
            _entity.SetValue(new DateTimeControl() { Name = fieldName, Value = fieldValue });
        }

        public void SetLookupValue(string fieldName, EntityReference value)
        {
            _entity.SelectLookup(new LookupItem { Name = fieldName });

            var lookup = _browser.Lookup;
            if (!string.IsNullOrEmpty(value.Name))
                lookup.Search(value.Name);

            var index = FindGridItemIndex(value, lookup);

            if (index == null)
                throw new ArgumentException($"Lookup not found. Was looking for Entity: {value.Id} ({value.Name}) of type {value.LogicalName}");

            lookup.SelectItem(index.Value);
            lookup.Add();
            _entity.SwitchToContentFrame();
        }

        

        public void SetOptionSetField(string fieldName, string fieldValue)
        {
            _entity.SetValue(new OptionSet { Name = fieldName, Value = fieldValue });
        }

        public void SetTextField(string fieldName, string fieldValue)
        {
            _entity.SetValueFix(fieldName, fieldValue);
        }

        public void SetTwoOptionField(string fieldName, bool fieldValue)
        {
            // The new method doesn't work yet
#pragma warning disable CS0618 // Type or member is obsolete
            _entity.SetValue(fieldName, fieldValue);
#pragma warning restore CS0618 // Type or member is obsolete
        }


        private int? FindGridItemIndex(EntityReference value, Lookup lookup)
        {
            var gridItems = lookup.GetGridItems();

            for (int i = 0; i < gridItems.Value.Count; i++)
            {
                if (gridItems.Value[i].Id == value.Id)
                    return i;
            }

            var next = lookup.NextPage();

            if (next.Success.GetValueOrDefault())
                return FindGridItemIndex(value, lookup);
            else
                return null;

        }
    }
}
