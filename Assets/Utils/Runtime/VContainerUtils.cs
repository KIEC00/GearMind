using VContainer;

namespace Assets.Utils.Runtime
{
    public static class VContainerUtils
    {
        public static RegistrationBuilder BindAll(this RegistrationBuilder builder) =>
            builder.AsSelf().AsImplementedInterfaces();
    }
}
