using System;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Data.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data
{
    [JsonObject(MemberSerialization.OptIn), Name("Пользовательские настройки")]
    public class UserSettings:BaseConfig
    {
        [JsonProperty, DBField] protected double _accuracy = 1d;
        [JsonProperty, DBField] private bool _invertX;
        [JsonProperty, DBField] private bool _invertY = true;
        [JsonProperty, DBField] private bool _invertZ;
        [ConfigProperty("Точность", 
            "Максимальное допустимое отклонение между точкой середины отрезка и точкой середины участка кривой, описываемой отрезком в мм")]
        public double Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
                HandleChange();
            }
        }
        
        [ConfigProperty("Инверсия по X")]
        public bool InvertX
        {
            get => _invertX;
            set
            {
                _invertX = value;
                HandleChange();
            }
        }
        [ConfigProperty("Инверсия по Y")]
        public bool InvertY
        {
            get => _invertY;
            set
            {
                _invertY = value;
                HandleChange();
            }
        }

        [ConfigProperty("Инверсия по Z")]
        public bool InvertZ
        {
            get => _invertZ;
            set
            {
                _invertZ = value;
                HandleChange();
            }
        }
    }
}