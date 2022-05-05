namespace CNC_CAD.CNC.Controllers
{
    public struct CNCConfig
    {
        public bool InvertX;
        public bool InvertY;
        public bool InvertZ;
        public string AxisX;
        public string AxisY;
        public string AxisZ;
        public string COMPort;
        public int BaudRate;

        public CNCConfig(string port, int rate, bool invertX = false, bool invertY = false, bool invertZ=false, string axisX="X", string axisY="Y", string axisZ="Z")
        {
            COMPort = port;
            BaudRate = rate;
            InvertX = invertX;
            InvertY = invertY;
            InvertZ = invertZ;
            AxisX = axisX;
            AxisY = axisY;
            AxisZ = axisZ;
        }
    }
}