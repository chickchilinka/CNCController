using System.Windows.Shapes;
using CNC_CAM.Base;
using CNC_CAM.SVG.Elements;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.UI.Windows;
using CNC_CAM.Workspaces.Hierarchy;
using CNC_CAM.Workspaces.Hierarchy.View;
using CNC_CAM.Workspaces.Rule;
using CNC_CAM.Workspaces.View;
using DryIoc;

namespace CNC_CAM.Workspaces;

public class WorkspaceInstaller:Installer
{
    public override void Install(IContainer container)
    {
        container.RegisterSingletonNonLazy<WPFViewFactory>();
        container.RegisterSingletonNonLazy<WorkspaceElementStorage>();
        container.RegisterSingletonNonLazy<WorkspaceElementViewFactory>();
        container.RegisterSingletonNonLazy<SelectShapeRule>();
        container.Register<IWorkspaceElementView<SvgWorkspaceElement>, SvgView>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
        container.Register<Workspace2D>(Reuse.Transient);
        container.Register<ExportWindow>(Reuse.Transient);
        container.Register<HierarchyView>(Reuse.Singleton);
    }
}