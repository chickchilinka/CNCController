using System;
using System.Windows;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Data.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data
{
    [JsonObject(MemberSerialization.OptIn), Name("Рабочее поле")]
    public class WorksheetConfig:BaseConfig
    {
        [JsonProperty, DBField] private double _boundsMaxX = 200;
        [JsonProperty, DBField] private double _boundsMaxY = 150;
        [JsonProperty, DBField] private double _boundsMinX = 0;
        [JsonProperty, DBField] private double _boundsMinY = 0;
        [JsonProperty, DBField] private double _gridSizeX = 50;
        [JsonProperty, DBField] private double _gridSizeY = 50;
        [JsonProperty, DBField] private double _scale = 0.5d;

        [ConfigProperty("Левая граница X")]
        public double MinX
        {
            get => _boundsMinX;
            set
            {
                _boundsMinX = value;
                HandleChange();
            }
        }
        [ConfigProperty("Верхняя граница Y")]
        public double MinY
        {
            get => _boundsMinY;
            set
            {
                _boundsMinY = value;
                HandleChange();
            }
        }
        [ConfigProperty("Правая граница X")]
        public double MaxX
        {
            get => _boundsMaxX;
            set
            {
                _boundsMaxX = value;
                HandleChange();
            }
        }
        [ConfigProperty("Нижняя Граница Y")]
        public double MaxY
        {
            get => _boundsMaxY;
            set
            {
                _boundsMaxY = value;
                HandleChange();
            }
        }
        
        [ConfigProperty("Размер сетки X")]
        public double GridSizeX
        {
            get => _gridSizeX;
            set
            {
                _gridSizeX = value;
                HandleChange();
            }
        }
        [ConfigProperty("Размер сетки Y")]
        public double GridSizeY
        {
            get => _gridSizeY;
            set
            {
                _gridSizeY = value;
                HandleChange();
            }
        }

        [ConfigProperty("MM per pixel")]
        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                HandleChange();
            }
        }
    }
}