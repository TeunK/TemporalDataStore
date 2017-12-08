using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Terms;

namespace Timeline.Responses
{
    internal class ObservationResponse
    {
        public readonly Observation Observation;
        public readonly string ErrorMessage;

        public ObservationResponse(Observation observation, string errorMessage)
        {
            Observation = observation;
            ErrorMessage = errorMessage;
        }
    }
}
