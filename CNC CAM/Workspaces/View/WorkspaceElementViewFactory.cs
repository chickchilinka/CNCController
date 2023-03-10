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

    public IWorkspaceElementView<TElement> Create<TElement>(TElement element) where TElement : WorkspaceElement
    {
        var view = _container.Resolve<IWorkspaceElementView<TElement>>();
        view.Initialize(element);
        return view;
    }
}