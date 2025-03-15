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

        [SerializeField] private Animation anim;
        
        private EventService es;
        
        public FoodObject FoodData => foodData;
        
        private void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            sr = gameObject.GetComponent<SpriteRenderer>();
            Hydrate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                es.Raise(EventNames.INVENTORY_ADD_FOOD, this, new InventoryChangeEventArgs(new []{foodData}, Enums.INVENTORY_ACTIONS.ADD_FOOD, false));
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
            anim.clip = foodData.clip;
            anim.AddClip(foodData.clip, foodData.clip.name);
            anim.wrapMode = WrapMode.Loop;
            if (anim.Play())
            {
                Debug.Log($"[{GetType().Name}] Successfully playing animation!");
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
        
    }
}