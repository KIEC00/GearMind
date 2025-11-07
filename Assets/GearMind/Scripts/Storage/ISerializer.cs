namespace Assets.GearMind.Storage
{
    public interface ISerializer<TSerialized>
    {
        TSerialized Serialize<T>(T data);
        T Deserialize<T>(TSerialized data);
    }
}
