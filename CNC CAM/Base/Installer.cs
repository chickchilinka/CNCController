using DryIoc;

namespace CNC_CAM.Base;

public abstract class Installer
{
    public abstract void Install(IContainer container);
}