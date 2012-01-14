using System;
using System.ComponentModel;

namespace AD.DomainModel
{
    [Serializable]
    public abstract class UndoableDomainObject<TIdentity> : UndoableBase, IEditableObject, IGenericEntity<TIdentity>
    {
        [NotUndoable]
        private bool _disableBindingEdit;
        private int? _requestedHashCode;

        protected UndoableDomainObject()
        {
            _disableBindingEdit = false;
        }

        #region IEditableObject Members

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool DisableBindingEdit
        {
            get { return _disableBindingEdit; }
            set { _disableBindingEdit = value; }
        }

        void IEditableObject.BeginEdit()
        {
            if (!_disableBindingEdit && !BindingEdit)
            {
                BindingEdit = true;
                StartEdit();
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (!_disableBindingEdit && BindingEdit)
            {
                CancelEdit();
            }
        }

        void IEditableObject.EndEdit()
        {
            if (!_disableBindingEdit && BindingEdit)
            {
                ApplyEdit();
            }
        }

        #endregion

        /// <summary>
        /// Starts a nested edit on the object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this method is called the object takes a snapshot of
        /// its current state (the values of its variables). This snapshot
        /// can be restored by calling CancelEdit
        /// or committed by calling ApplyEdit.
        /// </para><para>
        /// This is a nested operation. Each call to BeginEdit adds a new
        /// snapshot of the object's state to a stack. You should ensure that 
        /// for each call to BeginEdit there is a corresponding call to either 
        /// CancelEdit or ApplyEdit to remove that snapshot from the stack.
        /// </para><para>
        /// See Chapters 2 and 3 for details on n-level undo and state stacking.
        /// </para>
        /// </remarks>
        public virtual void StartEdit()
        {
            CopyState(EditLevel + 1);
        }

        /// <summary>
        /// Cancels the current edit process, restoring the object's state to
        /// its previous values.
        /// </summary>
        /// <remarks>
        /// Calling this method causes the most recently taken snapshot of the 
        /// object's state to be restored. This resets the object's values
        /// to the point of the last BeginEdit call.
        /// </remarks>
        public virtual void CancelEdit()
        {
            UndoChanges(EditLevel - 1);
            BindingEdit = false;
        }

        /// <summary>
        /// Commits the current edit process.
        /// </summary>
        /// <remarks>
        /// Calling this method causes the most recently taken snapshot of the 
        /// object's state to be discarded, thus committing any changes made
        /// to the object's state since the last BeginEdit call.
        /// </remarks>
        public virtual void ApplyEdit()
        {
            AcceptChanges(EditLevel - 1);
            BindingEdit = false;
        }
    
        #region IGenericDomainObject<TIdentity> Members

        public abstract TIdentity Id { get; protected set; }

        #endregion

        #region IEquatable<IGenericDomainObject<TIdentity>> Members

        public bool  Equals(IGenericEntity<TIdentity> other)
        {
            if (null == other || !GetType().IsInstanceOfType(other))
            {
                return false;
            }

            if (ReferenceEquals(this, other)) { return true; }
            var otherIsTransient = Equals(other.Id, default(TIdentity));
            var thisIsTransient = IsTransient();

            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }
            return other.Id.Equals(Id);
        }

        #endregion

        protected bool IsTransient()
        {
            return Equals(Id, default(TIdentity));
        }

        public override bool Equals(object obj)
        {
            var that = obj as IGenericEntity<TIdentity>;
            return Equals(that);
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
            }
            return _requestedHashCode.Value;
        }

        public static bool operator ==(UndoableDomainObject<TIdentity> left, UndoableDomainObject<TIdentity> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UndoableDomainObject<TIdentity> left, UndoableDomainObject<TIdentity> right)
        {
            return !Equals(left, right);
        }
    }
}
