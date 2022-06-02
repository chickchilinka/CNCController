namespace CNC_CAD.Configs
{
    public struct AccuracySettings
    {
        public double AngleAccuracy;
        public double RelativeAccuracy;

        public AccuracySettings(double angleAccuracy, double relativeAccuracy)
        {
            AngleAccuracy = angleAccuracy;
            RelativeAccuracy = relativeAccuracy;
        }
    }
}