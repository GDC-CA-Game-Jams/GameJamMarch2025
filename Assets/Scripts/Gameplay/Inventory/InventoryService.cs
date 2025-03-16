using System;
using System.Collections.Generic;
using Gameplay.Cooking.Monobehaviours;
using Gameplay.Cooking.ScriptableObjects;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Inventory
{
    public class InventoryService : IService
    {
        private List<FoodObject> carryingFood = new List<FoodObject>();

        private EventService es;
        
        public void Init()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Add(EventNames.INVENTORY_ADD_FOOD, OnPickupFood());
            es.Add(EventNames.INVENTORY_REMOVE_FOOD, OnDropFood());
        }

        ~InventoryService()
        {
            es.Remove(EventNames.INVENTORY_ADD_FOOD, OnPickupFood());
            es.Remove(EventNames.INVENTORY_REMOVE_FOOD, OnDropFood());
        }
        
        private EventHandler OnPickupFood()
        {
            return (sender, args) =>
            {
                InventoryChangeEventArgs inventoryChangeEventArgs = args as InventoryChangeEventArgs;
                
                if (inventoryChangeEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast inventory event args, pickup failed");
                    return;
                }
                
                if (inventoryChangeEventArgs.action != Enums.INVENTORY_ACTIONS.ADD_FOOD)
                {
                    Debug.LogWarning($"[{GetType().Name}] Invalid action type {inventoryChangeEventArgs.action} for this event, failing");
                    return;
                }

                if (carryingFood.Count >= Constants.MAX_FOOD_CARRY)
                {
                    Debug.Log($"[{GetType().Name}] Player already carrying maximum allowed food {Constants.MAX_FOOD_CARRY}");
                    return;
                }

                if (sender.GetType() == typeof(FoodBehaviour))
                {
                    FoodBehaviour behaviour = sender as FoodBehaviour;
                    behaviour.DestroySelf();
                }
                
                carryingFood.AddRange(inventoryChangeEventArgs.food);
                es.Raise(EventNames.INVENTORY_ADD_SUCCESS, this, inventoryChangeEventArgs);
            };
        }

        private EventHandler OnDropFood()
        {
            return (sender, args) =>
            {
                InventoryChangeEventArgs inventoryChangeEventArgs = args as InventoryChangeEventArgs;

                if (inventoryChangeEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast inventory event args, drop failed");
                    return;
                }

                if (inventoryChangeEventArgs.action != Enums.INVENTORY_ACTIONS.REMOVE_FOOD)
                {
                    Debug.LogWarning($"[{GetType().Name}] Invalid action type {inventoryChangeEventArgs.action} for this event, failing");
                    return;
                }

                FoodObject[] toRemove = inventoryChangeEventArgs.food;
                foreach (var food in toRemove)
                {
                    carryingFood.Remove(food);
                }
                es.Raise(EventNames.INVENTORY_REMOVE_SUCESS, this, inventoryChangeEventArgs);
            };
        }

        public bool HasFood(FoodObject food)
        {
            return carryingFood.Contains(food);
        }
        
        public FoodObject GetFood(int pickupOrder)
        {
            if (pickupOrder < carryingFood.Count)
            {
                return carryingFood[pickupOrder];
            }

            return null;
        }
    }
}