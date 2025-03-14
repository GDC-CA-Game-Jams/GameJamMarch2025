using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
[SerializeField] TextMeshProUGUI timerText;
[SerializeField] float remainingTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
