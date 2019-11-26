using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    public void MyRoom()
    {
        SceneManager.LoadScene("MyRoom");
    }

    public void Game1()
    {
        SceneManager.LoadScene("BugerGame");
    }

    public void Game2()
    {
        SceneManager.LoadScene("JumpGame");
    }

    public void Game3()
    {
        SceneManager.LoadScene("LightningGame");
    }
}
