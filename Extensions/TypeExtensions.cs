using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MausWorks.MongoDB.Extensions
{
    public static class TypeExtensions
    {
        public static readonly ConcurrentDictionary<Type, object> TypeCache = new ConcurrentDictionary<Type, object>();
        
        public static object GetDefaultValue(this Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }

            return type.IsValueType 
                ? TypeCache.GetOrAdd(type, t => Activator.CreateInstance(t)) 
                : null;
        }
    }
}