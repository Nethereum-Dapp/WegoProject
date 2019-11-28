using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject creatCanvas;
    public GameObject loginCanvas;
    public Image isCapsLock;
    public Text passwordStrength;
    public Text passwordStrength2;
    public bool creatFlag = false;

    [System.Runtime.InteropServices.DllImport("USER32.dll")]
    public static extern short GetKeyState(int nVirtKey);
    bool IsCapsLockOn => (GetKeyState(0x14) & 1) > 0;

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

        if (IsCapsLockOn)
        {
            isCapsLock.enabled = true;
        }
        else
        {
            isCapsLock.enabled = false;
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
