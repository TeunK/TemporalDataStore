using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class GetOperation : Operation
    {
        public Identifier Id { get; set; }
        public Timestamp Timestamp { get; set; }

        public GetOperation(Identifier id, Timestamp timestamp) : base(OperationTypes.Get)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}
