using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;

namespace Timeline.Operations.Handlers
{
    internal class CreateOperationHandler : IOperationHandler
    {
        public string HandleOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as CreateOperation;

            if (operation?.Id == null || operation.Data == null || operation.Timestamp == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id.Value}) already exists");

            timeline.AddNewIdentity(operation.Id, operation.Timestamp, operation.Data);

            return Response.OkResponse(operation.Data.Value);
        }
    }
}
