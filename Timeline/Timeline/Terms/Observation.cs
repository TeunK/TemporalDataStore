using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Terms
{
    class Observation : IComparable<Observation>
    {
        public readonly string Value;

        public Observation(string value)
        {
            Value = value;
        }

        public int CompareTo(Observation other)
        {
            if (Value.Length < other.Value.Length) return -1;
            if (Value.Length == other.Value.Length) return 0;
            return 1;
        }
    }
}
