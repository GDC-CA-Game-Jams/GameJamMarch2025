using System;
using System.Collections.Generic;
using Gameplay.Cooking.Monobehaviours;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Cooking
{
    /// <summary>
    /// Handles the cooking and food-related aspects of the game. Recieves registrations from cooking stations
    /// and processes their activations
    /// </summary>
    public class CookingService : IService
    {
        
        private EventService es;

        private Dictionary<String, StationBehaviour> registeredStations = new Dictionary<string, StationBehaviour>();
        
        /// <summary>
        /// Initializes the service to be listening for the correct events
        /// </summary>
        public void Init()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Add(EventNames.STATION_REGISTRATION_EVENT, OnRegisterStation());
            es.Add(EventNames.STATION_ACTIVATED_EVENT, OnActivateStation());
        }

        /// <summary>
        /// Registers a station with the CookingService
        /// </summary>
        /// <returns>Event that handles station registration</returns>
        private EventHandler OnRegisterStation()
        {
            return (sender, args) =>
            {
                StationBehaviour behaviour = sender as StationBehaviour;
                StationRegistrationEventArgs registrationEventArgs = args as StationRegistrationEventArgs;

                if (behaviour is null || registrationEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast registration behaviour or args, registration failed");
                    return;
                }

                string id = registeredStations.Count.ToString();
                behaviour.SetId(id);
                registeredStations.Add(id, behaviour);
                Debug.Log($"[{GetType().Name}] Successfully registered ID {id} to behaviour {behaviour}");
            };
        }

        /// <summary>
        /// Handle the activation of a station
        /// TODO: Read player inventory, determine if anything can be done, and do it
        /// </summary>
        /// <returns>Event to run when a station is activated</returns>
        private EventHandler OnActivateStation()
        {
            return (sender, args) =>
            {
                StationActivationEventArgs activationEventArgs = args as StationActivationEventArgs;

                if (activationEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast activation event args, activation failed");
                    return;
                }

                StationBehaviour behaviour = registeredStations[activationEventArgs.id];
            };
        }
    }
}