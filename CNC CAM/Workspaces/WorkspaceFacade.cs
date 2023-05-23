using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CNC_CAM.Workspaces.Hierarchy;
using CNC_CAM.Workspaces.Hierarchy.View;
using CNC_CAM.Workspaces.View;
using DryIoc;
using IContainer = DryIoc.IContainer;

namespace CNC_CAM.Workspaces
{
    public class WorkspaceFacade:INotifyPropertyChanged
    {
        public WorkspaceView WorkspaceView { get; }
        public HierarchyView HierarchyView { get; }

        public bool IsNotEmpty { get; private set; }

        private WorkspaceElementStorage _workspaceElements;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public WorkspaceFacade(IContainer container)
        {
            HierarchyView = container.Resolve<HierarchyView>();
            _workspaceElements = container.Resolve<WorkspaceElementStorage>();
            WorkspaceView = container.Resolve<WorkspaceView>();
        }

        public void Add<TElement>(TElement element) where TElement : WorkspaceElement
        {
            _workspaceElements.Add(element);
            HierarchyView.Add(element);
            WorkspaceView.AddElement(element);
            UpdateIsEmpty();
        }

        public IEnumerable<WorkspaceElement> GetElements()
        {
            return _workspaceElements;
        }
        
        public void Remove<TElement>(TElement element) where TElement : WorkspaceElement
        {
            _workspaceElements.Remove(element);
            HierarchyView.Remove(element);
            WorkspaceView.RemoveElement(element);
            UpdateIsEmpty();
        }

        private void UpdateIsEmpty()
        {
            IsNotEmpty = _workspaceElements.Any();
            OnPropertyChanged(nameof(IsNotEmpty));
        }
        
        public void Select(WorkspaceElement element)
        {
            WorkspaceView.Select(element);
        }

        
    }
}