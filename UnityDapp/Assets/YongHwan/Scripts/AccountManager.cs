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
using Nethereum.HdWallet;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance = null;

    [Space]
    public string json;
    public string password;

    //public string privateKey;

    //[Space]
    //public int gas;
    //[Tooltip("Using Unit: Gwei")]
    //public float gasPrice;
    //[Tooltip("Using Unit :Ether")]
    //public int transferAmount;
    //[Tooltip("The Address You Want to Transfer Ether")]
    //public string toAddress;

    [Space]
    public InputField signUpPW;
    public InputField signInPW;
    public Text passwordNotice;

    [Space]
    // public QRCodeDisplay qrcode;
    //public Text addr;
    //public Text balance;
    //public Text tx;

    [Space]
    public GameObject seedPanel;
    public GameObject forgotPanel;
    public Text seedText;
    public InputField submitSeedText;
    public InputField submitPWText;
    public Text missingText;

    public string toAddress;
    public int transferAmount;

    private Account account;
    private Web3 web3;

    public List<int> items;

    private static RubiTokenWrapper tokenContractService;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        IsEncryptedJson();
        tokenContractService = new RubiTokenWrapper();
    }

    public async Task<int> GetTokenBalanceOf()
    {
        int balance = (int)UnitConversion.Convert.FromWei(await tokenContractService.GetFunctionBalanceOf().CallAsync<BigInteger>(WalletManager.Instance.publicAddress));
        Debug.Log("Balance : " + balance);

        return balance;
    }

    public async void GetItemCount()
    {
        int count = await tokenContractService.GetFunctionItemCount().CallAsync<int>(WalletManager.Instance.publicAddress);
        Debug.Log("count : " + count);

        //return count;
    }

    public async void GetPlayerItem()
    {
        //int count = await GetItemCount();

        //for (int i = 0; i < count; i++)
        //{
        //    items.Add(await tokenContractService.GetFunctionPlayerItem().CallAsync<int>(i));
        //}
    }

    public void ReceiveTokenTransfer(int amount)
    {
        tokenContractService.ReceiveTransfer(WalletManager.Instance.publicAddress, amount);
    }

    public void TokenTransferMaster(int amount)
    {
        tokenContractService.TransferMaster(amount);
    }

    public void GetTokenTransfer()
    {
        tokenContractService.toAddress = toAddress;
        tokenContractService.transferAmount = transferAmount;

        tokenContractService.Transfer();
    }

    public void PurchaseItem(int index, int add)
    {
        tokenContractService.PurchaseItem(index, add);
    }

    public void UseItem(int index, int add)
    {
        tokenContractService.UseItem(index, add);
    }

    public void CreateAccount()
    {
        if (signUpPW.text.Length > 7)
        {
            password = signUpPW.text;
            WalletManager.Instance.CreateAccount(password);

            seedPanel.SetActive(true);
            seedText.text = WalletManager.Instance.mnemo.ToString();

            IsEncryptedJson();

            Debug.Log("Address:" + WalletManager.Instance.publicAddress);
            Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);
            Debug.Log("Json:" + WalletManager.Instance.encryptedJson);
            Debug.Log("Password:" + WalletManager.Instance.password);

            password = null;
            //UIFunction();
        }
    }

    public void ImportAccountFromJson()
    {
        if (signInPW.text.Length > 7)
        {
            password = signInPW.text;
            WalletManager.Instance.ImportAccountFromJson(password, json);
            if (WalletManager.Instance.privateKey != null)
            {
                account = new Account(WalletManager.Instance.privateKey);
                web3 = new Web3(account, WalletManager.Instance.URL);

                SceneManager.LoadScene("Main");
            }

            Debug.Log("Address:" + WalletManager.Instance.publicAddress);
            Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);
            Debug.Log("Json:" + WalletManager.Instance.encryptedJson);
            Debug.Log("Password:" + WalletManager.Instance.password);
        }
        else
        {
            passwordNotice.enabled = true;
        }
    }

    public void IsEncryptedJson()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        try
        {
            json = File.ReadAllText(WalletManager.Instance.jsonPath);

            uiManager.creatFlag = false;
            uiManager.AccountButton();
        }
        catch (FileNotFoundException)
        {
            uiManager.creatFlag = true;
            uiManager.AccountButton();
        }
    }

    public void SubmitSeed()
    {
        if (submitSeedText.text != null && submitPWText.text.Length > 7)
        {
            try
            {
                string seedWord = submitSeedText.text;
                string password = submitPWText.text;

                var wallet = new Wallet(seedWord, "rubidium");
                var recoveredAccount = wallet.GetAccount(0);
                var address = recoveredAccount.Address;
                var privateKey = recoveredAccount.PrivateKey;

                WalletManager.Instance.EncryptedJson(password, wallet.GetPrivateKey(0), address);
                IsEncryptedJson();
                CancleForgotPanel();

                Debug.Log("New password:" + password);
            }
            catch (FormatException)
            {
                missingText.enabled = true;
            }
        }
    }

    public void CopySeedBt()
    {
        UniClipboard.SetText(seedText.text);
        SceneManager.LoadScene(0);
    }

    public void ForgotPassword()
    {
        forgotPanel.SetActive(true);
    }

    public void CancleForgotPanel()
    {
        forgotPanel.SetActive(false);
    }

    //private void CreateDefaultAccount()
    //{
    //    if (WalletManager.Instance.publicAddress == "")
    //        ImportAccountFromPrivateKey();
    //}

    //public void ImportAccountFromPrivateKey()
    //{
    //    WalletManager.Instance.ImportAccountFromPrivateKey(privateKey);

    //    Debug.Log("Address:" + WalletManager.Instance.publicAddress);
    //    Debug.Log("PrivateKey:" + WalletManager.Instance.privateKey);
    //}

    //public void TransferWego()
    //{
    //    CreateDefaultAccount();

    //    HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
    //    HexBigInteger gasPrice = new HexBigInteger(UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
    //    HexBigInteger value = new HexBigInteger(UnitConversion.Convert.ToWei(transferAmount));

    //    TransactionInput input = WalletManager.Instance.GetTransferEtherInput(null, toAddress,
    //            gas, gasPrice, value);

    //    StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
    //    {
    //        if (result.Exception == null)
    //        {
    //            Debug.Log(result.Result);
    //            OpenWegoscan(result.Result);
    //        }
    //        else
    //        {
    //            throw new System.InvalidOperationException("Transfer failed");
    //        }
    //    }));
    //}

    //private void OpenWegoscan(string result)
    //{
    //    if (WalletManager.Instance.URL.Contains("7766"))
    //    {
    //        // tx.text = "Tx:" + result;
    //        string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
    //        Application.OpenURL(Wegoscan + result);
    //    }
    //}

    //public void DeployContact()
    //{
    //    CreateDefaultAccount();

    //    HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
    //    HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
    //    HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

    //    TransactionInput input = ContractService.Instance.GetDepolyInpute(gas, gasPrice, value, new object[] { "123" });

    //    StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
    //    {
    //        if (result.Exception == null)
    //        {
    //            Debug.Log(result.Result);
    //            OpenEtherscan(result.Result);
    //        }
    //        else
    //        {
    //            throw new System.InvalidOperationException("Transfer failed");
    //        }
    //    }));
    //}

    //public void TransferEtherToContract()
    //{
    //    CreateDefaultAccount();

    //    HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
    //    HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
    //    HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

    //    TransactionInput input = ContractService.Instance.GetTransactionInput("deposit", gas, gasPrice, value, null);

    //    StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
    //    {
    //        if (result.Exception == null)
    //        {
    //            Debug.Log(result.Result);
    //            OpenEtherscan(result.Result);
    //        }
    //        else
    //        {
    //            throw new System.InvalidOperationException("Transfer failed");
    //        }
    //    }));
    //}

    //public void WriteToContract()
    //{
    //    CreateDefaultAccount();

    //    HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
    //    HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
    //    HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

    //    BigInteger withdrawValue = Nethereum.Util.UnitConversion.Convert.ToWei(0.01);

    //    TransactionInput input = ContractService.Instance.GetTransactionInput("withdraw", gas, gasPrice, value, "123", withdrawValue);

    //    StartCoroutine(WalletManager.Instance.SignTransaction(input, (UnityRequest<string> result) =>
    //    {
    //        if (result.Exception == null)
    //        {
    //            Debug.Log(result.Result);
    //            OpenEtherscan(result.Result);
    //        }
    //        else
    //        {
    //            throw new System.InvalidOperationException("Transfer failed");
    //        }
    //    }));
    //}

    //public void ReadDateFromContract()
    //{
    //    CreateDefaultAccount();

    //    HexBigInteger gas = new HexBigInteger(new BigInteger(this.gas));
    //    HexBigInteger gasPrice = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(this.gasPrice, UnitConversion.EthUnit.Gwei));
    //    HexBigInteger value = new HexBigInteger(Nethereum.Util.UnitConversion.Convert.ToWei(transferAmount));

    //    CallInput input = ContractService.Instance.CreateCallInput("getBalance");

    //    StartCoroutine(WalletManager.Instance.CallTransaction(input, (UnityRequest<string> result) =>
    //    {
    //        if (result.Exception == null)
    //        {
    //            // getbalance of contract ,unit: wei
    //            BigInteger balance = ContractService.Instance.DecodeDate<BigInteger>("getBalance", result.Result);
    //            decimal ba = Nethereum.Util.UnitConversion.Convert.FromWei(balance);

    //            Debug.Log(ba);
    //        }
    //        else
    //        {
    //            throw new System.InvalidOperationException("Transfer failed");
    //        }
    //    }));
    //}

    //private void UIFunction()
    //{
    //    if (qrcode)
    //        qrcode.RenderQRCode(WalletManager.Instance.publicAddress);

    //    addr.text = "Address:" + WalletManager.Instance.publicAddress;
    //}
}
