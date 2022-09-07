using System;

namespace CNC_CAM.Machine.Configs
{
    [Serializable]
    public struct AccuracySettings
    {
        public static double MaxAccuracyPer10MM = 100d;
        public double AngleAccuracy;
        public double RelativeAccuracy;
        public double AccuracyPer10MM;

        public AccuracySettings(double angleAccuracy, double relativeAccuracy, double accuracyPer10Mm)
        {
            AngleAccuracy = angleAccuracy;
            RelativeAccuracy = relativeAccuracy;
            AccuracyPer10MM = accuracyPer10Mm;
        }
    }
}