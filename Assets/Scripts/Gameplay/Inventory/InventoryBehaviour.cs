using System;
using System.Collections.Generic;
using Gameplay.Cooking.Monobehaviours;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Inventory
{
    public class InventoryBehaviour : MonoBehaviour
    {
        private EventService es;
        
        private InventoryService invService;

        [SerializeField] private GameObject foodPrefab;
        
        [SerializeField] private List<GameObject> followingFoods;
        
        private void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Add(EventNames.INVENTORY_ADD_SUCCESS, OnPickupFood());
            es.Add(EventNames.INVENTORY_REMOVE_SUCESS, OnDropFood());
            invService = ServicesLocator.Instance.Get<InventoryService>();
        }

        private void OnDestroy()
        {
            es.Remove(EventNames.INVENTORY_ADD_SUCCESS, OnPickupFood());
            es.Remove(EventNames.INVENTORY_REMOVE_SUCESS, OnDropFood());
        }

        private EventHandler OnPickupFood()
        {
            return (sender, args) =>
            {
                InventoryChangeEventArgs inventoryChangeEventArgs = args as InventoryChangeEventArgs;

                if (inventoryChangeEventArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to cast inventory event args, add failed");
                    return;
                }

                if (inventoryChangeEventArgs.action != Enums.INVENTORY_ACTIONS.ADD_FOOD)
                {
                    Debug.LogWarning($"[{GetType().Name}] Invalid action type {inventoryChangeEventArgs.action} for this event, failing");
                    return;
                }

                foreach (var food in inventoryChangeEventArgs.food)
                {
                    GameObject followFood = Instantiate(foodPrefab, transform.position, transform.rotation);
                    followFood.GetComponent<Collider2D>().enabled = false;
                    FollowBehaviour followBehaviour = followFood.AddComponent<FollowBehaviour>();
                    followBehaviour.SetTarget(gameObject);
                    followBehaviour.StartFollow();
                    FoodBehaviour foodBehaviour = followFood.GetComponent<FoodBehaviour>();
                    foodBehaviour.SetData(food);
                    foodBehaviour.Hydrate();
                    followingFoods.Add(followFood);
                }
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

                foreach (var food in inventoryChangeEventArgs.food)
                {
                    for (int i = 0; i < followingFoods.Count; ++i)
                    {
                        GameObject followFood = followingFoods[i];
                        FoodBehaviour followBehaviour = followFood.GetComponent<FoodBehaviour>();
                        if (food == followBehaviour.FoodData)
                        {
                            followingFoods.RemoveAt(i);
                            // TODO: Make sure this happens when the player is NOT touching it
                            if (inventoryChangeEventArgs.toStation)
                            {
                                followBehaviour.DestroySelf();
                            }
                            followFood.GetComponent<Collider2D>().enabled = true;
                            break;
                        }
                    }
                }
            };
        }
        
    }
}