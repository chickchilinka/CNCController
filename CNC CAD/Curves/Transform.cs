using System.Windows;
using System.Windows.Media;

namespace CNC_CAD.Curves
{
    public abstract class Transform
    {
        public Matrix TransformationMatrix { get; set; } = Matrix.Identity;
        
        public virtual Vector ToGlobalPoint(Vector point)
        {
            return point * TransformationMatrix;
        }
        public virtual void Move(Vector delta)
        {
            
        }
        public virtual void Scale(Vector multiplication, Vector? pivot=null){}
        public virtual void Rotate(float angle, Vector? pivot=null){}

        public virtual void Skew(float x, float y){}
    }
}