using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OpenRastaSwagger.Model.ResourceDetails;

namespace OpenRastaSwagger.ContractJsonGeneration.Contracts
{
    [Serializable]
    [KnownType(typeof(PropertyType))]
    [KnownType(typeof(ModelSpec))]
    [KnownType(typeof(Operation))]
    [KnownType(typeof(HttpHeader))]
    [KnownType(typeof(Parameter))]
    [KnownType(typeof(Time))]
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