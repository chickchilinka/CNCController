using System.ComponentModel;

namespace CNC_CAM.UI.CustomWPFElements
{
    public interface IGenericField
    {
        public object GenericValueObject { get; }
        public string FieldName { get; }
    }
    public interface IGenericField<out T>:IGenericField
    {
        public T GenericValue { get; }
        
    }
    public class GenericField<T>:LabeledField, IGenericField<T>
    {
        public T GenericValue
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

        public object GenericValueObject => GenericValue;
    }
}