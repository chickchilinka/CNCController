using System.Windows;

namespace CNC_CAD.CNC.Controllers
{
    public class CncConfig
    {
        public bool InvertX;
        public bool InvertY;
        public bool InvertZ;
        public string AxisX = "X";
        public string AxisY = "Y";
        public string AxisZ = "Z";
        public double HeadUp = 0;
        public double HeadDown = -20;
        public string COMPort;
        public int BaudRate;

        public Vector ConvertVectorToPhysical(Vector position)
        {
            //TODO:Implement conversion
            return position;
        }
    }
}