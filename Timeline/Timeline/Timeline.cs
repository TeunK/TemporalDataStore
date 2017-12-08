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
                if (Data[id].ContainsKey(timestamp))
                    Data[id][timestamp] = observation;
                else
                    Data[id].Add(timestamp, observation);
            else
                throw new ArgumentException("id was not contained in timeline");
        }

        public void DeleteIdentity(Identifier id, Timestamp timestamp)
        {
            if (Data.ContainsKey(id))
            {
                if (timestamp == null)
                    Data.Remove(id);
                else
                {
                    Timestamp previousObservationTimestamp = BinarySearchPreviousTimestamp(Data[id], timestamp);

                    if (previousObservationTimestamp == null)
                        return;

                    if (previousObservationTimestamp.Value > 0)
                    {
                        // iterating removal backward-to-front to prevent sortedList from automatically (repeatedly) filling up removed gaps in each iteration
                        int indexAtPreviousTimestamp = Data[id].IndexOfKey(previousObservationTimestamp);
                        for (int i = Data[id].Count - 1; i > indexAtPreviousTimestamp; --i)
                            Data[id].RemoveAt(i);

                        if (Data[id].ContainsKey(timestamp))
                            Data[id].Remove(timestamp);

                        if (Data[id].Count == 0) Data.Remove(id);
                    }
                    else
                    {
                        Data.Remove(id);
                    }
                }
            }
            else throw new ArgumentException("id was not contained in timeline");
        }

        public ObservationResponse GetLatestObservationForId(Identifier id, bool ignoreTimestamp = false)
        {
            if (!Data.ContainsKey(id))
                return new ObservationResponse(null, $"No history exists for identifier '{id.Value}'");

            var sortedObservationsListForId = Data[id];

            if (sortedObservationsListForId.Count == 0)
                return new ObservationResponse(null, $"No history exists for identifier '{id.Value}'");

            var latestObservationTimestamp = Data[id].Keys[Data[id].Count - 1].Value;
            var latestObservationData = Data[id].Values[Data[id].Count - 1].Value;
            Observation latestObservation = ignoreTimestamp 
                ? new Observation(latestObservationData) 
                : new Observation($"{latestObservationTimestamp} {latestObservationData}");

            return new ObservationResponse(latestObservation, null);
        }

        public ObservationResponse GetPreviousObservationForId(Identifier id, Timestamp timestamp)
        {
            if (!Data.ContainsKey(id))
                return new ObservationResponse(null, $"No history exists for identifier '{id.Value}'");
            
            var sortedObservationsListForId = Data[id];

            if (sortedObservationsListForId.Count == 0)
                return new ObservationResponse(null, $"No history exists for identifier '{id.Value}'");

            if (timestamp.Value < sortedObservationsListForId.Keys[0].Value)
                return new ObservationResponse(null, $"No history exists for identifier '{id.Value}' before requested time '{timestamp.Value}'");

            if (timestamp.Value >= sortedObservationsListForId.Keys[sortedObservationsListForId.Count-1].Value)
                return new ObservationResponse(sortedObservationsListForId.Values[sortedObservationsListForId.Count - 1], null);

            return BinarySearchPreviousEventResponse(sortedObservationsListForId, timestamp);
        }

        private static ObservationResponse BinarySearchPreviousEventResponse(SortedList<Timestamp, Observation> sortedObservationsList, Timestamp timestamp)
        {
            Timestamp previousEventTimestamp = BinarySearchPreviousTimestamp(sortedObservationsList, timestamp);
            if (sortedObservationsList.TryGetValue(previousEventTimestamp, out Observation observation))
                return new ObservationResponse(observation, null);

            return new ObservationResponse(null, "Failed to find previously occurred event");
        }

        public static Timestamp BinarySearchPreviousTimestamp(SortedList<Timestamp, Observation> sortedObservationsList, Timestamp timestamp)
        {
            int mid;
            int first = 0;
            int last = sortedObservationsList.Count - 1;
            Timestamp result = null;

            while (first <= last)
            {
                mid = (first + last) / 2;
                Timestamp middleTimestamp = sortedObservationsList.Keys[mid];

                if (timestamp.Value == middleTimestamp.Value)
                    return timestamp;

                if (timestamp.Value < sortedObservationsList.Keys[first].Value)
                {
                    if (first > 0)
                        return sortedObservationsList.Keys[first - 1];
                    else
                        return new Timestamp(0); //edge case, return minimum possible timestamp
                }
                    
                result = sortedObservationsList.Keys[mid];

                if (timestamp.Value > middleTimestamp.Value)
                    first = mid + 1;
                if (timestamp.Value < middleTimestamp.Value)
                    last = mid - 1;
            }

            return result;
        }
    }
}
