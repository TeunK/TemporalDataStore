using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;

namespace Timeline.Operations.Handlers
{
    internal class LatestOperationHandler : IOperationHandler
    {
        public string HandleOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as LatestOperation;

            if (operation?.Id == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id.Value}) doesn't exist.");

            var currentData = timeline.GetLatestObservationForId(operation.Id);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            return Response.OkResponse(currentData.Observation.Value);
        }
    }
}
