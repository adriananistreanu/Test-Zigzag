using CodeBase.Helpers;
using UnityEngine.Events;

namespace CodeBase.Common
{
    public class EventsHolder : Singleton<EventsHolder>
    {
        public UnityEvent startEvent;
        public UnityEvent dieEvent;
    }
}
