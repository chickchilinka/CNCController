using System.Collections;
using System.Collections.Generic;

namespace CNC_CAM.Machine.GCode
{
    public class GCodeCommand:IEnumerable<string>
    {
        private readonly List<string> _stringCommands;
        public string this[int i] => _stringCommands[i];
        public int Length => _stringCommands.Count;
        public int Time { get; set; }

        public GCodeCommand(List<string> stringCommands)
        {
            _stringCommands = stringCommands;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)_stringCommands).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}