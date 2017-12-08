using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Timeline.Operations;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline
{
    class Program
    {
        static void Main(string[] args)
        {
            var operationParser = new OperationParser();
            var responseHandler = new ResponseHandler();

            Timeline timeline = new Timeline();

            while (true)
            {
                var line = Console.ReadLine();
                var genericOperation = operationParser.ParseOperation(line);
                string response = null;

                if (genericOperation != null)
                {
                    if (genericOperation.Type == OperationTypes.Quit) break;

                    if (genericOperation.Type == OperationTypes.Create)
                    {
                        response = responseHandler.HandleCreateOperation(genericOperation, ref timeline);
                    }
                    else if (genericOperation.Type == OperationTypes.Update)
                    {
                        response = responseHandler.HandleUpdateOperation(genericOperation, ref timeline);
                    }
                    else if (genericOperation.Type == OperationTypes.Get)
                    {
                        response = responseHandler.HandleGetOperation(genericOperation, timeline);
                    }
                    else if (genericOperation.Type == OperationTypes.Delete)
                    {
                        response = responseHandler.HandleDeleteOperation(genericOperation, ref timeline);
                    }
                    else if (genericOperation.Type == OperationTypes.Latest)
                    {
                        response = responseHandler.HandleLatestOperation(genericOperation, timeline);
                    }
                    else
                    {
                        response = responseHandler.HandleUnknownResponse();
                    }
                }
                else
                {
                    response = Response.ErrResponse("Invalid input parameters provided");
                }

                Console.WriteLine(response);
            }
        }
    }
}
