using System;

namespace EventHandling
{
    public static class EventUtils
    {
        /// < summary > event dispatcher </summary >
        private static EventDispatcher dispatcher = new EventDispatcher();

        /// < summary > add event listener </summary >
        /// <param name="eventType">event type </param>
        /// <param name="eventHandler">event handler</param>
        public static void AddListener(string eventType, EventListener.EventHandler eventHandler)
        {
            dispatcher.AddListener(eventType, eventHandler);
        }

        /// < summary > remove event listener </summary >
        /// <param name="eventType">event type </param>
        /// <param name="eventHandler">event handler</param>
        public static void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
        {
            dispatcher.RemoveListener(eventType, eventHandler);
        }

        /// Whether < summary > already has this type of event </summary >
        /// <param name="eventType">event type </param>
        public static bool HasListener(string eventType)
        {
            return dispatcher.HasListener(eventType);
        }

        /// < summary > dispatch event </summary >
        /// <param name="eventType">event type </param>
        public static void DispatchEvent(string eventType, params object[] args)
        {
            dispatcher.DispatchEvent(eventType, args);
        }

        /// < summary > Clean up all event listeners </summary >
        public static void Clear()
        {
            dispatcher.Clear();
        }
    }
}