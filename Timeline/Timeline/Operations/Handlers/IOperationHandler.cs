using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;

namespace Timeline.Operations.Handlers
{
    internal interface IOperationHandler
    {
        string HandleOperation(Operation genericOperation, ref Timeline timeline);
    }
}
