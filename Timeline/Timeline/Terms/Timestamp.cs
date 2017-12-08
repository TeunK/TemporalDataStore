using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Terms
{
    class Timestamp
    {
        public readonly ulong Value;

        public Timestamp(ulong value)
        {
            Value = value;
        }
    }
}
