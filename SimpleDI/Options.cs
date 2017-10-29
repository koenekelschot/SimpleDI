using System;
using System.Reflection;

namespace SimpleDI
{
    public class Options<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        protected readonly TOptions _value;

        protected internal Options(TOptions value) {
            _value = value;
        }

        public TOptions Value => _value;
        public dynamic this[string key] => GetValue(key);

        private dynamic GetValue(string key)
        {
            TypeInfo valueTypeInfo = typeof(TOptions).GetTypeInfo();
            
            foreach (PropertyInfo property in valueTypeInfo.DeclaredProperties)
            {
                if (property.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return property.GetValue(_value);
                }
            }

            return null;
        }
    }
}
