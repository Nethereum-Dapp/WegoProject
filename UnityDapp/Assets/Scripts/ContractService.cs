using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using UnityEngine;

public class ContractService : MonoBehaviour
{
    public static ContractService Instance = null;

    public string ABI; // Copy from Remix
    public string Bytecode;  // Copy from Remix
    public string contractAddress;

    private Contract contract;
    private DeployContractTransactionBuilder contractTransactionBuilder;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        contractTransactionBuilder = new DeployContractTransactionBuilder();
        InitializeContract();
    }

    public void InitializeContract()
    {
        ABI = ABI.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
        if (contract == null)
        {
            if (ABI != "" && Bytecode != "" && contractAddress != "")
            {
                contract = new Contract(null, ABI, contractAddress);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gas"></param>
    /// <param name="gasPrice"></param>
    /// <param name="value">amount that you transfer ether</param>
    /// <param name="values">values for constructed function of contract</param>
    /// <returns></returns>
    public TransactionInput GetDepolyInpute(HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, object[] values)
    {
        var transactionInput = contractTransactionBuilder.BuildTransaction(ABI, Bytecode, WalletManager.Instance.publicAddress, gas, gasPrice, value, values);

        return transactionInput;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="functionName"></param>
    /// <param name="gas"></param>
    /// <param name="gasPrice"></param>
    /// <param name="value">amount that you transfer ether</param>
    /// <param name="values">values for function parameter</param>
    /// <returns></returns>
    public TransactionInput GetTransactionInput(string functionName, HexBigInteger gas, HexBigInteger gasPrice, HexBigInteger value, params object[] values)
    {
        InitializeContract();

        var func = contract.GetFunction(functionName);

        var transactionInput = func.CreateTransactionInput(WalletManager.Instance.publicAddress, gas, gasPrice, value, values);

        return transactionInput;
    }

    public CallInput CreateCallInput(string functionName)
    {
        InitializeContract();

        var func = contract.GetFunction(functionName);

        return func.CreateCallInput();
    }

    public T DecodeDate<T>(string functionName, string date)
    {
        var func = contract.GetFunction(functionName);

        return func.DecodeSimpleTypeOutput<T>(date);
    }
}
