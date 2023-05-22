using System.Windows.Controls;

namespace CNC_CAM.Configuration.View;

public partial class MasterConfigurationGroup : UserControl
{
    public string Header { get; set; }
    public MasterConfigurationGroup()
    {
        InitializeComponent();
    }
}