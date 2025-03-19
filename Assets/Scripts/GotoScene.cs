using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour
{
    public void OnClick(string name)
    {
        SceneManager.LoadScene(name);
    }
}


