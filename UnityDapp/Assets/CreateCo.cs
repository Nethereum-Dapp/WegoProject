using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts; 
using Nethereum.Util; 
using Nethereum.Hex.HexConvertors.Extensions; 
using Nethereum.HdWallet;
using System;
using UnityEngine;

public class CreateCo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        string Password2 = "password2";
        var wallet2 = new Wallet(mnemo.ToString(), Password2);
        var account2 = wallet2.GetAccount(0);
        Debug.Log(" - Address : " + account2.Address + " - Private key : " + account2.PrivateKey);
        Debug.Log(wallet2.IsMneumonicValidChecksum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
