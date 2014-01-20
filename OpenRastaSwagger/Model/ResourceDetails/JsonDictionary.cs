using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    [Serializable]
    [KnownType(typeof(PropertyType))]
    [KnownType(typeof(ModelSpec))]
    public class JsonDictionary<TKey, TValue> : ISerializable
    {
        private readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

        public JsonDictionary() { }

        protected JsonDictionary(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var key in _dict.Keys)
            {
                info.AddValue(key.ToString(), _dict[key]);
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dict.Add(key, value);
        }

        public TValue this[TKey index]
        {
            set { _dict[index] = value; }
            get { return _dict[index]; }
        }
    }
}