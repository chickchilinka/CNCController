using System;
using System.IO;
using System.Xml.Serialization;

namespace CNC_CAM.Tools.Serialization;

public class SimpleSerializer
{
    public void Serialize<T>(string path, string filename, T obj)
    {
        path = Environment.ExpandEnvironmentVariables(path);
        var fullPath = path+filename;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        Stream fileStream = File.Create(fullPath);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        xmlSerializer.Serialize(fileStream, obj);
        fileStream.Close();
    }

    public T Deserialize<T>(string path, T defaultValue)
    {
        try
        {
            Stream fileStream = File.Open(Environment.ExpandEnvironmentVariables(path), FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(fileStream);
        }
        catch (IOException exception)
        {
            return defaultValue;
        }
    }
}