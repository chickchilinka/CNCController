using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;
using DryIoc;

namespace CNC_CAM.Configuration.View;

public partial class ManageConfigurationWindow : Window
{
    private Dictionary<string, Type> _treeViewConfigTypes = new Dictionary<string, Type>();
    private ObservableCollection<BaseConfig> _configs;
    public ObservableCollection<BaseConfig> Configs => _configs;
    private ConfigurationStorage _configurationStorage;
    private BaseConfig _selection;
    private SignalBus _signalBus;
    private Type _configType;

    public ManageConfigurationWindow(ConfigurationStorage storage, SignalBus signalBus)
    {
        _configurationStorage = storage;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        InitializeComponent();
        var types = Const.Configs.DefaultConfigs.Select(config => config.GetType());
        foreach (var type in types)
        {
            var nameAttribute = type.GetCustomAttribute(typeof(NameAttribute)) as NameAttribute;
            if (nameAttribute == null)
                continue;
            TreeView.Items.Add(nameAttribute.Name);
            _treeViewConfigTypes.Add(nameAttribute.Name, type);
        }

        TreeView.SelectedItemChanged -= ItemsOnCurrentChanged;
        TreeView.SelectedItemChanged += ItemsOnCurrentChanged;
    }

    private void ItemsOnCurrentChanged(object sender, EventArgs e)
    {
        var item = TreeView.SelectedItem as string;
        if(!_treeViewConfigTypes.TryGetValue(item, out var type))
            return;
        Initialize(type);
    }

    public void Initialize(Type type)
    {
        if (!IsInitialized)
        {
            Initialize();
        }
        ManageConfigurationPanel.Inject(_configurationStorage, _signalBus);
        ManageConfigurationPanel.Initialize(type);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _signalBus.Fire(new ConfigurationSignals.SaveConfigs());
    }
}