using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CNC_CAM.Configuration.Data;
using DryIoc;

namespace CNC_CAM.Configuration.View;

public partial class SelectConfigurationWindow : Window
{
    private ObservableCollection<BaseConfig> _configs;
    public ObservableCollection<BaseConfig> Configs => _configs;
    private ConfigurationStorage _configurationStorage;
    private BaseConfig _selection;
    private SignalBus _signalBus;

    public SelectConfigurationWindow(ConfigurationStorage storage, SignalBus signalBus)
    {
        _configurationStorage = storage;
        _signalBus = signalBus;
    }


    public void Initialize(Type type)
    {
        _configs = _configurationStorage.GetAll(type);
        InitializeComponent();
        SetButtonsEnabled(false);
    }

    private void SetButtonsEnabled(bool enabled)
    {
        DuplicateButton.IsEnabled = enabled;
        DeleteButton.IsEnabled = enabled  && _configs.Count > 1;
        EditButton.IsEnabled = enabled;
        UseButton.IsEnabled = enabled;
    }

    private void Table_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is BaseConfig)
        {
            SetButtonsEnabled(true);
            _selection = e.AddedItems[0] as BaseConfig;
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
        Close();
    }
}