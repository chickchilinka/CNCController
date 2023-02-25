using System;

namespace CNC_CAM.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigPropertyAttribute:Attribute
{
    public readonly string Name;
    public readonly string Description;
    public ConfigPropertyAttribute(string name, string description = null)
    {
        Name = name;
        Description = description;
    }
}