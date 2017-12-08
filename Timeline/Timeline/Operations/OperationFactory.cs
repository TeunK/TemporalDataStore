using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Operations.Types;
using Timeline.Terms;

namespace Timeline.Operations
{
    class OperationFactory
    {
        #region Abstract Creations
        public Operation GetCreateOperation(List<string> properties)
        {
            try
            {
                var id = new Identifier(Convert.ToUInt32(properties[1]));
                var timestamp = new Timestamp(Convert.ToUInt64(properties[2]));
                var data = new Observation(properties[3]);
                return GetCreateOperation(id, timestamp, data);
            }
            catch (Exception e)
            {
                //todo: return appropriate message
                return null;
            }
        }

        public Operation GetUpdateOperation(List<string> properties)
        {
            try
            {
                var id = new Identifier(Convert.ToUInt32(properties[1]));
                var timestamp = new Timestamp(Convert.ToUInt64(properties[2]));
                var data = new Observation(properties[3]);

                return GetUpdateOperation(id, timestamp, data);
            }
            catch (Exception e)
            {
                //todo: return appropriate message
                return null;
            }
        }

        public Operation GetDeleteOperation(List<string> properties)
        {
            try
            {
                var id = new Identifier(Convert.ToUInt32(properties[1]));

                Timestamp timestamp = null;
                if (properties.Count == 3)
                    timestamp = new Timestamp(Convert.ToUInt64(properties[2]));

                return GetDeleteOperation(id, timestamp);
            }
            catch (Exception e)
            {
                //todo: return appropriate message
                return null;
            }
        }

        public Operation GetGetOperation(List<string> properties)
        {
            try
            {
                var id = new Identifier(Convert.ToUInt32(properties[1]));
                var timestamp = new Timestamp(Convert.ToUInt64(properties[2]));

                return GetGetOperation(id, timestamp);
            }
            catch (Exception e)
            {
                //todo: return appropriate message
                return null;
            }
        }

        public Operation GetLatestOperation(List<string> properties)
        {
            try
            {
                var id = new Identifier(Convert.ToUInt32(properties[1]));

                return GetLatestOperation(id);
            }
            catch (Exception e)
            {
                //todo: return appropriate message
                return null;
            }
        }

        public Operation GetQuitOperation()
        {
            return new QuitOperation();
        }

        public Operation GetUnknownOperation()
        {
            return new UnknownOperation();
        }
        #endregion


        #region Concrete Creations
        private Operation GetCreateOperation(Identifier id, Timestamp timestamp, Observation data)
        {
            return new CreateOperation(id, timestamp, data);
        }

        private Operation GetUpdateOperation(Identifier id, Timestamp timestamp, Observation data)
        {
            return new UpdateOperation(id, timestamp, data);
        }

        private Operation GetDeleteOperation(Identifier id, Timestamp timestamp)
        {
            return new DeleteOperation(id, timestamp);
        }

        private Operation GetGetOperation(Identifier id, Timestamp timestamp)
        {
            return new GetOperation(id, timestamp);
        }

        private Operation GetLatestOperation(Identifier id)
        {
            return new LatestOperation(id);
        }
        #endregion
    }
}
