using System.Collections.Generic;

namespace CNC_CAD.Operations
{
    public class OperationsHistory
    {
        private Stack<Operation> _operations;

        public OperationsHistory()
        {
            _operations = new Stack<Operation>();
        }

        public void LaunchOperation(Operation operation)
        {
            _operations.Push(operation);
            operation.Execute();
        }

        public void Undo()
        {
            if(_operations.Count==0)
                return;
            _operations.Pop().Undo();
        }
    }
}