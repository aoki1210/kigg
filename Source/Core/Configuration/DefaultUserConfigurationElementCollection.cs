namespace Kigg.Configuration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;

    [ConfigurationCollection(typeof(DefaultUserConfigurationElement))]
    public class DefaultUserConfigurationElementCollection : ConfigurationElementCollection, IEnumerable<DefaultUserConfigurationElement>
    {
        public new DefaultUserConfigurationElement this[string source]
        {
            [DebuggerStepThrough]
            get
            {
                return BaseGet(source) as DefaultUserConfigurationElement;
            }
        }

        [DebuggerStepThrough]
        public void Add(DefaultUserConfigurationElement element)
        {
            BaseAdd(element);
        }

        [DebuggerStepThrough]
        IEnumerator<DefaultUserConfigurationElement> IEnumerable<DefaultUserConfigurationElement>.GetEnumerator()
        {
            IEnumerator iterator = GetEnumerator();

            while (iterator.MoveNext())
            {
                yield return iterator.Current as DefaultUserConfigurationElement;
            }
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [DebuggerStepThrough]
        protected override ConfigurationElement CreateNewElement()
        {
            return new DefaultUserConfigurationElement();
        }

        [DebuggerStepThrough]
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DefaultUserConfigurationElement)element).UserName;
        }
    }
}