using CNC_CAM.Base;
using CNC_CAM.Configuration.Rule;
using CNC_CAM.Configuration.View;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using DryIoc;

namespace CNC_CAM.Configuration;

public class ConfigurationInstaller:Installer
{
    public override void Install(IContainer container)
    {
        container.Register<CurrentConfiguration>(Reuse.Singleton);
        container.Register<SelectConfigurationWindow>(Reuse.Transient);
        container.RegisterSingletonNonLazy<SaveConfigsRule>();
        container.RegisterSingletonNonLazy<DeleteConfigRule>();
        container.RegisterSingletonNonLazy<DuplicateConfigRule>();
        container.RegisterSingletonNonLazy<EditConfigRule>();
        container.RegisterSingletonNonLazy<SetConfigRule>();
        InstallConfigs(container);
    }
    
    protected void InstallConfigs(IContainer container)
    {
        var serializationService = container.Resolve<SerializationService>();
        var configurationStorage = serializationService.Deserialize(
            Const.Paths.ConfigurationsPath, Const.Paths.LastConfigsFilename, 
            new ConfigurationStorage());
        if (configurationStorage.LastConfigurations.Keys.Count == 0)
        {
            foreach (var config in Const.Configs.DefaultConfigs)
            {
                configurationStorage.RegisterConfig(config);
                configurationStorage.SetAsLast(config);
            }
        }
        container.RegisterInstance(configurationStorage);
    }
}