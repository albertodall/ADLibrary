using System.Reflection;

namespace AD.Windows.Forms.EditableAdapter
{
    /// <summary>
    /// An interface for editable wrapper objects so that they can be reflected at design time. 
    /// </summary>
    internal interface IEditable
    {
        object WrappedInstance { get; }
        object ReadProperty(PropertyInfo property);
        void WriteProperty(PropertyInfo property, object value);
    }

}
