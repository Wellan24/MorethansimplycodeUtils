using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace Cartif.Util
{
    public struct Optional<T> where T : class
    {
        #region Static methods

        public static readonly Optional<T> EMPTY = new Optional<T>();

        public static Optional<T> Empty { get { return EMPTY; } }

        public static Optional<T> Of(T value)
        {
            value.ThrowIfArgumentIsNull("value");
            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            if (value == null)
                return EMPTY;

            return new Optional<T>(value);
        }

        public static explicit operator T(Optional<T> optional)
        {
            return optional.Value;
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        #endregion

        #region Properties

        public bool HasValue { get; private set; }

        private T value;
        public T Value
        {
            get
            {
                if (HasValue)
                    return value;
                else
                    throw new ArgumentNullException("El valor de Optional es null");
            }
        }

        public T GetValue(T defaultValue)
        {
            return value;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? value : defaultValue;
        }

        #endregion

        #region Class

        private Optional(T value)
        {
            this.value = value;
            HasValue = true;
        }

        public bool Equals(Optional<T> other)
        {
            if (HasValue && other.HasValue)
                return object.Equals(value, other.value);
            else
                return HasValue == other.HasValue;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (this.GetHashCode() != obj.GetHashCode())
                return false;

            if (obj is Optional<T>)
                return this.Equals((Optional<T>)obj);
            else
                return false;
        }

        public override Int32 GetHashCode()
        {
            if (HasValue)
                return value.GetHashCode();
            else
                return 0;
        }

        public override String ToString()
        {
            if (HasValue)
                return value.ToString();
            else
                return "empty";
        }

        #endregion
    }
}
