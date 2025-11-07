namespace Assets.GearMind.Storage
{
    public interface IStorage<TKey, TValue>
    {
        TValue Load(TKey key);
        void Save(TKey key, TValue value);
    }
}
