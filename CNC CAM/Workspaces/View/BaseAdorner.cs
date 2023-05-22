using System;
using System.Windows;
using System.Windows.Documents;
using CNC_CAM.Operations;
using DryIoc;

namespace CNC_CAM.Workspaces.View;

public class BaseAdorner:Adorner, IDisposable
{
    protected SignalBus _signalBus;
    protected OperationsController OperationsController;
    protected IWorkspaceElementView _workspaceElementView;
    public BaseAdorner(UIElement adornedElement) : base(adornedElement)
    {
        if(adornedElement is not IWorkspaceElementView workspaceElementView)
            return;
        _signalBus = MainScope.Instance.Container.Resolve<SignalBus>();
        OperationsController = MainScope.Instance.Container.Resolve<OperationsController>();
        _workspaceElementView = workspaceElementView;
        _workspaceElementView.Element.TransformElement.OnChange += InvalidateVisual;
    }

    public void Dispose()
    {
        _workspaceElementView.Element.TransformElement.OnChange -= InvalidateVisual;
    }
}