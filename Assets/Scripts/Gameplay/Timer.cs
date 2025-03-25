using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Util;
using Util.Services;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] private float correctFoodTimeGain;

    private EventService es;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        es = ServicesLocator.Instance.Get<EventService>();
        es.Add(EventNames.SLIME_DESIRED_FOOD_SUCCESS, OnCorrectFoodDelivered());
    }

    private void OnDisable()
    {
        es.Remove(EventNames.SLIME_DESIRED_FOOD_SUCCESS, OnCorrectFoodDelivered());
    }

    // Update is called once per frame
    void Update()
    {
        
        if(remainingTime > 0)
        {
        remainingTime -= Time.deltaTime;
        }
        else
        {
        remainingTime = 0;
        //end the game
        SceneManager.LoadScene("GameOver");
        }



        int minutes = Mathf.FloorToInt(remainingTime /60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private EventHandler OnCorrectFoodDelivered()
    {
        return (sender, args) =>
        {
            remainingTime += correctFoodTimeGain;
        };
    }
}
