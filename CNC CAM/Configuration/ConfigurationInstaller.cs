using System.Linq;
using CNC_CAM.Base;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Configuration.Rule;
using CNC_CAM.Configuration.View;
using CNC_CAM.Data;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using DryIoc;

namespace CNC_CAM.Configuration;

public class ConfigurationInstaller:Installer
{
    public override void Install(IContainer container)
    {
        container.Register<CurrentConfiguration>(Reuse.Singleton);
        container.Register<ManageConfigurationWindow>(Reuse.Transient);
        container.RegisterSingletonNonLazy<SaveConfigsRule>();
        container.RegisterSingletonNonLazy<DeleteConfigRule>();
        container.RegisterSingletonNonLazy<DuplicateConfigRule>();
        container.RegisterSingletonNonLazy<EditConfigRule>();
        container.RegisterSingletonNonLazy<SetConfigRule>();
        container.RegisterSingletonNonLazy<ExportConfigRule>();
        container.RegisterSingletonNonLazy<ImportConfigRule>();
        InstallConfigs(container);
    }
    
    protected void InstallConfigs(IContainer container)
    {
        var dbService = container.Resolve<DBService>();
        var configurationStorage = new ConfigurationStorage();
        if (configurationStorage.LastConfigurations.Keys.Count == 0)
        {
            foreach (var defaultConfig in Const.Configs.DefaultConfigs)
            {
                foreach (BaseConfig config in dbService.Load(defaultConfig.GetType(), defaultConfig))
                {
                    configurationStorage.RegisterConfig(config);
                    configurationStorage.SetAsLast(config);   
                }
            }
        }

        var last = dbService.Load<Config>().FirstOrDefault();
        if (last != null)
            configurationStorage.LastConfigurations = last.GetLastIds();
        container.RegisterInstance(configurationStorage);
    }
}