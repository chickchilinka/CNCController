using System.Collections.Generic;

namespace CNC_CAD.Operations
{
    public class OperationsHistory
    {
        private List<Operation> _operations;

        public OperationsHistory()
        {
            _operations = new List<Operation>();
        }

        public void LaunchOperation(Operation operation)
        {
            _operations.Add(operation);
            operation.Execute();
        }
    }
}