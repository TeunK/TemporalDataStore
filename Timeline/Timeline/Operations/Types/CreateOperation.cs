using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class CreateOperation : Operation
    {
        public Identifier Id { get; set; }
        public Timestamp Timestamp { get; set; }
        public Observation Data { get; set; }

        public CreateOperation(Identifier id, Timestamp timestamp, Observation data) : base(OperationTypes.Create)
        {
            Id = id;
            Timestamp = timestamp;
            Data = data;
        }
    }
}
