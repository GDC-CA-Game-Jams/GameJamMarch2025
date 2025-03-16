using System;
using System.Collections.Generic;
using Gameplay.Cooking.ScriptableObjects;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Cooking.Monobehaviours
{
    public class StationBehaviour : MonoBehaviour
    {
        [SerializeField] private StationObject stationData;

        [SerializeField] private GameObject[] openSpots;

        private EventService es;

        private string id = "";

        public StationObject StationData => stationData;

        private bool playerInRange = false;
        
        private void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Raise(EventNames.STATION_REGISTRATION_EVENT, this, new StationRegistrationEventArgs(stationData, openSpots.Length));
            es.Add(EventNames.STATION_UPDATE_FOOD_EVENT, OnShowHeldFood());
        }

        private void OnDisable()
        {
            es.Remove(EventNames.STATION_UPDATE_FOOD_EVENT, OnShowHeldFood());
        }
        
        public void SetId(string id)
        {
            if (id == "")
            {
                Debug.LogWarning($"[{GetType().Name}] Station ID already set, not re-setting");
                return;
            }
            this.id = id;
        }

        public int GetOpenSpots()
        {
            int count = 0;

            foreach (var spot in openSpots)
            {
                if (!spot.activeSelf)
                {
                    ++count;
                }
            }

            return count;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(Constants.INTERACT_BUTTON) && playerInRange)
            {
                Debug.Log($"[{GetType().Name}] Activating Station {id}");
                es.Raise(EventNames.STATION_ACTIVATED_EVENT, this, new StationActivationEventArgs(id));
            }
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }

        private EventHandler OnShowHeldFood()
        {
            return (sender, args) =>
            {
                StationShowHeldFoodArgs foodArgs = args as StationShowHeldFoodArgs;

                if (foodArgs is null)
                {
                    Debug.LogError($"[{GetType().Name}] Unable to cast to StationShowHeldFoodArgs, failing");
                    return;
                }

                if (foodArgs.id != id)
                {
                    //Debug.Log($"[{GetType().Name}] ID {id} does not match station, ignoring ShowHeldFood event");
                    return;
                }
                
                // TODO: There has to be a better way to do this, but I can't be darned to figure it out right now
                int i = 0;
                for (; i < foodArgs.heldFood.Count; ++i)
                {
                    openSpots[i].GetComponent<SpriteRenderer>().sprite = foodArgs.heldFood[i].sprite;
                    openSpots[i].SetActive(true);
                }

                for (; i < openSpots.Length; ++i)
                {
                    openSpots[i].SetActive(false);
                }
            };
        }
    }
}