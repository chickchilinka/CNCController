using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CNC_CAM.Operations
{
    public class OperationsController:INotifyPropertyChanged
    {
        private Stack<IRevokable> _revokableOperations;
        private Stack<IRevokable> _revokedOperations;
        
        public bool CanUndo { get; private set; }
        public bool CanRedo { get; private set; }
        
        public OperationsController()
        {
            _revokableOperations = new Stack<IRevokable>();
            _revokedOperations = new Stack<IRevokable>();
        }

        public void LaunchOperation(Operation operation)
        {
            if (operation is IRevokable revokable)
            {
                _revokableOperations.Push(revokable);
                _revokedOperations.Clear();
            }
            operation.Execute();
            UpdateFlags();
        }
        
        public void Undo()
        {
            if(_revokableOperations.Count==0)
                return;
            var operation = _revokableOperations.Pop();
            operation.Undo();
            _revokedOperations.Push(operation);
            UpdateFlags();
        }
        
        public void Redo()
        {
            if(_revokedOperations.Count==0)
                return;
            var operation = _revokedOperations.Pop();
            operation.Redo();
            _revokableOperations.Push(operation);
            UpdateFlags();
        }

        private void UpdateFlags()
        {
            CanUndo = _revokableOperations.Any();
            CanRedo = _revokedOperations.Any();
            OnPropertyChanged(nameof(CanRedo));
            OnPropertyChanged(nameof(CanUndo));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}