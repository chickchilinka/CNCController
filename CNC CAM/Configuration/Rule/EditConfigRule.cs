using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using CNC_CAM.Base;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Configuration.Data;
using CNC_CAM.UI.DrawShapeWindows;
using DryIoc;

namespace CNC_CAM.Configuration.Rule;

public class EditConfigRule:AbstractSignalRule<ConfigurationSignals.EditConfig>
{
    public EditConfigRule(SignalBus signalBus) : base(signalBus)
    {
    }

    protected override void OnSignalFired(ConfigurationSignals.EditConfig signal)
    {
        var properties = signal.Config.GetType().GetProperties();

        Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();
        var builder = GenericWindowWithFieldsBuilder.Create(OnSubmitLocal, OnCancel);
        builder.WithTitle(nameof(signal.Config));
        foreach (var property in properties)
        {
            var attributes = property.GetAttributes();
            var propertyAttribute = attributes.FirstOrDefault(attribute => attribute is ConfigPropertyAttribute) as ConfigPropertyAttribute;
            if(propertyAttribute == null)
                continue;
            propertyInfos.Add(propertyAttribute.Name, property);
            if (property.PropertyType == typeof(float))
                builder.AddSimpleFloatField(propertyAttribute?.Name, tooltipContent:propertyAttribute.Description,
                    defaultValue: Convert.ToSingle(property.GetValue(signal.Config)));
            if(property.PropertyType == typeof(double))
                builder.AddSimpleFloatField(propertyAttribute?.Name, tooltipContent:propertyAttribute.Description,
                    defaultValue: Convert.ToSingle(property.GetValue(signal.Config)));
        }

        var window = builder.Build();
        window.Show();
        
        void OnSubmitLocal(Dictionary<string, object> values)
        {
            foreach (var key in values.Keys)
            {
                propertyInfos[key].SetValue(signal.Config, values[key]);
            }
        }
    }

    private void OnCancel()
    {
        
    }
}