using System.Collections.Generic;

namespace Assets.GearMind.Storage.Endpoints
{
    public class LevelProgressEndpoint : StorageEndpoint<string, PassedLevelsIdsSet, string>
    {
        public override PassedLevelsIdsSet DefaultValue => new();

        public LevelProgressEndpoint(
            string key,
            IStorage<string, string> storage,
            ISerializer<string> serializer
        )
            : base(key, storage, serializer) { }
    }

    public class PassedLevelsIdsSet : HashSet<int> { }
}
