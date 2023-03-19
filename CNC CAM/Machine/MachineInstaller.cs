using CNC_CAM.Base;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Machine.Rule;
using DryIoc;

namespace CNC_CAM.Machine;

public class MachineInstaller:Installer
{
    public override void Install(IContainer container)
    {
        container.Register<DummyCncController2D>(Reuse.Singleton);
        container.Register<SimpleCncSerialController2D>(Reuse.Singleton);
        container.RegisterSingletonNonLazy<ExportShapesRule>();
    }
}