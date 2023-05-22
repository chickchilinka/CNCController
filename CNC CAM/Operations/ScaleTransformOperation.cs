
using System.Windows;
using System.Windows.Media;

namespace CNC_CAM.Operations;

public class ScaleTransformOperation:TransformViewOperation
{
    public ScaleTransformOperation(string name) : base(name)
    {
    }
    
    public virtual void Scale(Vector scale, Vector anchor)
    {
        var identityScaled = Matrix.Identity;
        identityScaled.ScaleAt(scale.X, scale.Y, anchor.X, anchor.Y);
        TransformationMatrix = identityScaled;
    }
}