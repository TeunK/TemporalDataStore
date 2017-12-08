using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations.Types
{
    class QuitOperation : Operation
    {
        public QuitOperation() : base(OperationTypes.Quit)
        {
        }
    }
}
