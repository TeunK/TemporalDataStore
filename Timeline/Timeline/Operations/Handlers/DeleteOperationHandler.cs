using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Responses;

namespace Timeline.Operations.Handlers
{
    internal class DeleteOperationHandler : IOperationHandler
    {
        public string HandleOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as DeleteOperation;

            if (operation?.Id == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"No history exists for identifier '{operation.Id.Value}'");

            ObservationResponse currentData;
            if (operation.Timestamp == null)
                currentData = timeline.GetLatestObservationForId(operation.Id, true);
            else
                currentData = timeline.GetPreviousObservationForId(operation.Id, operation.Timestamp);

            timeline.DeleteIdentity(operation.Id, operation.Timestamp);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            return Response.OkResponse(currentData.Observation.Value);
        }
    }
}
