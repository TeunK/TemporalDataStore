using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class LatestOperation : Operation
    {
        public Identifier Id { get; set; }

        public LatestOperation(Identifier id) : base(OperationTypes.Latest)
        {
            Id = id;
        }
    }
}
