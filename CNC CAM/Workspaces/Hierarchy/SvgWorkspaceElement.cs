using CNC_CAM.SVG.Elements;

namespace CNC_CAM.Workspaces.Hierarchy;

public class SvgWorkspaceElement:WorkspaceElement<SvgRoot>
{
    public override string HierarchyIcon => "Svg_Icon.png";
    public string Path;
    public SvgWorkspaceElement(string name, string path, SvgRoot element) : base(name, element)
    {
        Path = path;
    }
}