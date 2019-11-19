using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject creatCanvas;
    public GameObject loginCanvas;
    public Text passwordStrength;
    public Text passwordStrength2;
    public bool creatFlag = false;

    void Update()
    {
        if (AccountManager.Instance.signUpPW.isActiveAndEnabled)
        {
            if (AccountManager.Instance.signUpPW.text.Length > 7)
            {
                passwordStrength.enabled = false;
            }
            else
            {
                passwordStrength.enabled = true;
            }
        }

        if (AccountManager.Instance.forgotPanel.activeSelf)
        {
            if (AccountManager.Instance.submitPWText.text.Length > 7)
            {
                passwordStrength2.enabled = false;
            }
            else
            {
                passwordStrength2.enabled = true;
            }
        }
    }

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
