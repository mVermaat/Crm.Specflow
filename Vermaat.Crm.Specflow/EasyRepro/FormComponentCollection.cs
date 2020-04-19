using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormComponentCollection<T> : IEnumerable<T>
        where T : FormComponent
    {
        protected Dictionary<string, T> _componentsByName;
        protected Dictionary<string, List<T>> _componentsByLabel;

        public FormComponentCollection()
        {
            _componentsByName = new Dictionary<string, T>();
            _componentsByLabel = new Dictionary<string, List<T>>();
        }

        public void Add(T component)
        {
            _componentsByName.Add(component.Name.ToLower(), component);

            if (!_componentsByLabel.ContainsKey(component.Label))
                _componentsByLabel.Add(component.Label, new List<T>());

            _componentsByLabel[component.Label].Add(component);
        }

        public T FindByName(string value)
        {
            return _componentsByName.ContainsKey(value) ? _componentsByName[value] : null;
        }

        public bool ContainsByName(string value)
        {
            return _componentsByName.ContainsKey(value);
        }

        public bool Contains(string value, bool byName = true, bool byLabel = true)
        {
            return (byName && _componentsByName.ContainsKey(value)) ||
                (byLabel && _componentsByLabel.ContainsKey(value));
        }

        public IEnumerable<T> Find(string value, bool byName = true, bool byLabel = true)
        {
            var results = new List<T>();

            if (byName && _componentsByName.ContainsKey(value.ToLower()))
                results.Add(_componentsByName[value.ToLower()]);

            if (byLabel && _componentsByLabel.ContainsKey(value))
                results.AddRange(_componentsByLabel[value]);

            return results;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _componentsByName
                .Select(x => x.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _componentsByName
                .Select(x => x.Value)
                .GetEnumerator();
        }
    }
}
