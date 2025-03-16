using System;
using Gameplay.Cooking.ScriptableObjects;
using Gameplay.Inventory;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay.Cooking.Monobehaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FoodBehaviour : MonoBehaviour
    {
        [SerializeField] private FoodObject foodData;

        [SerializeField] private SpriteRenderer sr;

        [SerializeField] private Animator anim;
        
        private EventService es;
        
        public FoodObject FoodData => foodData;

        private bool playerInRange = false;
        
        private void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            sr = gameObject.GetComponent<SpriteRenderer>();
            Hydrate();
        }

        private void Update()
        {
            if (Input.GetKeyDown(Constants.INTERACT_BUTTON) && playerInRange)
            {
                Debug.Log($"[{GetType().Name}] Picking up food {foodData.name}");
                es.Raise(EventNames.INVENTORY_ADD_FOOD, this, new InventoryChangeEventArgs(new []{foodData}, Enums.INVENTORY_ACTIONS.ADD_FOOD, false));
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

        public void SetData(FoodObject data)
        {
            if (foodData != null)
            {
                Debug.LogWarning($"[{GetType().Name}] Food data already set, failing");
                return;
            }
            foodData = data;
        }
        
        /// <summary>
        /// Set the properties of the actual GameObject to match what they should be based on the data
        /// </summary>
        public void Hydrate()
        {
            if (foodData is null)
            {
                Debug.LogWarning($"[{GetType().Name}] Tried hydrating a null food, failing");
                return;
            }
            
            sr.sprite = foodData.sprite;
            anim.runtimeAnimatorController = foodData.controller;
            anim.Play(foodData.clip.name);
        }
        
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}