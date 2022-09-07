namespace CNC_CAM.Operations
{
    public abstract class Operation
    {
        public string Name { get; protected set; }

        protected Operation(string name)
        {
            Name = name;
        }
        public abstract void Execute();
        public abstract void Undo();
    }
}