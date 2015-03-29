using MausWorks.MongoDB.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MausWorks.MongoDB
{
    public class MongoEntityValidator
    {
        public IEnumerable<KeyValuePair<string, object>> RequiredProperties { get; private set; }

        public MongoEntityValidator(object model)
        {
            var modelType = model.GetType();

            if (modelType.IsValueType || modelType.IsAbstract)
            {
                return;
            }

<<<<<<< HEAD
            RequiredProperties = GetPropertyValues(modelType, model);
=======
            PropertyValues = GetPropertyValues(modelType, model);
>>>>>>> origin/master
        }

        public bool Validates()
        {
            return !RequiredProperties.Any(pvs => IsDefaultOrNull(pvs.Value));
        }

        public IEnumerable<string> GetInvalidPropertyNames()
        {
            return RequiredProperties.Where(kvp => IsDefaultOrNull(kvp.Value)).Select(k => k.Key); 
        }

        public bool IsDefaultOrNull(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            var t = obj.GetType();

            if (t.IsValueType)
            {
                return obj == t.GetDefaultValue();
            }

            return obj == null;
        }

        public IEnumerable<KeyValuePair<string, object>> GetPropertyValues(Type type, object obj)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var pt = property.PropertyType;

                if (pt.IsValueType || pt.IsPrimitive || pt == typeof(string))
                {
                    var bsonReqAttr = property.GetCustomAttribute<BsonRequiredAttribute>();

                    if (bsonReqAttr == null)
                    {
                        continue;
                    }

                    yield return new KeyValuePair<string, object>(property.Name, property.GetValue(obj));
                }

                var subObj = property.GetValue(obj);

                if (subObj != null)
                {
                    foreach (var kvp in GetPropertyValues(pt, subObj))
                    {
                        yield return kvp;
                    }
                }
            }
        }
    }
}
