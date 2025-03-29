using Gameplay.Cooking.ScriptableObjects;
using Gameplay.Inventory;
using UnityEngine;
using Util;

namespace Gameplay.Depot
{
    public class SlimeDepot : FoodDepot
    {
        private FoodObject desiredFood;

        [SerializeField] private Animator slimeAnim;
        
        [SerializeField] private Animator desiredFoodAnim;

        protected override void Start()
        {
            base.Start();
            ChooseDesiredFood();
        }
        
        protected override void ActivateDepot()
        {
            if (invService.HasFood(desiredFood))
            {
                es.Raise(EventNames.INVENTORY_REMOVE_FOOD, this, new InventoryChangeEventArgs(new []{desiredFood}, Enums.INVENTORY_ACTIONS.REMOVE_FOOD, true));
                es.Raise(EventNames.SLIME_DESIRED_FOOD_SUCCESS, this, null);
                slimeAnim.SetTrigger("Eat");
                ChooseDesiredFood();
            }
        }

        private void ChooseDesiredFood()
        {
            int random = Random.Range(0, validFoods.Length);
            desiredFood = validFoods[random];
            desiredFoodAnim.runtimeAnimatorController = desiredFood.controller;
            desiredFoodAnim.Play(desiredFood.clip.name);
        }
    }
}