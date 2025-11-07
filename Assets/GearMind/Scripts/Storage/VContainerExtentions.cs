using VContainer;

namespace Assets.GearMind.Storage
{
    public static class VContainerExtentions
    {
        public static RegistrationBuilder RegisterEndpoint<TEndpoint, TKey>(
            this IContainerBuilder builder,
            TKey key
        ) => builder.Register<TEndpoint>(Lifetime.Singleton).WithParameter(key).AsSelf();
    }
}
