using System.Linq;
using CNC_CAM.Base;
using CNC_CAM.Configuration;
using CNC_CAM.Machine;
using CNC_CAM.Operations;
using CNC_CAM.Tools.Serialization;
using CNC_CAM.Workspaces;
using DryIoc;

namespace CNC_CAM;

public class MainScope:Scope
{
    private Container _container = new Container();
    public Container Container => _container;
    private static MainScope _instance;
    public static MainScope Instance => _instance ??= new MainScope();
    public void Install()
    {
        _instance = this;
        Container.Rules.WithoutThrowOnRegisteringDisposableTransient();
        
        _container.Register<SerializationService>(Reuse.Singleton);
        _container.Register<SignalBus>(Reuse.Singleton);
        _container.Register<WorkspaceFacade>(Reuse.Singleton);
        _container.Register<OperationsHistory>(Reuse.Singleton);
        new ConfigurationInstaller().Install(Container);
        new WorkspaceInstaller().Install(Container);
        new MachineInstaller().Install(Container);
        
        var registrations = _container.GetServiceRegistrations()
            .Where(r => r.Factory.Setup.Metadata?.Equals(ContainerExtensions.NonLazy) ?? false)
            .Select(r => _container.Resolve(r.ServiceType, r.OptionalServiceKey)).ToArray(); 
    }
}