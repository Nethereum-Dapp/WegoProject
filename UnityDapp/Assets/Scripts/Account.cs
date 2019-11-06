using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using NBitcoin;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
    [Space]
    public string json;
    public string password;
    public string privateKey;

    [Space]
    public int gas;
    [Tooltip("Using Unit: Gwei")]
    public float gasPrice;
    [Tooltip("Using Unit :Ether")]
    public float transferAmount;
    [Tooltip("The Address You Want to Transfer Ether")]
    public string toAddress;

    [Space]
    public InputField signUpPW;
    public InputField signInPW;
    public Text passwordStrength;
    public Text passwordNotice;

    [Space]
    // public QRCodeDisplay qrcode;
    public Text addr;
    public Text balance;
    public Text tx;

    [Space]
    public GameObject seedPanel;
    public GameObject forgotPanel;
    public Text seedText;
    public InputField submitSeedText;
    public InputField submitPWText;
    public Text passwordStrength2;
    public Text missingText;

    private void Start()
    {
        IsEncryptedJson();
    }

    void Update()
    {
        if (signUpPW.isActiveAndEnabled)
        {
            if (signUpPW.text.Length > 7)
            {
                passwordStrength.enabled = false;
            }
            else
            {
                passwordStrength.enabled = true;
            }
        }

        if (forgotPanel.activeSelf)
        {
            if (submitPWText.text.Length > 7)
            {
                passwordStrength2.enabled = false;
            }
            else
            {
                passwordStrength2.enabled = true;
            }
        }
    }

    public void IsEncryptedJson()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        try
        {
            string encryptedJson = File.ReadAllText(WalletManager.Instance.jsonPath);
            json = encryptedJson;

            uiManager.creatFlag = false;
            uiManager.AccountButton();
        }
        catch (FileNotFoundException)
        {
            uiManager.creatFlag = true;
            uiManager.AccountButton();
        }
    }

    public void GetBalance()
    {
        CreateDefaultAccount();

        StartCoroutine(WalletManager.Instance.GetAccountBalance((decimal amount) =>
        {
            Debug.Log("Balance:" + amount);
            balance.text = "Balance:" + amount;
        }));
    }

    public void CreateAccount()
    {
        if(signUpPW.text.Length > 7)
        {
            password = signUpPW.text;
            WalletManager.Instance.CreateAccount(password);

            seedPanel.SetActive(true);

            // Creat mnemonic
            Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
            ExtKey hdRoot = mnemo.DeriveExtKey(password);
            seedText.text = mnemo.ToString();

            IsEncryptedJson();

            Debug.Log(mnemo);
            Debug.Log("Address:" + WalletManager.Instance.publicAddress);
            Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);
            Debug.Log("Json:" + WalletManager.Instance.encryptedJson);
            Debug.Log("Password:" + WalletManager.Instance.password);

            password = null;
            //UIFunction();
        }
    }

    public void CopySeedBt()
    {
        ClipBoard = seedText.ToString();
        seedPanel.SetActive(false);
    }

    public static string ClipBoard
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }

    public void ForgotPassword()
    {
        forgotPanel.SetActive(true);
    }

    public void SubmitSeed()
    {
        if (submitSeedText.text != null && submitPWText.text.Length > 7)
        {
            try {
                string seedWord = submitSeedText.text;
                string password = submitPWText.text;
                Mnemonic mnemo = new Mnemonic(seedWord, Wordlist.English);
                ExtKey hdRoot = mnemo.DeriveExtKey(password);

                

                CancleForgotPanel();

                Debug.Log("New password:" + password);
            }
            catch (FormatException)
            {
                missingText.enabled = true;
            }
        }
    }

    public void CancleForgotPanel()
    {
        forgotPanel.SetActive(false);
    }

    // password와 encryptedjson으로 로그인
    public void ImportAccountFromJson()
    {
        password = signInPW.text;
        WalletManager.Instance.ImportAccountFromJson(password, json);

        Debug.Log("Address:" + WalletManager.Instance.publicAddress);
        Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);
        Debug.Log("Json:" + WalletManager.Instance.encryptedJson);
        Debug.Log("Password:" + WalletManager.Instance.password);

        UIFunction();
    }

    // privateKey로 로그인
    public void ImportAccountFromPrivateKey()
    {
        WalletManager.Instance.ImportAccountFromPrivateKey(privateKey);

        Debug.Log("Address:" + WalletManager.Instance.publicAddress);
        Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);

        UIFunction();
    }

    public void TransferEther()
    {
        CreateDefaultAccount();

        HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
        HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
        HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

        TransactionInput input = WalletManager.Instance.GetTransferEtherInput(null, toAddress,
                gas, gasPrice, value);

        StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
        {
            if (result.Exception == null)
            {
                Debug.Log(result.Result);
                OpenEtherscan(result.Result);
            }
            else
            {
                throw new System.InvalidOperationException("Transfer failed");
            }
        }));
    }

    public void DeployContact()
    {
        CreateDefaultAccount();

        HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
        HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
        HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

        TransactionInput input = ContractService.Instance.GetDepolyInpute(gas, gasPrice, value, new object[] { "123" });

        StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
        {
            if (result.Exception == null)
            {
                Debug.Log(result.Result);
                OpenEtherscan(result.Result);
            }
            else
            {
                throw new System.InvalidOperationException("Transfer failed");
            }
        }));
    }

    public void TransferEtherToContract()
    {
        CreateDefaultAccount();

        HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
        HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
        HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

        TransactionInput input = ContractService.Instance.GetTransactionInput("deposit", gas, gasPrice, value, null);

        StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
        {
            if (result.Exception == null)
            {
                Debug.Log(result.Result);
                OpenEtherscan(result.Result);
            }
            else
            {
                throw new System.InvalidOperationException("Transfer failed");
            }
        }));
    }

    public void WriteToContract()
    {
        CreateDefaultAccount();

        HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
        HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
        HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

        BigInteger withdrawValue = Nethereum.Util.UnitConversion.Convert.ToWei(0.01);

        TransactionInput input = ContractService.Instance.GetTransactionInput("withdraw", gas, gasPrice, value, "123", withdrawValue);

        StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
        {
            if (result.Exception == null)
            {
                Debug.Log(result.Result);
                OpenEtherscan(result.Result);
            }
            else
            {
                throw new System.InvalidOperationException("Transfer failed");
            }
        }));
    }

    public void ReadDateFromContract()
    {
        CreateDefaultAccount();

        HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
        HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
        HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

        CallInput input = ContractService.Instance.CreateCallInput("getBalance");

        StartCoroutine(WalletManager.Instance.CallTransaction(input, (UnityRequest<string> result) =>
        {
            if (result.Exception == null)
            {
                // getbalance of contract ,unit: wei
                BigInteger balance = ContractService.Instance.DecodeDate<BigInteger>("getBalance", result.Result);
                decimal ba = Nethereum.Util.UnitConversion.Convert.FromWei(balance);

                Debug.Log(ba);
            }
            else
            {
                throw new System.InvalidOperationException("Transfer failed");
            }
        }));
    }

    private void UIFunction()
    {
        //if (qrcode)
        //    qrcode.RenderQRCode(WalletManager.Instance.publicAddress);

        //addr.text = "Address:" + WalletManager.Instance.publicAddress;
    }

    private void CreateDefaultAccount()
    {
        if (WalletManager.Instance.publicAddress == "")
            ImportAccountFromPrivateKey();
    }

    private void OpenEtherscan(string result)
    {
        if (WalletManager.Instance.URL.Contains("infura"))
        {
            tx.text = "Tx:" + result;
            string etherscan = WalletManager.Instance.URL.Replace("infura", "etherscan") + "/tx/";
            Application.OpenURL(etherscan + result);
        }
    }
}
