using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Responses;
using Timeline.Terms;

namespace Timeline
{
    internal class ResponseHandler
    {
        public string HandleCreateOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as CreateOperation;

            if (operation?.Id == null || operation.Data == null || operation.Timestamp == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id}) already exists");

            SortedList<Timestamp, Observation> newTree = new SortedList<Timestamp, Observation> {
                {operation.Timestamp, operation.Data}
            };

            timeline.AddNewIdentity(operation.Id, operation.Timestamp, operation.Data);

            return Response.OkResponse(operation.Data.Value);
        }

        public string HandleUpdateOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as UpdateOperation;

            if (operation?.Id == null || operation.Data == null || operation.Timestamp == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id}) doesn't exist.");

            var currentData = timeline.GetPreviousObservationForId(operation.Id, operation.Timestamp);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            timeline.UpdateIdentity(operation.Id, operation.Timestamp, operation.Data);

            return Response.OkResponse(currentData.Observation.Value);
        }

        public string HandleGetOperation(Operation genericOperation, Timeline timeline)
        {
            var operation = genericOperation as GetOperation;

            if (operation?.Id == null || operation.Timestamp == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id}) doesn't exist.");

            var currentData = timeline.GetPreviousObservationForId(operation.Id, operation.Timestamp);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            return Response.OkResponse(currentData.Observation.Value);
        }

        public string HandleDeleteOperation(Operation genericOperation, ref Timeline timeline)
        {
            var operation = genericOperation as DeleteOperation;

            if (operation?.Id == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id}) doesn't exist.");

            var currentData = timeline.GetPreviousObservationForId(operation.Id, operation.Timestamp);

            timeline.DeleteIdentity(operation.Id, operation.Timestamp);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            return Response.OkResponse(currentData.Observation.Value);
        }

        public string HandleLatestOperation(Operation genericOperation, Timeline timeline)
        {
            var operation = genericOperation as LatestOperation;

            if (operation?.Id == null)
                return Response.ErrResponse($"The input provided for operation ({genericOperation.Type}) was invalid");

            if (!timeline.Data.ContainsKey(operation.Id))
                return Response.ErrResponse($"Item ({operation.Id}) doesn't exist.");

            var currentData = timeline.GetLatestObservationForId(operation.Id);

            if (!string.IsNullOrEmpty(currentData.ErrorMessage))
                return Response.ErrResponse(currentData.ErrorMessage);

            return Response.OkResponse(currentData.Observation.Value);
        }

        public string HandleUnknownResponse()
        {
            return Response.ErrResponse("Unknown request type");
        }
    }
}
