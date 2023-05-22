using System;
using System.Windows.Controls;
using CNC_CAM.Configuration;
using DryIoc;

namespace CNC_CAM.UI.CustomWPFElements;

public class ConfigurationTextbox:Label
{
    public Func<string> LabelFormatter { get; set; }

    private CurrentConfiguration _currentConfiguration;
    public override void EndInit()
    {
        base.EndInit();
        _currentConfiguration = MainScope.Instance.Container.Resolve<CurrentConfiguration>();
        _currentConfiguration.OnCurrentConfigChanged+=OnConfigChanged;
        OnConfigChanged(null);
    }

    private void OnConfigChanged(Type _)
    {
        this.Content = LabelFormatter?.Invoke() ?? string.Empty;
    }
}