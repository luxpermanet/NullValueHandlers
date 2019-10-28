using System;
using System.Collections;
using System.Collections.Generic;

namespace NullValueHandlers {
    /// <summary>
    /// Wrapper for a null or single object to avoid null checks on the caller side.
    /// <para>Use <see cref="None"/> to signal the caller that the method returned a null object.</para>
    /// <para>Use <see cref="Single"/> to signal the caller that the method returned a valid object.</para>
    /// <para>Since this type implements <see cref="IEnumerable{T}"/>, the caller may use LINQ methods to access the value.</para>
    /// </summary>
    public class Option<T> : IEnumerable<T> where T : class {
        /// <summary>
        /// Creates an <see cref="Option{T}"/> with no value.
        /// </summary>
        public static readonly Option<T> None = new Option<T>(new T[0]);

        private readonly T[] Data;

        /// <summary>
        /// Determines whether the <see cref="Option{T}"/> has value or not.
        /// </summary>
        public bool HasValue => Data.Length > 0;

        /// <summary>
        /// Gets <see cref="Option{T}"/>'s value.
        /// <para>Caution: Call for <see cref="HasValue"/> first prior to calling to this property.</para>
        /// </summary>
        /// <exception cref="InvalidOperationException">When the <see cref="Option{T}"/> has no value.</exception>
        public T Value {
            get {
                if (!HasValue) {
                    throw new InvalidOperationException("Option has no value");
                }
                return Data[0];
            }
        }

        private Option(T[] data) {
            Data = data;
        }

        /// <summary>
        /// Creates an <see cref="Option{T}"/> with a valid object.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when argument provided is a null object</exception>
        public static Option<T> Single(T value) {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return new Option<T>(new[] { value });
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)Data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
