using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using CNC_CAM.Data.Attributes;
using Microsoft.Data.Sqlite;

namespace CNC_CAM.Data;

public class DBService
{
    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection("Data Source=cnc_cam.db;Mode=ReadWriteCreate;");
        connection.Open();
        return connection;
    }

    public void Execute(string query, Action<SqliteDataReader> callback = null)
    {
        using var connection = CreateConnection();
        SqliteCommand command = new SqliteCommand(query, connection);
        callback?.Invoke(command.ExecuteReader());
    }

    public List<object> Load(Type type, object defaultValue = default)
    {
        CreateTableForType(type);
        var name = type.Name;
        var fields = LookupAllFields(type);
        var list = new List<object>();
        var query = $"SELECT * FROM {name}";
        Execute(query, reader =>
        {
            while (reader.Read())
            {
                var instance = Activator.CreateInstance(type);
                foreach (var field in fields)
                {
                    ReadFieldValue(field,  reader, instance);
                }
                list.Add(instance);
            }
        });
        if(list.Count==0)
            list.Add(defaultValue);
        return list;
    }
    public List<T> Load<T>(T defaultValue = default)
    {
        return Load(typeof(T), defaultValue).Select(value => (T)value).ToList();
    }
    public void Save(object obj)
    {
        CreateTableForType(obj.GetType());
        var fields = LookupAllFields(obj.GetType());
        InsertRow(fields, obj);
    }

    public void Remove(object obj)
    {
        CreateTableForType(obj.GetType());
        var fields = LookupAllFields(obj.GetType());
        var fieldDatas = FillFieldDatas(fields, obj);
        var key = fieldDatas.FirstOrDefault(data => data.IsPrimary);
        if(key==null)
            return;
        var query = $"DELETE FROM {obj.GetType().Name} WHERE `{key.FieldName}` = '{key.FieldValue}'";
        Execute(query, _=>{});
    }

    private void InsertRow(List<FieldInfo> fields, object obj)
    {
        var fieldData = FillFieldDatas(fields, obj).ToList();

        var query = $"INSERT OR REPLACE INTO `{obj.GetType().Name}`{CreateInsertRow(fieldData)}";
        Execute(query,_=>{});
    }

    private string CreateInsertRow(List<FieldData> fieldDatas)
    {
        var fields = string.Join(',', fieldDatas.Select(data =>$"`{data.FieldName}`"));
        var values = string.Join(',', fieldDatas.Select(data => $"'{data.FieldValue}'"));
        return $" ({fields}) VALUES ({values})";
    }


    private IEnumerable<FieldData> FillFieldDatas(List<FieldInfo> fields, object obj)
    {
        foreach (var field in fields)
        {
            yield return new FieldData()
            {
                FieldName = field.Name,
                FieldValue = field.GetValue(obj).ToString(),
                IsPrimary = field.GetCustomAttribute<DBPrimaryKeyAttribute>() != null
            };
        }
    }

    private List<FieldInfo> LookupAllFields(Type type)
    {
        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(info => info.GetCustomAttribute<DBFieldAttribute>() != null).ToList();
        var baseType = type.BaseType;
        if (baseType != null)
            fields.AddRange(LookupAllFields(baseType));
        return fields.OrderByDescending(info => info.GetCustomAttribute<DBFieldAttribute>().Priority).ToList();
    }

    private void CreateTableForType(Type type)
    {
        var query = "SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
        var exists = false;
        Execute(query, reader =>
        {
            while (reader.Read())
            {
                if (reader["name"].Equals(type.Name))
                {
                    exists = true;
                    return;
                }
            }
        });
        if (exists)
            return;

        var objFields = LookupAllFields(type);
        var fields = objFields.Select(info =>
        {
            var name = info.Name;
            var type = info.GetCustomAttribute<DBFieldAttribute>().CustomType ??
                       DataTypeMapping.GetSQLType(info.FieldType);
            var key = info.GetCustomAttribute<DBPrimaryKeyAttribute>() != null ? "PRIMARY KEY" : "";
            return $"{name} {type} {key}";
        });
        var fieldsIntializer = String.Join(',', fields);
        var checkTableQuery = $"CREATE TABLE IF NOT EXISTS `{type.Name}` ({fieldsIntializer})";
        Execute(checkTableQuery, _ => { });
    }


    private void ReadFieldValue(FieldInfo fieldInfo, SqliteDataReader sqliteDataReader, object instance)
    {
        fieldInfo.SetValue(instance, GetValue(fieldInfo, sqliteDataReader));
    }

    private object GetValue(FieldInfo fieldInfo, SqliteDataReader sqliteDataReader)
    {
        if (fieldInfo.FieldType == typeof(double))
            return double.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        if (fieldInfo.FieldType == typeof(float))
            return float.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        if (fieldInfo.FieldType == typeof(int))
            return int.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        if (fieldInfo.FieldType == typeof(long))
            return long.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        if (fieldInfo.FieldType == typeof(DateTime))
            return DateTime.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        if (fieldInfo.FieldType == typeof(bool))
            return bool.Parse(sqliteDataReader[fieldInfo.Name].ToString());
        return sqliteDataReader[fieldInfo.Name];
    }
}