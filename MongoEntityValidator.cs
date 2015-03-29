using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MausWorks.MongoDB
{
    public class MongoEntityValidator
    {
        public IEnumerable<KeyValuePair<string, object>> PropertyValues { get; private set; }

        public MongoEntityValidator(object model)
        {
            var modelType = model.GetType();

            if (modelType.IsValueType || modelType.IsAbstract)
            {
                return;
            }


        }

        public bool Validates()
        {
            return !PropertyValues.Any(pvs => IsDefaultOrNull(pvs.Value));
        }

        public bool IsDefaultOrNull(object obj)
        {
            var t = obj.GetType();

            if (t.IsValueType)
            {
                return true; // <-- Here.
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