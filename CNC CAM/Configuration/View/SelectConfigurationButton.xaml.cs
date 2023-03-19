using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Configuration.Data;
using DryIoc;
using IContainer = DryIoc.IContainer;

namespace CNC_CAM.Configuration.View;

public partial class SelectConfigurationButton
{
    public class PropertyRow
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
    public Type ConfigType { get; set; }
    public string ConfigName { get; set; }

    private CurrentConfiguration _currentConfiguration;
    private IContainer _container;
    private ObservableCollection<PropertyRow> _propertyRows =  new ObservableCollection<PropertyRow>();
    public ObservableCollection<PropertyRow> PropertyRows => _propertyRows;

    private bool _initialized = false;
    public SelectConfigurationButton()
    {
        _container = MainScope.Instance.Container;
        _currentConfiguration = _container.Resolve<CurrentConfiguration>();
        _currentConfiguration.OnCurrentConfigChanged += Initialize;
        InitializeComponent();
        Loaded += UserControl1_Loaded;
    }

    void UserControl1_Loaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        window.Closing += window_Closing;
    }

    void window_Closing(object sender, CancelEventArgs e)
    {
        _currentConfiguration.OnCurrentConfigChanged -= Initialize;
    }
    private void Initialize(Type type)
    {
        if(type!=ConfigType)
            return;
        var config = _currentConfiguration.GetCurrentConfig(ConfigType);
        FillLabels(config);
    }
    protected override void OnRender(DrawingContext drawingContext)
    {
        if (!_initialized)
        {
            Initialize(ConfigType);
            _initialized = true;
        }
        base.OnRender(drawingContext);
    }
    private void SelectButton_OnClick(object sender, RoutedEventArgs e)
    {
        SelectConfigurationWindow window = _container.Resolve<SelectConfigurationWindow>();
        window.Initialize(ConfigType);
        window.Show();
    }

    private void FillLabels(BaseConfig config)
    {
        PropertyRows.Clear();
        var properties = config.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<ConfigPropertyAttribute>();
            if (attribute == null)
            {
                continue;
            }
            PropertyRows.Add(new PropertyRow(){
                Name = attribute.Name,
                Value = property.GetValue(config)
            });
        }
    }
}