using System.ComponentModel;
using System.Runtime.CompilerServices;
using CNC_CAM.Annotations;

namespace CNC_CAM.Configuration.Data;

public class ConfigDataWrapper:INotifyPropertyChanged
{
    public bool IsCurrent { get; set; }
    public BaseConfig Config { get; }

    public ConfigDataWrapper(BaseConfig config, bool isCurrent)
    {
        Config = config;
        IsCurrent = isCurrent;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}