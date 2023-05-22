namespace CNC_CAM.Operations;

public interface IRevokable
{
    public void Undo();
    public void Redo();
}