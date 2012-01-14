using System;
using System.ComponentModel;

namespace AD.DomainModel
{
    /// <summary>
    /// This class implements INotifyPropertyChanged in a serialization-safe manner.
    /// </summary>
    [Serializable]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler _nonSerializableChangedHandlers;
        private PropertyChangedEventHandler _serializableChangedHandlers;

        /// <summary>
        /// Implements a serialization-safe PropertyChanged event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                    _serializableChangedHandlers = 
                        (PropertyChangedEventHandler)Delegate.Combine(_serializableChangedHandlers, value);
                else
                    _nonSerializableChangedHandlers = 
                        (PropertyChangedEventHandler)Delegate.Combine(_nonSerializableChangedHandlers, value);
            }
            remove
            {
                if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                    _serializableChangedHandlers = 
                        (PropertyChangedEventHandler)Delegate.Remove(_serializableChangedHandlers, value);
                else
                    _nonSerializableChangedHandlers = 
                        (PropertyChangedEventHandler)Delegate.Remove(_nonSerializableChangedHandlers, value);
            }
        }
 
        /// <summary>
        /// Call this method to raise the PropertyChanged event
        /// for a specific property.
        /// </summary>
        /// <param name="propertyName">Name of the property that
        /// has changed.</param>
        /// <remarks>
        /// This method may be called by properties in the business
        /// class to indicate the change in a specific property.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_nonSerializableChangedHandlers != null)
                _nonSerializableChangedHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
          
            if (_serializableChangedHandlers != null)
                _serializableChangedHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}