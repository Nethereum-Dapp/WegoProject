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
        //Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        var wallet2 = new Wallet("beef model hip rich moral vague affair prevent maze employ saddle eye", "rubidium");
        var account2 = wallet2.GetAccount(0);
        Debug.Log(" - Address : " + account2.Address + " - Private key : " + account2.PrivateKey + " - wallet : " + BitConverter.ToString(wallet2.GetPrivateKey(0)));
        Debug.Log("seed: " + wallet2.Seed + "word: " + wallet2.Words[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
