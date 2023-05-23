using System;

namespace CNC_CAM.Data.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class DBFieldAttribute:Attribute
{
    public string? CustomType { get; set; }
    public int Priority { get; set; } = 0;

    public DBFieldAttribute(){}
}