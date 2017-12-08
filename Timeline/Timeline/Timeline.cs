using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Timeline.Responses;
using Timeline.Terms;

namespace Timeline
{
    internal class Timeline
    {
        public Dictionary<Identifier, SortedList> Data = new Dictionary<Identifier, SortedList>();

        public void AddNewIdentity(Identifier id, Timestamp timestamp, Observation observation)
        {
            Data.Add(id, new SortedList{{timestamp, observation}});
        }

        public void UpdateIdentity(Identifier id, Timestamp timestamp, Observation observation)
        {
            if (Data.ContainsKey(id))
                Data[id][timestamp] = observation;
            else
                throw new ArgumentException("id was not contained in timeline");
        }

        public void DeleteIdentity(Identifier id, Timestamp timestamp)
        {
            if (Data.ContainsKey(id))
            {
                if (Data[id][timestamp] == null)
                    Data.Remove(id);
                else
                {
                    // iterating removal backwards to prevent sorting from repeatedly filling up removed gaps
                    int indexAtCurrentTimestamp = Data[id].IndexOfKey(timestamp);
                    for (int i=Data[id].Count-1; i>=indexAtCurrentTimestamp; --i)
                    {
                        Data[id].RemoveAt(i);
                    }
                }
                    
            }
            else throw new ArgumentException("id was not contained in timeline");
        }

        public ObservationResponse GetLatestObservationForId(Identifier id)
        {
            if (!Data.ContainsKey(id))
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");

            var sortedObservationsListForId = Data[id];

            if (sortedObservationsListForId.Count == 0)
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");

            return new ObservationResponse((Observation) Data[id].GetByIndex(Data[id].Count - 1), null);
        }

        public ObservationResponse GetPreviousObservationForId(Identifier id, Timestamp timestamp)
        {
            if (!Data.ContainsKey(id))
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");
            
            var sortedObservationsListForId = Data[id];

            if (sortedObservationsListForId.Count == 0)
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");

            if (timestamp.Value < (ulong)sortedObservationsListForId.GetByIndex(0))
                return new ObservationResponse(null, $"No history exists for identifier '{id}' before requested time '{timestamp}'");

            return BinarySearchPreviousEvent(sortedObservationsListForId, timestamp);
        }

        private static ObservationResponse BinarySearchPreviousEvent(SortedList sortedObservationsList, Timestamp timestamp)
        {
            // Binary search through timestamps
            int mid;
            int first = 0;
            int last = sortedObservationsList.Count - 1;
            Observation result = null;

            while (first <= last)
            {
                mid = (first + last) / 2;
                Timestamp middleTimestamp = (Timestamp)sortedObservationsList.GetKey(mid);
                Observation middleObservation = (Observation)sortedObservationsList.GetByIndex(mid);

                if (timestamp.Value == middleTimestamp.Value)
                    return new ObservationResponse(middleObservation, null);

                if (mid > 0) result = (Observation)sortedObservationsList.GetByIndex(mid - 1);

                if (timestamp.Value < middleTimestamp.Value)
                    first = mid + 1;
                if (timestamp.Value > middleTimestamp.Value)
                    last = mid - 1;
            }

            if (result != null)
                return new ObservationResponse(result, null);

            return new ObservationResponse(null, "Could not find previous observation for some reason, please check broken logic in Timeline.GetPreviousObservationForId()");
        }
    }
}
