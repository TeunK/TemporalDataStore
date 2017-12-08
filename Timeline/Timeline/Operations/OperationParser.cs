using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timeline.Operations.Types;

namespace Timeline.Operations
{
    internal class OperationParser
    {
        private readonly OperationFactory _operationFactory = new OperationFactory();

        public Operation ParseOperation(string input)
        {
            var operationLine = input.Split().Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (operationLine.Count > 0)
            {
                var operationType = operationLine[0];
                if (operationType == OperationTypes.Create) return _operationFactory.GetCreateOperation(operationLine);
                if (operationType == OperationTypes.Update) return _operationFactory.GetUpdateOperation(operationLine);
                if (operationType == OperationTypes.Delete) return _operationFactory.GetDeleteOperation(operationLine);
                if (operationType == OperationTypes.Get) return _operationFactory.GetGetOperation(operationLine);
                if (operationType == OperationTypes.Latest) return _operationFactory.GetLatestOperation(operationLine);
                if (operationType == OperationTypes.Quit) return _operationFactory.GetQuitOperation();
            }

            //todo: (optionally) log invalid operation
            return null;
        }
    }
}
