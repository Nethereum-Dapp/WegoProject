using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject creatCanvas;
    public GameObject loginCanvas;
    public bool creatFlag = false;

    public void AccountButton()
    {
        if (creatFlag)
        {
            creatCanvas.SetActive(true);
            loginCanvas.SetActive(false);
            creatFlag = false;
        }
        else
        {
            creatCanvas.SetActive(false);
            loginCanvas.SetActive(true);
            creatFlag = true;
        }
    }
}
