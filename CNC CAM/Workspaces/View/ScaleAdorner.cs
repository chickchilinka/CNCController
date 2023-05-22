using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Operations;
using DryIoc;

namespace CNC_CAM.Workspaces.View;

public class ScaleAdorner : BaseAdorner
{
    public enum ScaleMode
    {
        Horizontal,
        Vertical
    }

    private FormattedText _formattedTextBottom;
    private FormattedText _formattedTextRight;
    private Point _positionInBlock;
    private ScaleTransformOperation _scaleTransformOperation;
    private Vector _anchor;
    private ScaleMode _scaleMode;
    private double _capturedWidth;
    private double _capturedHeight;

    private CurrentConfiguration _currentConfiguration;

    public ScaleAdorner(UIElement adornedElement) : base(adornedElement)
    {
        _currentConfiguration = MainScope.Instance.Container.Resolve<CurrentConfiguration>();
        _currentConfiguration.OnCurrentConfigChanged+=OnConfigChanged;
    }

    private void OnConfigChanged(Type _)
    {
        InvalidateVisual();
    }

    ~ScaleAdorner()
    {
        _currentConfiguration.OnCurrentConfigChanged-=OnConfigChanged;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        var mousePosition = Mouse.GetPosition(AdornedElement);
        var container = VisualTreeHelper.GetParent(this) as UIElement;
        if (container == null)
            return;
        _positionInBlock = e.GetPosition(container);
        _scaleTransformOperation = new ScaleTransformOperation("Scale");
        _scaleTransformOperation.Initialize(_workspaceElementView.Element);
        Rect adornedElementRect = VisualTreeHelper.GetDescendantBounds(AdornedElement);
        if (mousePosition.X < adornedElementRect.Left + 10 && mousePosition.X > adornedElementRect.Left- 10)
        {
            _anchor = new Vector(1, 0.5f);
            _scaleMode = ScaleMode.Horizontal;
        }
        else if (_positionInBlock.X >adornedElementRect.Right - 10 &&
                 _positionInBlock.X <adornedElementRect.Right + 10)
        {
            _anchor = new Vector(0, 0.5f);
            _scaleMode = ScaleMode.Horizontal;
        }
        else if (mousePosition.Y < adornedElementRect.Top + 10 && mousePosition.Y > adornedElementRect.Top - 10)
        {
            _anchor = new Vector(0.5f, 1);
            _scaleMode = ScaleMode.Vertical;
        }
        else if (_positionInBlock.Y > adornedElementRect.Bottom - 10 &&
                 _positionInBlock.Y <adornedElementRect.Bottom + 10)
        {
            _anchor = new Vector(0.5f, 0);
            _scaleMode = ScaleMode.Vertical;
        }

        _capturedWidth = adornedElementRect.Width;
        _capturedHeight = adornedElementRect.Height;
        CaptureMouse();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        var container = VisualTreeHelper.GetParent(this) as UIElement;
        if (container == null)
            return;
        var mousePositionInBlock = e.GetPosition(container);
        var mousePosition = Mouse.GetPosition(AdornedElement);
        Rect adornedElementRect = VisualTreeHelper.GetDescendantBounds(AdornedElement);
        if (IsMouseCaptured)
        {
            if (_scaleMode == ScaleMode.Horizontal)
            {
                var deltaX = 0d;

                if (Math.Abs(_anchor.X - 1) < 0.01)
                    deltaX = _positionInBlock.X - mousePositionInBlock.X;
                else
                    deltaX = mousePositionInBlock.X - _positionInBlock.X;
                var scale = (_capturedWidth + deltaX) / _capturedWidth;
                _scaleTransformOperation.Scale(new Vector(scale, scale),
                    new Vector(adornedElementRect.X + adornedElementRect.Width / 2f,
                        adornedElementRect.Y + adornedElementRect.Height / 2f));
                _scaleTransformOperation.Preview();
            }
            else if (_scaleMode == ScaleMode.Vertical)
            {
                var deltaY = 0d;

                if (Math.Abs(_anchor.Y - 1) < 0.01)
                    deltaY = _positionInBlock.Y - mousePositionInBlock.Y;
                else
                    deltaY = mousePositionInBlock.Y - _positionInBlock.Y;
                var scale = (_capturedHeight + deltaY) / _capturedHeight;
                _scaleTransformOperation.Scale(new Vector(scale, scale),
                    new Vector(adornedElementRect.X + Math.Abs(scale>0 ? adornedElementRect.Width:0) * _anchor.X,
                        adornedElementRect.Y + Math.Abs(scale>0 ? adornedElementRect.Height:0) * _anchor.Y));
                _scaleTransformOperation.Preview();
            }
        }

        if (mousePosition.X < adornedElementRect.Left + 10 && mousePosition.X > adornedElementRect.Left- 10)
        {
            Cursor = Cursors.SizeWE;
        }
        else if (_positionInBlock.X >adornedElementRect.Right - 10 &&
                 _positionInBlock.X <adornedElementRect.Right + 10)
        {
            Cursor = Cursors.SizeWE;
        }
        else if (mousePosition.Y < adornedElementRect.Top + 10 && mousePosition.Y > adornedElementRect.Top - 10)
        {
            Cursor = Cursors.SizeNS;
        }
        else if (_positionInBlock.Y > adornedElementRect.Bottom - 10 &&
                 _positionInBlock.Y <adornedElementRect.Bottom + 10)
        {
            Cursor = Cursors.SizeNS;
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        if(!IsMouseCaptured)
            return;
        ReleaseMouseCapture();
        OperationsController.LaunchOperation(_scaleTransformOperation);
        _scaleTransformOperation = null;
    }


    protected override void OnRender(DrawingContext drawingContext)
    {
        Rect adornedElementRect = VisualTreeHelper.GetDescendantBounds(AdornedElement);

        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
        renderBrush.Opacity = 0.2;
        SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
        Pen blackPen = new Pen(new SolidColorBrush(Colors.Black), 1.5);
        double renderRadius = 5.0;
        drawingContext.DrawRectangle(null, renderPen, adornedElementRect);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        var widthText =
            CreateFormattedText(
                (adornedElementRect.Width * _currentConfiguration.Get<WorksheetConfig>().Scale).ToString("0.00"));
        var heightText =
            CreateFormattedText((adornedElementRect.Height * _currentConfiguration.Get<WorksheetConfig>().Scale)
                .ToString("0.00"));
        var widthTextPosition = new Point(adornedElementRect.X - widthText.Width / 2 + adornedElementRect.Width / 2,
            adornedElementRect.Bottom);
        var heightTextPosition = new Point(adornedElementRect.Right,
            adornedElementRect.Y - heightText.Height / 2 + adornedElementRect.Height / 2);
        drawingContext.DrawRectangle(whiteBrush, blackPen, AddMargin(new Rect(widthTextPosition, new Size(widthText.Width, widthText.Height)),0));
        drawingContext.DrawRectangle(whiteBrush, blackPen, AddMargin(new Rect(heightTextPosition, new Size(heightText.Width, heightText.Height)),0));
        drawingContext.DrawText(widthText, widthTextPosition);
        drawingContext.DrawText(heightText,heightTextPosition);
    }

    private FormattedText CreateFormattedText(string text)
    {
        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
        return new FormattedText(text,
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            15,
            renderBrush,
            new NumberSubstitution(),
            TextFormattingMode.Display, 1);
    }

    private Rect AddMargin(Rect rect, double margin)
    {
        rect.X -= margin;
        rect.Y -= margin;
        rect.Width += margin;
        rect.Height += margin;
        return rect;
    }
}