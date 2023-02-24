using System;

namespace CNC_CAM.Machine.Configs
{
    [Serializable]
    public struct AccuracySettings
    {
        public double Accuracy;

        public AccuracySettings(double accuracy)
        {
            Accuracy = accuracy;
        }
    }
}