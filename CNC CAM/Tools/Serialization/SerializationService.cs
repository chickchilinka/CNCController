using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CNC_CAM.Tools.Serialization;

public class SerializationService
{
    private const Formatting Formatting = Newtonsoft.Json.Formatting.Indented;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.All
    }; 
    public void Serialize<T>(string path, string filename, T obj)
    {
        path = Environment.ExpandEnvironmentVariables(path);
        var fullPath = path+filename;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllText(fullPath,JsonConvert.SerializeObject(obj, Formatting, _settings));
    }

    public T Deserialize<T>(string path, string filename, T defaultValue)
    {
        try
        {
            path = Environment.ExpandEnvironmentVariables(path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(path + filename))
                return defaultValue;
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path + filename), _settings);
        }
        catch (IOException exception)
        {
            return defaultValue;
        }
    }
}