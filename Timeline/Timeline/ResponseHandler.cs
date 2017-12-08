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

        public string HandleUnknownResponse()
        {
            return Response.ErrResponse("Unknown request type");
        }
    }
}
