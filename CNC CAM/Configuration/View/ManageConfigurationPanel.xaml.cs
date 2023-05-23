using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.Configuration.View;

public partial class ManageConfigurationPanel : UserControl
{

    private ObservableCollection<ConfigDataWrapper> _configs = new();
    public ObservableCollection<ConfigDataWrapper> Configs => _configs;
    private ConfigurationStorage _configurationStorage;
    private BaseConfig _selection;
    private SignalBus _signalBus;
    private Type _configType;

    public void Inject(ConfigurationStorage storage, SignalBus signalBus)
    {
        _configurationStorage = storage;
        _signalBus = signalBus;
    }

    public void Initialize(Type type)
    {
        if(_configType!=null)
            _configurationStorage.GetAll(_configType).CollectionChanged -= OnCollectionChanged;
        _configurationStorage.GetAll(type).CollectionChanged += OnCollectionChanged;
        _configurationStorage.OnCurrentConfigChanged -= UpdateWrappers;
        _configurationStorage.OnCurrentConfigChanged += UpdateWrappers;
        _configType = type;
        UpdateWrappers(_configType);
        InitializeComponent();
        SetButtonsEnabled(false);
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    { 
        UpdateWrappers(_configType);
    }

    private void UpdateWrappers(Type type)
    {
        _configs.Clear();
        foreach (var config in _configurationStorage.GetAll(_configType))
        {
            _configs.Add(new ConfigDataWrapper(config, _configurationStorage.GetLast(_configType) == config));
        }
    }


    private void SetButtonsEnabled(bool enabled)
    {
        DuplicateButton.IsEnabled = enabled;
        DeleteButton.IsEnabled = enabled && _configs.Count > 1;
        EditButton.IsEnabled = enabled;
        UseButton.IsEnabled = enabled;
        ExportButton.IsEnabled = enabled;
    }

    private void Table_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is ConfigDataWrapper wrapper)
        {
            SetButtonsEnabled(true);
            _selection = wrapper.Config;
        }
        else
        {
            SetButtonsEnabled(false);
            _selection = null;
        }
    }

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selection == null)
            return;
        _signalBus.Fire(new ConfigurationSignals.EditConfig(_selection, (config) => { Table.Items.Refresh(); }));
        Table.UnselectAll();
    }

    private void DuplicateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selection == null)
            return;
        _signalBus.Fire(new ConfigurationSignals.DuplicateConfig(_selection));
        Table.UnselectAll();
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selection == null)
            return;
        _signalBus.Fire(new ConfigurationSignals.DeleteConfig(_selection));
        Table.UnselectAll();
    }

    private void UseButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selection == null)
            return;
        _signalBus.Fire(new ConfigurationSignals.SetConfig(_selection));
        Table.UnselectAll();
    }

    private void ImportButton_OnClick(object sender, RoutedEventArgs e)
    {
        _signalBus.Fire(new ConfigurationSignals.ImportConfig(_configType));
    }

    private void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selection == null)
            return;
        _signalBus.Fire(new ConfigurationSignals.ExportConfig(_selection));
    }

    private void MasterButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ConfigurationMasterWindow();
        window.ShowDialog();
    }
}