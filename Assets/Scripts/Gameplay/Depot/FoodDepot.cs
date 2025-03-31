using System;
using Gameplay.Cooking.ScriptableObjects;
using Gameplay.Inventory;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Depot
{
    public class FoodDepot : MonoBehaviour
    {
        [SerializeField] protected FoodObject[] validFoods;

        protected EventService es;

        protected InventoryService invService;

        private bool playerInRange = false;

        protected AudioSource audioSource;

        public AudioClip potSound;

        protected virtual void Start()
        {
            audioSource = GetComponent<AudioSource>();
            es = ServicesLocator.Instance.Get<EventService>();
            invService = ServicesLocator.Instance.Get<InventoryService>();
        }


        private void Update()
        {
            if (Input.GetKeyDown(Constants.INTERACT_BUTTON) && playerInRange)
            {
                ActivateDepot();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            playerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            playerInRange = false;
        }

        protected virtual void ActivateDepot()
        {
            audioSource.PlayOneShot(potSound);
            foreach (var food in validFoods)
            {
                if (invService.HasFood(food))
                {
                    es.Raise(EventNames.INVENTORY_REMOVE_FOOD, this, new InventoryChangeEventArgs(new []{food}, Enums.INVENTORY_ACTIONS.REMOVE_FOOD, true));
                }
            }
        }
    }
}