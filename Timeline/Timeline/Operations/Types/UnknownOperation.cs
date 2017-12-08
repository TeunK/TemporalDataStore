using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Operations.Types
{
    class UnknownOperation : Operation
    {
        public UnknownOperation() : base(OperationTypes.Unknown)
        {
        }
    }
}
