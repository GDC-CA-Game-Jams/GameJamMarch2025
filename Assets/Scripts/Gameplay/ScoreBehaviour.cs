using System;
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
                score += scorePerFood;
                scoreText.text = score.ToString();
            };
        }
        
    }
}