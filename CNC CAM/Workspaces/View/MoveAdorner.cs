using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CNC_CAM.Operations;
using DryIoc;

namespace CNC_CAM.Workspaces.View;

public class MoveAdorner:BaseAdorner
{
    private FormattedText _formattedTextBottom;
    private FormattedText _formattedTextRight;
    private Point _positionInBlock;
    private MoveTransformOperation _moveTransformOperation;
    public MoveAdorner(UIElement adornedElement) : base(adornedElement)
    {

    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        _positionInBlock = Mouse.GetPosition(this);
        _moveTransformOperation = new MoveTransformOperation("Move");
        _moveTransformOperation.Initialize(_workspaceElementView.Element);
        CaptureMouse();
    }


    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (this.IsMouseCaptured)
        {
            // get the parent container
            var container = VisualTreeHelper.GetParent(this) as UIElement;

            if(container == null)
                return;
            
            // get the position within the container
            var mousePosition = e.GetPosition(container);

            // move the usercontrol.
            _moveTransformOperation.Move(new Vector(mousePosition.X - _positionInBlock.X, mousePosition.Y - _positionInBlock.Y));
            _moveTransformOperation.Preview();
            _positionInBlock = mousePosition;
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        if(!IsMouseCaptured)
            return;
        ReleaseMouseCapture();
        OperationsController.LaunchOperation(_moveTransformOperation);
        _moveTransformOperation = null;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        Rect adornedElementRect = VisualTreeHelper.GetDescendantBounds(AdornedElement);
        
        SolidColorBrush fillBrush = new SolidColorBrush(Colors.Green);
        fillBrush.Opacity = 0d;
        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);

        // Draw a circle at each corner.
        drawingContext.DrawRectangle(fillBrush, null, adornedElementRect);
    }
}