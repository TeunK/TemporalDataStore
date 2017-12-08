using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Terms
{
    class Identifier
    {
        public readonly uint Value;

        public Identifier(uint value)
        {
            Value = value;
        }

        protected bool Equals(Identifier other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }
    }
}
