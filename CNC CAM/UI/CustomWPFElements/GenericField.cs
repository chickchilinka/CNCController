using System.ComponentModel;

namespace CNC_CAM.UI.CustomWPFElements
{
    public class GenericField<T>:LabeledField 
    {
        public T FloatValue
        {
            get
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if(converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(Value);
                }
                return default(T);
            } 
        } 
    }
}