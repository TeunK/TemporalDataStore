using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Terms
{
    class Timestamp : IComparable<Timestamp>
    {
        public readonly ulong Value;

        public Timestamp(ulong value)
        {
            Value = value;
        }

        public int CompareTo(Timestamp other)
        {
            if (Value < other.Value) return -1;
            if (Value == other.Value) return 0;
            return 1;
        }
    }
}
