using System;
using UnityEngine;
namespace EventHandling
{
    public class EventListener
    {

        /// < summary > event handler delegation </summary >
        public delegate void EventHandler(EventArgs eventArgs);
        /// < summary > set of event handlers </summary >
        public EventHandler eventHandler;

        /// < summary > Call all added events </summary >
        public void Invoke(EventArgs eventArgs)
        {
            if (eventHandler != null) eventHandler.Invoke(eventArgs);
        }

        /// < summary > Clean up all event delegations </summary >
        public void Clear()
        {
            eventHandler = null;
        }

    }
}