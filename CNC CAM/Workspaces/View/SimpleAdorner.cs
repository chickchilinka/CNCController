using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CNC_CAM.Workspaces.View;

public class SimpleAdorner:Adorner
{
    private FormattedText _formattedTextBottom;
    private FormattedText _formattedTextRight;
    public SimpleAdorner(UIElement adornedElement) : base(adornedElement)
    {
    }



    protected override void OnRender(DrawingContext drawingContext)
    {
        Rect adornedElementRect = VisualTreeHelper.GetDescendantBounds(AdornedElement);

        // Some arbitrary drawing implements.
        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
        renderBrush.Opacity = 0.2;
        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
        double renderRadius = 5.0;

        // Draw a circle at each corner.
        drawingContext.DrawRectangle(null, renderPen, adornedElementRect);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        drawingContext.DrawText(CreateFormattedText(adornedElementRect.Width.ToString()), new Point(adornedElementRect.X+adornedElementRect.Width/2, adornedElementRect.Bottom));
        drawingContext.DrawText(CreateFormattedText(adornedElementRect.Height.ToString()), new Point(adornedElementRect.Right, adornedElementRect.Y+adornedElementRect.Height/2));
    }

    public FormattedText CreateFormattedText(string text)
    {
        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
        return new FormattedText(text,
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            15,
            renderBrush,
            new NumberSubstitution(),
            TextFormattingMode.Display, 1);
    }
}