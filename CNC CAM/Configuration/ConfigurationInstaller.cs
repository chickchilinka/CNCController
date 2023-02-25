using CNC_CAM.Base;
using CNC_CAM.Configuration.Rule;
using DryIoc;

namespace CNC_CAM.Configuration;

public class ConfigurationInstaller:Installer
{
    public override void Install(IContainer container)
    {
        container.Register<CurrentConfiguration>(Reuse.Singleton);
        container.RegisterSingletonNonLazy<SaveConfigsRule>();
        container.RegisterSingletonNonLazy<EditConfigRule>();
    }
}