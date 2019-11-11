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
        if (Account.Instance.signUpPW.isActiveAndEnabled)
        {
            if (Account.Instance.signUpPW.text.Length > 7)
            {
                passwordStrength.enabled = false;
            }
            else
            {
                passwordStrength.enabled = true;
            }
        }

        if (Account.Instance.forgotPanel.activeSelf)
        {
            if (Account.Instance.submitPWText.text.Length > 7)
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
