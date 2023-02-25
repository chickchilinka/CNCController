using System;
using System.Windows;
using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorksheetConfig:BaseConfig
    {
        [JsonProperty] private Vector _boundsMax = new Vector(200, 150);
        [JsonProperty] private Vector _gridSize = new Vector(50, 50);
        [JsonProperty] private double _pxToMmFactor = 1d;

        [ConfigProperty("Right Boundary")]
        public double MaxX
        {
            get => _boundsMax.X;
            set
            {
                _boundsMax = _boundsMax with { X = value };
                HandleChange();
            }
        }
        [ConfigProperty("Bottom Boundary")]
        public double MaxY
        {
            get => _boundsMax.Y;
            set
            {
                _boundsMax = _boundsMax with { Y = value };
                HandleChange();
            }
        }
        
        [ConfigProperty("Grid Size X")]
        public double GridSizeX
        {
            get => _gridSize.X;
            set
            {
                _gridSize = _gridSize with { X = value };
                HandleChange();
            }
        }
        [ConfigProperty("Grid Size Y")]
        public double GridSizeY
        {
            get => _gridSize.Y;
            set
            {
                _gridSize = _gridSize with { Y = value };
                HandleChange();
            }
        }

        [ConfigProperty("MM per pixel")]
        public double PxToMmFactor
        {
            get => _pxToMmFactor;
            set
            {
                _pxToMmFactor = value;
                HandleChange();
            }
        }
    }
}