using System;
using System.Collections.Generic;

namespace CNC_CAM.Data;

public static class DataTypeMapping
{
    private static Dictionary<Type, string> _mapping = new Dictionary<Type, string>()
    {
        { typeof(int), "INTEGER" },
        { typeof(long), "INTEGER" },
        { typeof(double), "REAL" },
        { typeof(float), "REAL" },
        { typeof(string), "TEXT" },
        { typeof(bool), "INTEGER" },
        { typeof(DateTime), "DATETIME" }
    };

    public static string GetSQLType(Type type)
    {
        if (_mapping.TryGetValue(type, out var sqlType))
        {
            return sqlType;
        }

        return "";
    }
}