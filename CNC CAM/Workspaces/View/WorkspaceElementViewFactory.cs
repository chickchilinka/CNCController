using System;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Workspaces.Hierarchy;
using DryIoc;

namespace CNC_CAM.Workspaces.View;

public class WorkspaceElementViewFactory
{
    private IContainer _container;
    public WorkspaceElementViewFactory(IContainer container)
    {
        _container = container;
    }

    public IWorkspaceElementView Create<TElement>(TElement element) where TElement : WorkspaceElement
    {
        var view = _container.Resolve<IWorkspaceElementView>(serviceKey:element.GetType());
        view.Initialize(element);
        return view;
    }
}