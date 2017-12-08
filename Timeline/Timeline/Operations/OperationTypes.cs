using System;
using System.Collections.Generic;
using System.Text;

namespace Timeline.Operations
{
    public static class OperationTypes
    {
        public static string Create => "CREATE";
        public static string Delete => "DELETE";
        public static string Get => "GET";
        public static string Latest => "LATEST";
        public static string Quit => "QUIT";
        public static string Update => "UPDATE";
        public static string Unknown => "UNKNOWN";
    }
}
