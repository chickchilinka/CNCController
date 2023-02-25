using System;
using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AccuracySettings:BaseConfig
    {
        [JsonProperty]
        protected double _accuracy = 1d;
        [ConfigProperty("Accuracy", 
            "The maximum deviation between the middle of the curve segment and the middle of the line connecting the ends of the segment (in mm)")]
        public double Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
                HandleChange();
            }
        }
    }
}