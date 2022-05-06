using System.IO;
using System.Windows.Controls;
using System.Xml;
using CNC_CAD.Shapes;
using Microsoft.Win32;
using CNC_CAD.Workspaces;

namespace CNC_CAD.Operations
{
    public class LoadSvgOperation:Operation
    {
        private bool _canceled;
        private readonly Workspace _currentWorkspace;
        private Shape _shape;
        public LoadSvgOperation(Workspace workspace) : base("Load SVG")
        {
            _currentWorkspace = workspace;
        }

        public override void Execute()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Scalable vector graphics (*.svg)|*.svg|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog()==true)
            {
                var filePath = openFileDialog.FileName;
                Name += $" {filePath}";
                XmlReader reader = XmlReader.Create(filePath);
                AddSvgShape(reader);
            }
            else
            {
                _canceled = true;
            }
        }

        private void AddSvgShape(XmlReader reader)
        {
            _shape = new SvgShape(reader);
            _currentWorkspace.AddShape(_shape);
        }

        public override void Undo()
        {
            if (!_canceled)
            {
                
            }
        }
    }
}