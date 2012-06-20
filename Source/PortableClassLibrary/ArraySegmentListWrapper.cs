using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

// TODO (after VS2012 is released): Update this to inlcude IReadOnlyList<T> on .NET 4.5.

namespace ArraySegments
{
    /// <summary>
    /// A wrapper around an array segment, providing <see cref="IList{T}"/> and <see cref="System.Collections.IList"/> implementations.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the array.</typeparam>
    public sealed class ArraySegmentListWrapper<T> : IList<T>, System.Collections.IList
    {
        /// <summary>
        /// The array segment.
        /// </summary>
        private readonly ArraySegment<T> segment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySegmentListWrapper&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="segment">The array segment.</param>
        public ArraySegmentListWrapper(ArraySegment<T> segment)
        {
            this.segment = segment;
        }

        /// <summary>
        /// Gets the array segment.
        /// </summary>
        public ArraySegment<T> Segment
        {
            get { return this.segment; }
        }

        /// <summary>
        /// Gets the number of elements in this array segment.
        /// </summary>
        public int Count
        {
            get { return this.segment.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { return true; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return (this.segment.Array as System.Collections.ICollection).SyncRoot; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                Contract.Assume(this.segment.Offset + index < this.segment.Array.Length);
                return this.segment.Array[this.segment.Offset + index];
            }

            set
            {
                Contract.Assume(this.segment.Offset + index < this.segment.Array.Length);
                this.segment.Array[this.segment.Offset + index] = value;
            }
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                Contract.Assume(this.segment.Offset + index < this.segment.Array.Length);
                return this.segment.Array[this.segment.Offset + index];
            }

            [SuppressMessage("Microsoft.Contracts", "Nonnull-70-0")]
            set
            {
                Contract.Assume(this.segment.Offset + index < this.segment.Array.Length);
                this.segment.Array[this.segment.Offset + index] = (T)value;
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            throw this.NotSupported();
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            throw this.NotSupported();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw this.NotSupported();
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            throw this.NotSupported();
        }

        void ICollection<T>.Clear()
        {
            throw this.NotSupported();
        }

        void System.Collections.IList.Clear()
        {
            throw this.NotSupported();
        }

        void ICollection<T>.Add(T item)
        {
            throw this.NotSupported();
        }

        int System.Collections.IList.Add(object value)
        {
            throw this.NotSupported();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw this.NotSupported();
        }

        void System.Collections.IList.Remove(object value)
        {
            throw this.NotSupported();
        }

        int IList<T>.IndexOf(T item)
        {
            var found = this.segment.Array.Select((element, index) => new { element, index }).FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.element, item));
            if (found == null)
                return -1;
            Contract.Assume(found.index >= 0 && found.index < this.Count);
            return found.index;
        }

        [SuppressMessage("Microsoft.Contracts", "Nonnull-13-0")]
        int System.Collections.IList.IndexOf(object value)
        {
            if (!this.ObjectIsT(value))
            {
                return -1;
            }

            return (this as IList<T>).IndexOf((T)value);
        }

        bool ICollection<T>.Contains(T item)
        {
            var ret = this.Contains(item);
            Contract.Assume(!ret || this.Count > 0);
            return ret;
        }

        [SuppressMessage("Microsoft.Contracts", "Nonnull-13-0")]
        bool System.Collections.IList.Contains(object value)
        {
            if (!this.ObjectIsT(value))
            {
                return false;
            }

            return this.Contains((T)value);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Contract.Assume(arrayIndex + this.segment.Count <= array.Length);
            Array.Copy(this.segment.Array, this.segment.Offset, array, arrayIndex, this.segment.Count);
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            Contract.Assume(index + this.segment.Count <= array.Length);
            Array.Copy(this.segment.Array, this.segment.Offset, array, index, this.segment.Count);
        }

        /// <summary>
        /// Gets an enumerator which enumerates the elements in the array segment.
        /// </summary>
        /// <returns>An enumerator which enumerates the elements in the array segment.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            int end = this.segment.Offset + this.segment.Count;
            for (int i = this.segment.Offset; i != end; ++i)
                yield return this.segment.Array[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns whether or not the type of a given item indicates it is appropriate for storing in this list.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns><c>true</c> if the item is appropriate to store in this list; otherwise, <c>false</c>.</returns>
        private bool ObjectIsT(object item)
        {
            if (item is T)
            {
                return true;
            }

            if (item == null && !typeof(T).IsValueType)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an exception stating that the operation is not supported.
        /// </summary>
        /// <returns>An exception stating that the operation is not supported.</returns>
        private Exception NotSupported()
        {
            return new NotSupportedException("This operation is not supported.");
        }
    }
}
