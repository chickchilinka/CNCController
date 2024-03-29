using System;
using System.Windows;
using CNC_CAM.Tools;

namespace CNC_CAM.Machine.Configs
{
    [Serializable]
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
        public double BaseFeedRate = 3000;
        public double PxToMMFactor = 1d;
        public int BaudRate;
        public AccuracySettings AccuracySettings = Const.Configs.DefaultAccuracy;
        public WorksheetConfig WorksheetConfig = new WorksheetConfig();

        public Vector ConvertVectorToPhysical(Vector position)
        {
            position *= PxToMMFactor;
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