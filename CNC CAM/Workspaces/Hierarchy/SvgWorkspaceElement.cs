using CNC_CAM.SVG.Elements;

namespace CNC_CAM.Workspaces.Hierarchy;

public class SvgWorkspaceElement:WorkspaceElement<SvgRoot>
{
    public override string HierarchyIcon => "Import.png";
    public string Path;
    public SvgWorkspaceElement(string name, string path, SvgRoot transform) : base(name, transform)
    {
        Path = path;
    }
}