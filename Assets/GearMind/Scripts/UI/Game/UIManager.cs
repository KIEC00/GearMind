using VContainer;

namespace Assets.GearMind.Scripts.UI.Game
{
    public abstract class UIManager
    {
        protected readonly IObjectResolver Resolver;

        protected UIManager(IObjectResolver resolver)
        {
            Resolver = resolver;
        }

        protected T CreateViewModel<T>() where T : class
        {
            return Resolver.Resolve<T>();
        }
    }
}