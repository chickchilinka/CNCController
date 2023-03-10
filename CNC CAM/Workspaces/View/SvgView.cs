using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Workspaces.View;

public class SvgView : Grid, IDisposable, IWorkspaceElementView<WorkspaceElement<SvgRoot>>
{
    private Dictionary<SvgElement, Shape> _shapes;
    private WPFViewFactory _factory;
    private SvgRoot _svgRoot;
    public Rect Bounds { get; private set; }

    public SvgView(WPFViewFactory factory)
    {
        _factory = factory;
    }

    public void Initialize(WorkspaceElement<SvgRoot> workspaceElement)
    {
        _svgRoot = workspaceElement.Element;
        _shapes = _factory.GetShapes(_svgRoot);
        foreach (var shape in _shapes.Values)
        {
            Children.Add(shape);
            shape.MouseDown += OnMouseDown;
            shape.InvalidateMeasure();
        }
        _svgRoot.OnChange += ApplyTransformationMatrixToWpf;
        ApplyTransformationMatrixToWpf();
        InvalidateArrange();
        InvalidateMeasure();
        // CalculateViewRect();
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        // CalculateViewRect();
    }


    private void CalculateViewRect()
    {
        List<Shape> shapesList = new List<Shape>(_shapes.Values);
        if(shapesList.Count==0)
            return;
        var minLeftTopPoint = shapesList[0].RenderedGeometry.Bounds.TopLeft;
        var maxRightBottomPoint = shapesList[0].RenderedGeometry.Bounds.BottomRight;
        for (int i = 1; i < shapesList.Count; i++)
        {
            var leftTop = shapesList[i].RenderedGeometry.Bounds.TopLeft;
            var rightBottom = shapesList[i].RenderedGeometry.Bounds.TopLeft;
            if (leftTop.X < minLeftTopPoint.X || leftTop.Y < minLeftTopPoint.Y)
                minLeftTopPoint = leftTop;
            if (rightBottom.X > maxRightBottomPoint.X || rightBottom.Y > maxRightBottomPoint.Y)
                maxRightBottomPoint = rightBottom;
        }

        Bounds = new Rect(minLeftTopPoint, maxRightBottomPoint);
    }


    protected virtual void ApplyTransformationMatrixToWpf()
    {
        foreach (var element in _shapes.Keys)
        {
            var scale = element.FinalMatrix.ExtractScale();
            _shapes[element].RenderTransform = new MatrixTransform(element.FinalMatrix);
            _shapes[element].StrokeThickness *= 1 / scale.X;
            _shapes[element].InvalidateVisual();
        }
        CalculateViewRect();
    }

    public void Dispose()
    {
        _svgRoot.OnChange -= ApplyTransformationMatrixToWpf;
        foreach (var shape in _shapes)
        {
            shape.Value.MouseDown-= OnMouseDown;
        }
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        
    }

}