using System;
using System.Windows;
using System.Windows.Media;

namespace CNC_CAM.SVG.Subpaths
{
    public abstract class Transform
    {
        protected Matrix _transformationMatrix = Matrix.Identity;
        public virtual Matrix TransformationMatrix
        {
            get => _transformationMatrix;
            set
            {
                _transformationMatrix = value; 
                OnChange?.Invoke();
            }
        }
        public virtual Matrix FinalMatrix => TransformationMatrix * (Parent?.FinalMatrix ?? Matrix.Identity);
        public event Action OnChange;
        
        protected Transform _parent;
        public virtual Transform Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                OnChange?.Invoke();
            }
        }
        public virtual Vector ToGlobalPoint(Vector point)
        {
            if (Parent == null)
            {
                var newPointNoParent =  TransformationMatrix.Transform(new Point(point.X, point.Y));
                point.X = newPointNoParent.X;
                point.Y = newPointNoParent.Y;
                return point;
            }
            var newPoint =  TransformationMatrix.Transform(new Point(point.X, point.Y));
            point.X = newPoint.X;
            point.Y = newPoint.Y;
            return Parent.ToGlobalPoint(point);
        }
     
        public virtual double? GetDistanceTo(Transform transform)
        {
            return null;
        }
    }
}