using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Timeline.Operations;
using Timeline.Operations.Handlers;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline
{
    class Program
    {
        static void Main(string[] args)
        {
            var operationParser = new OperationParser();
            Timeline timeline = new Timeline();
            Dictionary<string, Func<Operation, string>> operationResponses = new Dictionary<string, Func<Operation, string>>
            {
                {OperationTypes.Create, genericOperation => new CreateOperationHandler().HandleOperation(genericOperation, ref timeline)},
                {OperationTypes.Update, genericOperation => new UpdateOperationHandler().HandleOperation(genericOperation, ref timeline)},
                {OperationTypes.Get, genericOperation => new GetOperationHandler().HandleOperation(genericOperation, ref timeline)},
                {OperationTypes.Delete, genericOperation => new DeleteOperationHandler().HandleOperation(genericOperation, ref timeline)},
                {OperationTypes.Latest, genericOperation => new LatestOperationHandler().HandleOperation(genericOperation, ref timeline)},
            };

            using (StreamReader reader = new StreamReader(Console.OpenStandardInput()))
            using (StreamWriter writer = new StreamWriter(Console.OpenStandardOutput()))
            while (true)
            {
                string stdin = reader.ReadLine();

                var genericOperation = operationParser.ParseOperation(stdin);

                string response;
                if (genericOperation == null)
                    response = Response.ErrResponse("Invalid input parameters provided");
                else
                {
                    if (operationResponses.ContainsKey(genericOperation.Type))
                        response = operationResponses[genericOperation.Type](genericOperation);
                    else if (genericOperation.Type == OperationTypes.Quit)
                        break;
                    else
                        response = Response.ErrResponse("Unknown request type");
                }
                Console.WriteLine(response);
                writer.WriteLine(response);
            }
        }
    }
}
