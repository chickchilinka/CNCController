using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Xml;
using CNC_CAM.Shapes;
using CNC_CAM.Tools;
using Microsoft.Win32;
using CNC_CAM.SVG.Elements;
using CNC_CAM.SVG.Parsers;
using CNC_CAM.Workspaces;
using CNC_CAM.Workspaces.Hierarchy;

namespace CNC_CAM.Operations
{
    public class LoadSvgOperation:Operation, IRevokable
    {
        private bool _canceled;
        private readonly WorkspaceFacade _currentWorkspaceFacade;
        private List<SvgWorkspaceElement> _roots = new();
        private string _filepath;
        public LoadSvgOperation(WorkspaceFacade workspaceFacade) : base("Load SVG")
        {
            _currentWorkspaceFacade = workspaceFacade;
        }

        public override void Execute()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Изображение в формате SVG (*.svg)|*.svg|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog()==true)
            {
                _filepath = openFileDialog.FileName;
                Name += $" {_filepath}";
                if(!File.Exists(_filepath))
                    return;
                XmlReader reader = XmlReader.Create(_filepath, new XmlReaderSettings(){DtdProcessing = DtdProcessing.Parse});
                AddSvgShape(_filepath, reader);
                Redo();
            }
            else
            {
                _canceled = true;
            }
        }

        private void AddSvgShape(string path, XmlReader reader)
        {
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            foreach (XmlElement root in document.GetElementsByTagName("svg"))
            {
                var svgRoot = new SvgParser().Create(root);
                var indexOfLastSlash = path.LastIndexOf('\\') + 1;
                var workspaceElement =
                    new SvgWorkspaceElement(path.Substring(indexOfLastSlash, path.Length-indexOfLastSlash), path, svgRoot);
                _roots.Add(workspaceElement);
            }
        }

        public void Undo()
        {
            if (!_canceled)
            {
                _canceled = true;
                foreach (var element in _roots)
                {
                    _currentWorkspaceFacade.Remove(element);
                }
            }
        }

        public void Redo()
        {
            _canceled = false;
            foreach (var root in _roots)
            {
                _currentWorkspaceFacade.Add(root);   
            }
        }
    }
}