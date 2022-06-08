namespace CNC_CAD.Configs
{
    public struct AccuracySettings
    {
        public static double MaxAccuracyPer10MM = 0.01d;
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