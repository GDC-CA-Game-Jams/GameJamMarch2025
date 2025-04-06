using Gameplay.Cooking;
using Gameplay.Cooking.ScriptableObjects;
using Gameplay.Inventory;
using UnityEngine;
using Util;

namespace Gameplay.Depot
{
    public class SlimeDepot : FoodDepot
    {
        private FoodObject desiredFood;


        public AudioClip slimeEatSound;

        public Patrol patrol;

        Animator slimeAnim;

        [SerializeField] private Animator desiredFoodAnim;

        protected override void Start()
        {
            slimeAnim = GetComponent<Animator>();
            base.Start();
            ChooseDesiredFood();
        }
        
        protected override void ActivateDepot()
        {
            if (invService.HasFood(desiredFood))
            {
                es.Raise(EventNames.INVENTORY_REMOVE_FOOD, this, new InventoryChangeEventArgs(new []{desiredFood}, Enums.INVENTORY_ACTIONS.REMOVE_FOOD, true));
                es.Raise(EventNames.SLIME_DESIRED_FOOD_SUCCESS, this, null);
                //slimeAnim.SetTrigger("Eat");
                slimeAnim.Play("Idle");
                
                patrol.Eating();
                audioSource.PlayOneShot(slimeEatSound);
                ChooseDesiredFood();
            }
        }

        private void ChooseDesiredFood()
        {
            int random = Random.Range(0, validFoods.Length);
            desiredFood = validFoods[random];
            desiredFoodAnim.runtimeAnimatorController = desiredFood.controller;
            desiredFoodAnim.Play(desiredFood.clip.name);
            es.Raise(EventNames.SLIME_NEW_DESIRED_FOOD, this, new FoodObjectEventArgs(desiredFood));
        }

        public void SlimeFinishesEating()
        {
            print("Slime Ate The Food");

        }
    }
}