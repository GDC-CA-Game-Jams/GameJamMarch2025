using System;
using Gameplay.Cooking;
using TMPro;
using UnityEngine;
using Util;
using Util.Services;

namespace Gameplay
{
    public class ScoreBehaviour : MonoBehaviour
    {
        [SerializeField] private int scorePerFood = 1;

        [SerializeField] private TMP_Text scoreText;
        
        private int score;
        
        private EventService es;
        public void Start()
        {
            es = ServicesLocator.Instance.Get<EventService>();
            es.Add(EventNames.SLIME_DESIRED_FOOD_SUCCESS, OnCorrectFoodDelivered());
        }

        private void OnDisable()
        {
            es.Remove(EventNames.SLIME_DESIRED_FOOD_SUCCESS, OnCorrectFoodDelivered());
        }
        
        private EventHandler OnCorrectFoodDelivered()
        {
            return (sender, args) =>
            {
                FoodObjectEventArgs foodObjectEventArgs = args as FoodObjectEventArgs;

                if (foodObjectEventArgs is null)
                {
                    Debug.LogError($"{GetType().Name} Failed to cast to RecipieEventArgs, how did this happen? Failing!");
                    return;
                }

                score += foodObjectEventArgs.food.PointsGained;
                ServicesLocator.Instance.Get<GameManager>().SetScore(score);
                scoreText.text = score.ToString();
            };
        }
        
    }
}