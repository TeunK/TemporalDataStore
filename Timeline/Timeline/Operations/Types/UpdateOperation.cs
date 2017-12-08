using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class UpdateOperation : Operation
    {
        public Identifier Id { get; set; }
        public Timestamp Timestamp { get; set; }
        public Observation Data { get; set; }

        public UpdateOperation(Identifier id, Timestamp timestamp, Observation data) : base(OperationTypes.Update)
        {
            Id = id;
            Timestamp = timestamp;
            Data = data;
        }
    }
}
