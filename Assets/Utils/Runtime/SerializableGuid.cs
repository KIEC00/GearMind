using System;
using UnityEngine;

namespace Assets.Utils.Runtime
{
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>, ISerializationCallbackReceiver
    {
        public readonly Guid Value => _guid;

        [SerializeField]
        private string _serialized;
        private Guid _guid;

        public SerializableGuid(Guid guid)
        {
            _serialized = null;
            _guid = guid;
        }

        public static SerializableGuid Parse(string serialized) => new(Guid.Parse(serialized));

        public void OnBeforeSerialize()
        {
            if (_guid == Guid.Empty)
                _guid = Guid.NewGuid();
            _serialized = _guid.ToString();
        }

        public void OnAfterDeserialize()
        {
            if (Guid.TryParse(_serialized, out var guid))
            {
                _guid = guid;
                return;
            }

            _guid = Guid.Empty;
            Debug.LogWarning($"Attempted to parse invalid GUID string '{_serialized}.'");
        }

        public override readonly bool Equals(object obj) =>
            obj is SerializableGuid guid && Equals(guid);

        public readonly bool Equals(SerializableGuid other) => _guid.Equals(other._guid);

        public override readonly int GetHashCode() => _guid.GetHashCode() + 9973;

        public override readonly string ToString() => _guid.ToString();

        public static bool operator ==(SerializableGuid left, SerializableGuid right) =>
            left._guid == right._guid;

        public static bool operator !=(SerializableGuid left, SerializableGuid right) =>
            left._guid != right._guid;

        public static implicit operator Guid(SerializableGuid guid) => guid._guid;

        public static implicit operator SerializableGuid(Guid guid) => new(guid);

        public static implicit operator SerializableGuid(string guid) => Parse(guid);

        public static implicit operator string(SerializableGuid guid) => guid.ToString();
    }
}
