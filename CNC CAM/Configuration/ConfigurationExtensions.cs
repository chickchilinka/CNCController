using System.Windows;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.Configuration;

public static class ConfigurationExtensions
{
    public static Vector ConvertVectorToPhysical(this CurrentConfiguration currentConfiguration, Vector position)
    {
        var worksheetConfig = currentConfiguration.GetCurrentConfig < WorksheetConfig>();
        var controlConfig = currentConfiguration.GetCurrentConfig<CNCControlSettings>();
        position *= worksheetConfig.PxToMmFactor;
        double x = position.X;
        double y = position.Y;
        if (controlConfig.InvertY)
        {
            y = worksheetConfig.MaxY - y;
        }
        if (controlConfig.InvertX)
        {
            x = worksheetConfig.MaxX - x;
        }
        return new Vector(x, y);
    }
}