using CNC_CAM.Workspaces.Hierarchy;
using CNC_CAM.Workspaces.Hierarchy.View;
using CNC_CAM.Workspaces.View;
using DryIoc;

namespace CNC_CAM.Workspaces
{
    public class WorkspaceFacade
    {
        public Workspace2D Workspace2D { get; }
        private WorkspaceElementStorage _workspaceElements;
        private HierarchyView _hierarchyView;
        private WorkspaceElement _selectedElement;

        public WorkspaceFacade(IContainer container)
        {
            _hierarchyView = container.Resolve<HierarchyView>();
            _workspaceElements = container.Resolve<WorkspaceElementStorage>();
            Workspace2D = container.Resolve<Workspace2D>();
        }

        public void Add<TElement>(TElement element) where TElement : WorkspaceElement
        {
            _workspaceElements.Add(element);
            _hierarchyView.Add(element);
            Workspace2D.AddElement(element);
        }
        
        
        public void Remove<TElement>(TElement element) where TElement : WorkspaceElement
        {
            _workspaceElements.Remove(element);
            _hierarchyView.Remove(element);
            Workspace2D.RemoveElement(element);
        }

        public void Select(WorkspaceElement element)
        {
            Workspace2D.Select(element);
        }
    }
}