using DryIoc;

namespace CNC_CAM.Base;

public static class ContainerExtensions
{
    public const string NonLazy = "NonLazy";
    public static void RegisterSingletonNonLazy<TService>(this IContainer container)
    {
        container.Register<TService>(Reuse.Singleton, setup: Setup.With(NonLazy));
    }
}