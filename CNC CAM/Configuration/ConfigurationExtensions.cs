using System.Windows;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.Configuration;

public static class ConfigurationExtensions
{
    public static Vector ConvertVectorToPhysical(this CurrentConfiguration currentConfiguration, Vector position)
    {
        var worksheetConfig = currentConfiguration.Get<WorksheetConfig>();
        var userConfig = currentConfiguration.Get<UserSettings>();
        position *= worksheetConfig.Scale;
        double x = position.X;
        double y = position.Y;
        if (userConfig.InvertY)
        {
            y = worksheetConfig.MaxY - y;
        }
        if (userConfig.InvertX)
        {
            x = worksheetConfig.MaxX - x;
        }
        return new Vector(x, y);
    }
}