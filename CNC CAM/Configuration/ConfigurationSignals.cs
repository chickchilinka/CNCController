using System;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.Configuration;

public class ConfigurationSignals
{
    public class EditConfig
    {
        public BaseConfig Config { get; }
        public Action<BaseConfig> OnEdited { get; }

        public EditConfig(BaseConfig config, Action<BaseConfig> callback = null)
        {
            Config = config;
            OnEdited = callback;
        }
    }

    public class DuplicateConfig
    {
        public BaseConfig Config { get; }

        public DuplicateConfig(BaseConfig config)
        {
            Config = config;
        }
    }
    
    public class DeleteConfig
    {
        public BaseConfig Config { get; }

        public DeleteConfig(BaseConfig config)
        {
            Config = config;
        }
    }
    
    public class SetConfig
    {
        public BaseConfig Config { get; }

        public SetConfig(BaseConfig config)
        {
            Config = config;
        }
    }

    public class ImportConfig
    {
        public Type ConfigType { get; }

        public ImportConfig(Type configType)
        {
            ConfigType = configType;
        }
    }
    public class ExportConfig
    {
        public BaseConfig Config { get; }

        public ExportConfig(BaseConfig config)
        {
            Config = config;
        }
    }

    public class SaveConfigs
    {
    }
}