using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class DeleteOperation : Operation
    {
        public Identifier Id { get; set; }
        public Timestamp Timestamp { get; set; }

        public DeleteOperation(Identifier id, Timestamp timestamp) : base(OperationTypes.Delete)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}
