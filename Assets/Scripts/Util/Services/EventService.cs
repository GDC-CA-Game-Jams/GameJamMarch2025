using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Util.Services
{
    /// <summary>
    /// Manages events used across the program. Each must be registered with a unique name, otherwise no restrictions are in place
    /// Registered events should be unregistered when no longer needed. You can check this by testing the Action for being null
    /// Adapted from https://github.com/cuicheng11165/clr-via-csharp-4th-edition-code/blob/master/Ch11-1-EventSet.cs
    /// </summary>
    public class EventService : IService
    {
        /// <summary>
        /// The dictionary holding the Actions
        /// </summary>
        private Dictionary<string, Delegate> registeredEvents = new Dictionary<string, Delegate>();

        /// <summary>
        /// Register as listening to an event of a given name
        /// </summary>
        /// <param name="name">The name of the event to listen to</param>
        /// <param name="handler">The event to assign into the service</param>
        /// <returns>True if the registration was successful</returns>
        public void Add(string name, Delegate handler)
        {
            // Ensure the dictionary isn't getting modified elsewhere
            Monitor.Enter(registeredEvents);
            // Try getting the existing subscribers
            registeredEvents.TryGetValue(name, out Delegate d);
            // Combine the existing subscribers with the new one. If there are no existing subscribers, create the entry
            registeredEvents[name] = Delegate.Combine(d, handler);
            // Release the lock on the dictionary
            Monitor.Exit(registeredEvents);
        }

        /// <summary>
        /// Unregister the given delegate from the given name. If removing the last delegate, remove the entire mapping
        /// </summary>
        /// <param name="name">The name of the event to remove from</param>
        /// <param name="handler">The handler function to unregister</param>
        public void Remove(string name, Delegate handler)
        {
            // Lock the dictionary
            Monitor.Enter(registeredEvents);
            

            // See if there is a registered delegate with the given name
            if (registeredEvents.TryGetValue(name, out Delegate d))
            {
                // Remove the given delegate from the list of subscribers
                d = Delegate.Remove(d, handler);

                // If there are still subscribers left, update the list in the dictionary. Otherwise, remove the entry entirely
                if (d != null)
                {
                    registeredEvents[name] = d;
                }
                else
                {
                    registeredEvents.Remove(name);
                }
            }
            // Unlock the dictionary
            Monitor.Exit(registeredEvents);
        }


        /// <summary>
        /// Raise the event of the given name
        /// </summary>
        /// <param name="name">The name of the event to raise</param>
        /// <param name="sender">The object raising the event</param>
        /// <param name="e">Any arguments or information to send along with the raise</param>
        public void Raise(string name, System.Object sender, EventArgs e)
        {
            //Debug.Log($"[{GetType().Name}] Trying to raise event with name {name}");
            // Get the delegates for the given name
            Monitor.Enter(registeredEvents);
            bool foundEvent = registeredEvents.TryGetValue(name, out Delegate d);
            Monitor.Exit(registeredEvents);

            // If there are subscribers
            if (foundEvent)
            {
                //Debug.Log($"[{GetType().Name}] Found event {name}, firing for all subscribers!");
                // Invoke them, throwing an exception if there is a type mismatch between subscriber and delegate
                d.DynamicInvoke(sender, e);
            }
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}