using System;
using System.Collections.Concurrent;

namespace MausWorks.MongoDB.Extensions
{
    /// <summary>
    /// Contains <see cref="Type"/>-extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Contains default values of value types.
        /// </summary>
        public static readonly ConcurrentDictionary<Type, object> ValueTypeCache = new ConcurrentDictionary<Type, object>();
        
        /// <summary>
        /// Returns a default value of a provided type.
        /// </summary>
        /// <param name="type">The type to use to get the default value.</param>
        /// <returns>The default value of the <see cref="Type"/></returns>
        public static object GetDefaultValue(this Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }

            return type.IsValueType 
                ? ValueTypeCache.GetOrAdd(type, t => Activator.CreateInstance(t)) 
                : null;
        }
    }
}