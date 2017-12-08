using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline
{
    class Response
    {
        public static string OkResponse(string response)
        {
            return "OK " + response;
        }

        public static string ErrResponse(string response)
        {
            return "ERR " + response;
        }
    }
}
