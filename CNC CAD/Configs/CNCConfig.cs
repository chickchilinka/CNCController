using System.Windows;

namespace CNC_CAD.Configs
{
    public class CncConfig
    {
        public bool InvertX;
        public bool InvertY = true;
        public bool InvertZ;
        public string AxisX = "X";
        public string AxisY = "Y";
        public string AxisZ = "Z";
        public double HeadUp = 0;
        public double HeadDown;
        public string COMPort;
        public int BaudRate;
        public AccuracySettings AccuracySettings = new AccuracySettings(10, 0.01d);
        public WorksheetConfig WorksheetConfig = new WorksheetConfig();

        public Vector ConvertVectorToPhysical(Vector position)
        {
            double x = position.X;
            double y = position.Y;
            if (InvertY)
            {
                y = WorksheetConfig.MaxY - y;
            }
            if (InvertX)
            {
                x = WorksheetConfig.MaxX - x;
            }
            return new Vector(x, y);
        }
    }
}