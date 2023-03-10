using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CNC_CAM.Workspaces.Hierarchy.View;

public partial class HierarchyElementView : UserControl
{
    private const string IconRoot = @"/CNC CAM;component/Images/";

    public string IconPath
    {
        get => IconRoot + WorkspaceElement.HierarchyIcon;
    }

    protected WorkspaceElement WorkspaceElement;
    public HierarchyElementView(WorkspaceElement element)
    {
        WorkspaceElement = element;
        InitializeComponent();
        Label.Content = element.Name;
        Icon.Source = GetImageSourceFromResource(WorkspaceElement.HierarchyIcon);
    }


    static internal ImageSource GetImageSourceFromResource(string psResourceName)
    {
        Uri oUri = new Uri("pack://application:,,,/Images/"  + psResourceName);
        return new BitmapImage(oUri);
    }
}