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
        public Dictionary<Identifier, SortedList<Timestamp, Observation>> Data = new Dictionary<Identifier, SortedList<Timestamp, Observation>>();

        public void AddNewIdentity(Identifier id, Timestamp timestamp, Observation observation)
        {
            Data.Add(id, new SortedList<Timestamp, Observation> { { timestamp, observation } });
        }

        public void UpdateIdentity(Identifier id, Timestamp timestamp, Observation observation)
        {
            if (Data.ContainsKey(id))
                Data[id].Add(timestamp, observation);
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

            return new ObservationResponse(Data[id].Values[Data[id].Count - 1], null);
        }

        public ObservationResponse GetPreviousObservationForId(Identifier id, Timestamp timestamp)
        {
            if (!Data.ContainsKey(id))
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");
            
            var sortedObservationsListForId = Data[id];

            if (sortedObservationsListForId.Count == 0)
                return new ObservationResponse(null, $"No history exists for identifier '{id}'");

            if (timestamp.Value < sortedObservationsListForId.Keys[0].Value)
                return new ObservationResponse(null, $"No history exists for identifier '{id}' before requested time '{timestamp}'");

            if (timestamp.Value >= sortedObservationsListForId.Keys[sortedObservationsListForId.Count-1].Value)
                return new ObservationResponse(sortedObservationsListForId.Values[sortedObservationsListForId.Count - 1], null);

            return BinarySearchPreviousEvent(sortedObservationsListForId, timestamp);
        }

        private static ObservationResponse BinarySearchPreviousEvent(SortedList<Timestamp, Observation> sortedObservationsList, Timestamp timestamp)
        {
            // Binary search through timestamps
            int mid;
            int first = 0;
            int last = sortedObservationsList.Count - 1;
            Observation result = null;

            while (first <= last)
            {
                mid = (first + last) / 2;
                Timestamp middleTimestamp = sortedObservationsList.Keys[mid];
                Observation middleObservation = sortedObservationsList.Values[mid];

                if (timestamp.Value == middleTimestamp.Value)
                    return new ObservationResponse(middleObservation, null);

                result = sortedObservationsList.Values[mid];

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
