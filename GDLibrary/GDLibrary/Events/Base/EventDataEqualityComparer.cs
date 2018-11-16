using System.Collections.Generic;

namespace GDLibrary
{
    //used by the EventDispatcher to compare to events in the HashSet - remember that HashSets allow us to quickly prevent something from being added to a list/stack twice
    public class EventDataEqualityComparer : IEqualityComparer<EventData>
    {

        public bool Equals(EventData e1, EventData e2)
        {
            bool bEquals = true;

            //sometimes we don't specify ID or sender so run a test
            if (e1.ID != null && e2.ID != null)
                bEquals = e1.ID.Equals(e2.ID);

            bEquals = bEquals && e1.EventType.Equals(e2.EventType)
                    && e1.EventCategoryType.Equals(e2.EventCategoryType);

            if (e1.Sender != null && e2.Sender != null)
                bEquals = bEquals && (e1.Sender as Actor).GetID().Equals(e2.Sender as Actor);

            return bEquals;

        }

        public int GetHashCode(EventData e)
        {
            return e.GetHashCode();
        }
    }
}
