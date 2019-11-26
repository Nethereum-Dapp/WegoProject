using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip; // 툴팁 창
    public Text tooltipTextUI; // 툴팁의 글씨 UI
    public string tooltipText; // 툴팁에 띄울 글씨


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

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 400);
        tooltipTextUI.text = TextEnterLine(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    private string TextEnterLine(string text)
    {
        text = text.Replace("\\n", "\n");
        return text;
    }

}
