using System.Collections.Generic;
namespace EventHandling
{
    public class EventDispatcher
    {

        /// <summary>Event Map </summary>
        private Dictionary<string, EventListener> dic = new Dictionary<string, EventListener>();

        /// < summary > add event listener </summary >
        /// <param name="eventType">event type </param>
        /// <param name="eventHandler">event handler</param>
        public void AddListener(string eventType, EventListener.EventHandler eventHandler)
        {
            EventListener invoker;
            if (!dic.TryGetValue(eventType, out invoker))
            {
                invoker = new EventListener();
                dic.Add(eventType, invoker);
            }
            invoker.eventHandler += eventHandler;
        }

        /// < summary > remove event listener </summary >
        /// <param name="eventType">event type </param>
        /// <param name="eventHandler">event handler</param>
        public void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
        {
            EventListener invoker;
            if (dic.TryGetValue(eventType, out invoker)) invoker.eventHandler -= eventHandler;
        }

        /// Whether < summary > already has this type of event </summary >
        /// <param name="eventType">event type </param>
        public bool HasListener(string eventType)
        {
            return dic.ContainsKey(eventType);
        }

        /// < summary > dispatch event </summary >
        /// <param name="eventType">event type </param>
        public void DispatchEvent(string eventType, params object[] args)
        {
            EventListener invoker;
            if (dic.TryGetValue(eventType, out invoker))
            {
                EventArgs evt;
                if (args == null || args.Length == 0)
                {
                    evt = new EventArgs(eventType);
                }
                else
                {
                    evt = new EventArgs(eventType, args);
                }
                invoker.Invoke(evt);
            }
        }

        /// < summary > Clean up all event listeners </summary >
        public void Clear()
        {
            foreach (EventListener value in dic.Values)
            {
                value.Clear();
            }
            dic.Clear();
        }

    }
}