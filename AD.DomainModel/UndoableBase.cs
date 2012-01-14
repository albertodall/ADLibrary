using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace AD.DomainModel
{
    /// <summary>
    /// Implements n-level undo capabilities 
    /// </summary>
    [Serializable]
    public abstract class UndoableBase : BindableBase, IUndoableObject
    {
        // keep a stack of object state values.
        [NotUndoable] private readonly Stack<byte[]> _stateStack;

        [NotUndoable]
        private bool _bindingEdit;

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        protected UndoableBase()
        {
            _stateStack = new Stack<byte[]>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether n-level undo
        /// was invoked through IEditableObject. FOR INTERNAL USE ONLY!
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected bool BindingEdit
        {
          get { return _bindingEdit; }
          set { _bindingEdit = value; }
        }

        /// <summary>
        /// Returns the current edit level of the object.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected int EditLevel
        {
            get { return _stateStack.Count; }
        }

        #region IUndoableObject Members

        int IUndoableObject.EditLevel
        {
            get { return EditLevel; }
        }

        void IUndoableObject.CopyState(int parentEditLevel, bool parentBindingEdit)
        {
            if (!parentBindingEdit)
            {
                CopyState(parentEditLevel);
            }
        }

        void IUndoableObject.UndoChanges(int parentEditLevel, bool parentBindingEdit)
        {
            if (!parentBindingEdit)
            {
                UndoChanges(parentEditLevel);
            }
        }

        void IUndoableObject.AcceptChanges(int parentEditLevel, bool parentBindingEdit)
        {
            if (!parentBindingEdit)
            {
                AcceptChanges(parentEditLevel);
            }
        } 

        #endregion

        /// <summary>
        /// Copies the state of the object and places the copy
        /// onto the state stack.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal virtual void CopyState(int parentEditLevel)
        {
            Type currentType = GetType();
            HybridDictionary state = new HybridDictionary();

            if (EditLevel + 1 > parentEditLevel)
            {
                throw new InvalidOperationException("Error during Undo: CopyState");
            }

            do
            {
                // get the list of fields in this type
                FieldInfo[] fields = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo field in fields)
                {
                    // make sure we process only our variables
                    if (field.DeclaringType == currentType)
                    {
                        // see if this field is marked as not undoable
                        if (!NotUndoableField(field))
                        {
                            // the field is undoable, so it needs to be processed.
                            var value = field.GetValue(this);

                            if (typeof(IUndoableObject).IsAssignableFrom(field.FieldType))
                            {
                                // make sure the variable has a value
                                if (value == null)
                                {
                                    // variable has no value - store that fact
                                    state.Add(GetFieldName(field), null);
                                }
                                else
                                {
                                    // this is a child object, cascade the call
                                    ((IUndoableObject)value).CopyState(EditLevel + 1, BindingEdit);
                                }
                            }
                            else
                            {
                                // this is a normal field, simply trap the value
                                state.Add(GetFieldName(field), value);
                            }
                        }
                    }
                }
                currentType = currentType.BaseType;
            } while (currentType != typeof(UndoableBase));

            // serialize the state and stack it
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, state);
                _stateStack.Push(buffer.ToArray());
            }
        }

        /// <summary>
        /// Restores the object's state to the most recently copied values from the state stack.
        /// </summary>
        /// <remarks>
        /// Restores the state of the object to its
        /// previous value by taking the data out of
        /// the stack and restoring it into the fields
        /// of the object.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected internal virtual void UndoChanges(int parentEditLevel)
        {
            // if we are a child object we might be asked to
            // undo below the level of stacked states,
            // so just do nothing in that case
            if (EditLevel > 0)
            {
                if (EditLevel - 1 < parentEditLevel)
                {
                    throw new InvalidOperationException("Error during UndoChanges");
                }

                HybridDictionary state;
                using (MemoryStream buffer = new MemoryStream(_stateStack.Pop()))
                {
                    buffer.Position = 0;
                    BinaryFormatter formatter = new BinaryFormatter();
                    state = (HybridDictionary)formatter.Deserialize(buffer);
                }

                Type currentType = GetType();
                do
                {
                    // get the list of fields in this type
                    FieldInfo[] fields = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    foreach (FieldInfo field in fields)
                    {
                        // make sure we process only our variables
                        if (field.DeclaringType == currentType)
                        {
                            // see if the field is undoable or not
                            if (!NotUndoableField(field))
                            {
                                // the field is undoable, so restore its value
                                var value = field.GetValue(this);

                                if (typeof(IUndoableObject).IsAssignableFrom(field.FieldType))
                                {
                                    // this is a child object
                                    // see if the previous value was empty
                                    if (state.Contains(GetFieldName(field)))
                                    {
                                        // previous value was empty - restore to empty
                                        field.SetValue(this, null);
                                    }
                                    else
                                    {
                                        // make sure the variable has a value
                                        if (value != null)
                                        {
                                            // this is a child object, cascade the call.
                                            ((IUndoableObject)value).UndoChanges(EditLevel, BindingEdit);
                                        }
                                    }
                                }
                                else
                                {
                                    // this is a regular field, restore its value
                                    field.SetValue(this, state[GetFieldName(field)]);
                                }
                            }
                        }
                    }
                    currentType = currentType.BaseType;
                } while (currentType != typeof(UndoableBase));
            }
        }

    /// <summary>
    /// Accepts any changes made to the object since the last state copy was made.
    /// </summary>
    /// <remarks>
    /// The most recent state copy is removed from the state
    /// stack and discarded, thus committing any changes made
    /// to the object's state.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected internal virtual void AcceptChanges(int parentEditLevel)
    {
        if (EditLevel - 1 < parentEditLevel)
        {
            throw new InvalidOperationException("Error during AcceptChanges");
        }

        if (EditLevel > 0)
        {
            _stateStack.Pop();
            Type currentType = GetType();
            do
            {
                // get the list of fields in this type
                FieldInfo[] fields = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                {
                    // make sure we process only our variables
                    if (field.DeclaringType == currentType)
                    {
                        // see if the field is undoable or not
                        if (!NotUndoableField(field))
                        {
                            // the field is undoable so see if it is a child object
                            if (typeof(IUndoableObject).IsAssignableFrom(field.FieldType))
                            {
                                var value = field.GetValue(this);
                                // make sure the variable has a value
                                if (value != null)
                                {
                                    // it is a child object so cascade the call
                                    ((IUndoableObject)value).AcceptChanges(EditLevel, BindingEdit);
                                }
                            }
                        }
                    }
                }
                currentType = currentType.BaseType;
            } while (currentType != typeof(UndoableBase));
        }
    }

    #region Helper Functions

    private static bool NotUndoableField(MemberInfo field)
    {
      return Attribute.IsDefined(field, typeof(NotUndoableAttribute));
    }

    private static string GetFieldName(FieldInfo field)
    {
      return string.Concat(field.DeclaringType.FullName, "!", field.Name);
    }

    #endregion
  }
}
