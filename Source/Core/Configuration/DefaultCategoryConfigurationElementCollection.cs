namespace Kigg.Configuration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;

    [ConfigurationCollection(typeof(DefaultUserConfigurationElement))]
    public class DefaultCategoryConfigurationElementCollection : ConfigurationElementCollection, IEnumerable<DefaultCategoryConfigurationElement>
    {
        public new DefaultCategoryConfigurationElement this[string source]
        {
            [DebuggerStepThrough]
            get
            {
                return BaseGet(source) as DefaultCategoryConfigurationElement;
            }
        }

        [DebuggerStepThrough]
        public void Add(DefaultCategoryConfigurationElement element)
        {
            BaseAdd(element);
        }

        [DebuggerStepThrough]
        IEnumerator<DefaultCategoryConfigurationElement> IEnumerable<DefaultCategoryConfigurationElement>.GetEnumerator()
        {
            IEnumerator iterator = GetEnumerator();

            while (iterator.MoveNext())
            {
                yield return iterator.Current as DefaultCategoryConfigurationElement;
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
            return new DefaultCategoryConfigurationElement();
        }

        [DebuggerStepThrough]
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DefaultCategoryConfigurationElement)element).Name;
        }
    }
}