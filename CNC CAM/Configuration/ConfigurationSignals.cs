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
}