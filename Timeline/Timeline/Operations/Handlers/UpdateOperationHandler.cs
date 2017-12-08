using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;

namespace Timeline.Operations.Handlers
{
    internal class UpdateOperationHandler : IOperationHandler
    {
        public string HandleOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as UpdateOperation;

            if (operation?.Id == null || operation.Data == null || operation.Timestamp == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"No history exists for identifier '{operation.Id.Value}'");

            var currentData = timeline.GetPreviousObservationForId(operation.Id, operation.Timestamp);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            timeline.UpdateIdentity(operation.Id, operation.Timestamp, operation.Data);

            return Response.OkResponse(currentData.Observation.Value);
        }
    }
}
