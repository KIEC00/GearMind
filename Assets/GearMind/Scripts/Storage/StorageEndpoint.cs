using System;

namespace Assets.GearMind.Storage
{
    public class StorageEndpoint<TKey, TValue, TSerialized>
        : StorageEndpoint<TKey, TValue, TValue, TSerialized>
        where TValue : class
        where TSerialized : class
    {
        public StorageEndpoint(
            TKey key,
            IStorage<TKey, TSerialized> storage,
            ISerializer<TSerialized> serializer
        )
            : base(key, storage, serializer) { }

        protected override TValue Encode(TValue data) => data;

        protected override TValue Decode(TValue data) => data;
    }

    public abstract class StorageEndpoint<TKey, TValue, TEncode, TSerialized>
        where TValue : class
        where TEncode : class
        where TSerialized : class
    {
        private readonly TKey _key;
        private readonly IStorage<TKey, TSerialized> _storage;
        private readonly ISerializer<TSerialized> _serializer;

        public virtual TValue DefaultValue => default;

        public StorageEndpoint(
            TKey key,
            IStorage<TKey, TSerialized> storage,
            ISerializer<TSerialized> serializer
        )
        {
            _key = key;
            _storage = storage;
            _serializer = serializer;
        }

        public void Save(TValue data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));
            var encoded = Encode(data);
            var serialized = Serialize(encoded);
            _storage.Save(_key, serialized);
        }

        public TValue Load()
        {
            var serialized = _storage.Load(_key);
            if (serialized == null)
                return DefaultValue;
            var encoded = Deserialize(serialized);
            return Decode(encoded);
        }

        protected abstract TEncode Encode(TValue data);

        protected abstract TValue Decode(TEncode data);

        protected TSerialized Serialize(TEncode data) => _serializer.Serialize(data);

        protected TEncode Deserialize(TSerialized data) => _serializer.Deserialize<TEncode>(data);
    }
}
