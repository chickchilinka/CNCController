using System.IO;
using System.Windows.Controls;
using System.Xml;
using CNC_CAD.Shapes;
using CNC_CAD.Tools;
using Microsoft.Win32;
using CNC_CAD.Workspaces;

namespace CNC_CAD.Operations
{
    public class LoadSvgOperation:Operation
    {
        private bool _canceled;
        private readonly Workspace _currentWorkspace;
        private SvgRoot _root;
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
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            foreach (XmlElement root in document.GetElementsByTagName("svg"))
            {
                _root = new SvgParser().Create(root);    
            }
            
            foreach (var subshape in _root.Children)
            {
                _currentWorkspace.AddShape(subshape);   
            }
        }

        public override void Undo()
        {
            if (!_canceled)
            {
                _canceled = true;
                foreach (var subshape in _root.Children)
                {
                    _currentWorkspace.RemoveShape(subshape);
                }
            }
        }
    }
}