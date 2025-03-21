using System;
using System.Collections.Generic;
using Gameplay.Cooking.Monobehaviours;
using Gameplay.Cooking.ScriptableObjects;
using Gameplay.Inventory;
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

        private InventoryService invService;
        
        private Dictionary<string, StationBehaviour> registeredStations = new Dictionary<string, StationBehaviour>();

        private Dictionary<string, List<FoodObject>> storedFood = new Dictionary<string, List<FoodObject>>();
        
        /// <summary>
        /// Initializes the service to be listening for the correct events
        /// </summary>
        public void Init()
        {
            invService = ServicesLocator.Instance.Get<InventoryService>();
            es = ServicesLocator.Instance.Get<EventService>();
            es.Add(EventNames.STATION_REGISTRATION_EVENT, OnRegisterStation());
            es.Add(EventNames.STATION_ACTIVATED_EVENT, OnActivateStation());
            es.Add(EventNames.STATION_SPAWN_FOOD_EVENT, OnSpawnAtStation());
        }

        ~CookingService()
        {
            es.Remove(EventNames.STATION_REGISTRATION_EVENT, OnRegisterStation());
            es.Remove(EventNames.STATION_ACTIVATED_EVENT, OnActivateStation());
            es.Remove(EventNames.STATION_SPAWN_FOOD_EVENT, OnSpawnAtStation());
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
                storedFood.Add(id, new List<FoodObject>());
                Debug.Log($"[{GetType().Name}] Successfully registered ID {id} to behaviour {behaviour}");
            };
        }

        /// <summary>
        /// Handle the activation of a station
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

                string id = activationEventArgs.id;
                FoodObject food = invService.GetFood(0);
                StationBehaviour behaviour = registeredStations[id];
                StationObject data = behaviour.StationData;
                if (food && behaviour.GetOpenSpots() > 0)
                {
                    OnPlaceStation(id, behaviour, data, food);
                    return;
                }

                if (!food)
                {
                    OnPickupStation(id);
                }

            };
        }

        private EventHandler OnSpawnAtStation()
        {
            return (sender, args) =>
            {
                StationSpawnFoodEventArgs spawnFoodEventArgs = args as StationSpawnFoodEventArgs;

                if (spawnFoodEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast food spawn args, spawn failed");
                    return;
                }

                string id = spawnFoodEventArgs.id;
                storedFood[id].Add(spawnFoodEventArgs.food);
                es.Raise(EventNames.STATION_UPDATE_FOOD_EVENT, this, new StationShowHeldFoodArgs(id, storedFood[id]));
            };
        }
        
        private void OnPlaceStation(string id, StationBehaviour behaviour, StationObject data, FoodObject food)
        {
            es.Raise(EventNames.INVENTORY_REMOVE_FOOD, this, new InventoryChangeEventArgs(new [] {food}, Enums.INVENTORY_ACTIONS.REMOVE_FOOD, true));
            storedFood[id].Add(food);
            // TODO: Make this not as exponentially-scaling
            foreach (var validRecipe in data.Recipes)
            {
                FoodObject[] ingredients = validRecipe.Ingredients;

                int matchedIngredients = 0;
                foreach (var ingredient in ingredients)
                {
                    if (storedFood[id].Contains(ingredient))
                    {
                        ++matchedIngredients;
                    }
                }

                if (matchedIngredients == ingredients.Length)
                {
                    //es.Raise(EventNames.INVENTORY_REMOVE_FOOD, behaviour, new InventoryChangeEventArgs(ingredients, Enums.INVENTORY_ACTIONS.REMOVE_FOOD));
                    //es.Raise(EventNames.INVENTORY_ADD_FOOD, behaviour, new InventoryChangeEventArgs(validRecipe.Results, Enums.INVENTORY_ACTIONS.ADD_FOOD));
                    foreach (var foodObject in ingredients)
                    {
                        storedFood[id].Remove(foodObject);
                    }
                    storedFood[id].AddRange(validRecipe.Results);
                }
            }
            
            es.Raise(EventNames.STATION_UPDATE_FOOD_EVENT, this, new StationShowHeldFoodArgs(id, storedFood[id]));
        }
        
        private void OnPickupStation(string id)
        {
            if (storedFood[id].Count > 0)
            {
                int lastIndex = storedFood[id].Count - 1;
                FoodObject lastFood = storedFood[id][lastIndex];
                storedFood[id].RemoveAt(lastIndex);
                es.Raise(EventNames.INVENTORY_ADD_FOOD, this, new InventoryChangeEventArgs(new [] {lastFood}, Enums.INVENTORY_ACTIONS.ADD_FOOD, false));
                es.Raise(EventNames.STATION_UPDATE_FOOD_EVENT, this, new StationShowHeldFoodArgs(id, storedFood[id]));
            }
        }
    }
}