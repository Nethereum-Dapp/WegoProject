using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseWindow;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseWindow && !pauseWindow.activeSelf)
            pauseWindow.SetActive(false);
        else return;
    }

    public void OnPause()
    {
        pauseWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnGame()
    {
        pauseWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnMain()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

}
