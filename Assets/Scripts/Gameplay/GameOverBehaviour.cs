using System;
using TMPro;
using UnityEngine;
using Util.Services;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameOverBehaviour : MonoBehaviour
    {
        [SerializeField] private string[] badReviews;
        [SerializeField] private string[] mediumReviews;
        [SerializeField] private string[] goodReviews;

        [Tooltip("Any lower than this is considered bad")]
        [SerializeField] private int mediumCutoff;
        [Tooltip("Any lower than this is considered medium")]
        [SerializeField] private int goodCutoff;

        [SerializeField] private TMP_Text reviewText;
        [SerializeField] private TMP_Text scoreText;
        
        private void Start()
        {
            int score = ServicesLocator.Instance.Get<GameManager>().Score;

            if (score >= goodCutoff)
            {
                reviewText.text = goodReviews[Random.Range(0, goodReviews.Length - 1)];
            }
            else if (score >= mediumCutoff)
            {
                reviewText.text = mediumReviews[Random.Range(0, mediumReviews.Length - 1)];
            }
            else
            {
                reviewText.text = badReviews[Random.Range(0, badReviews.Length - 1)];
            }

            scoreText.text = score.ToString();
        }

        private void OnDestroy()
        {
            ServicesLocator.Instance.Unregister<GameManager>();
        }
    }
}