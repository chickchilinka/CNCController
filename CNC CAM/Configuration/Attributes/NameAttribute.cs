using System;

namespace CNC_CAM.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NameAttribute:Attribute
{
    public string Name { get; }

    public NameAttribute(string name)
    {
        Name = name;
    }
}