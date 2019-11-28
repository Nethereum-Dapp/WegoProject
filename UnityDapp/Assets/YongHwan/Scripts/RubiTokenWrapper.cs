using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class RubiTokenWrapper
{
    private static string contractABI = @"[{'constant': true,'inputs': [],'name': 'name','outputs': [{'name': '','type': 'string'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'approve','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'totalSupply','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'sender','type': 'address'},{'name': 'recipient','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'transferFrom','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'decimals','outputs': [{'name': '','type': 'uint8'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'addedValue','type': 'uint256'}],'name': 'increaseAllowance','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [{'name': 'account','type': 'address'}],'name': 'balanceOf','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [],'name': 'renounceOwnership','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'owner','outputs': [{'name': '','type': 'address'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [],'name': 'isOwner','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [],'name': 'symbol','outputs': [{'name': '','type': 'string'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'subtractedValue','type': 'uint256'}],'name': 'decreaseAllowance','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': false,'inputs': [{'name': 'recipient','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'transfer','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [{'name': 'owner','type': 'address'},{'name': 'spender','type': 'address'}],'name': 'allowance','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'newOwner','type': 'address'}],'name': 'transferOwnership','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'inputs': [{'name': 'name','type': 'string'},{'name': 'symbol','type': 'string'},{'name': 'decimals','type': 'uint8'},{'name': 'totalSupply','type': 'uint256'}],'payable': false,'stateMutability': 'nonpayable','type': 'constructor'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'previousOwner','type': 'address'},{'indexed': true,'name': 'newOwner','type': 'address'}],'name': 'OwnershipTransferred','type': 'event'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'from','type': 'address'},{'indexed': true,'name': 'to','type': 'address'},{'indexed': false,'name': 'value','type': 'uint256'}],'name': 'Transfer','type': 'event'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'owner','type': 'address'},{'indexed': true,'name': 'spender','type': 'address'},{'indexed': false,'name': 'value','type': 'uint256'}],'name': 'Approval','type': 'event'}]";
    private static string contractAddress = "0x4B1c16E5D874137D16238f8fe73f20ab962aA965";
    private Contract contract;

    private static string itemContractABI = @"[{'constant': true,'inputs': [{'name': '','type': 'address'},{'name': '','type': 'uint256'}],'name': 'itemToOwner','outputs': [{'name': 'itemIndex','type': 'uint256'},{'name': 'count','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'index','type': 'uint256'},{'name': 'add','type': 'uint256'}],'name': 'purchaseItem','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': false,'inputs': [{'name': 'index','type': 'uint256'},{'name': 'add','type': 'uint256'}],'name': 'useItem','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [{'name': 'index','type': 'uint256'}],'name': 'getPlayerItem','outputs': [{'name': 'itemIndex','type': 'uint256'},{'name': 'count','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [{'name': 'account','type': 'address'}],'name': 'getItemCount','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'}]";
    private static string itemContractAddress = "0xE8885D79A0482297190e33CD62E55E2A9f96B282";
    private Contract itemContract;

    [Tooltip("Using Unit :Ether")]
    public int transferAmount;
    [Tooltip("The Address You Want to Transfer Ether")]
    public string toAddress;

    private Account account;
    private Web3 web3;

    public static string masterAddress = "0xcEF6BEf33f86260e4f40b3feE889A98Ca6AE825B";
    private static string masterPrivateKey = "FB467591EF10F4E8B86D7D683D731D9B1C8BA08B1A46F92800979736074D15FF";

    public List<string> inventory = new List<string>();

    public RubiTokenWrapper()
    {
        var web3 = new Web3(WalletManager.Instance.URL);
        contract = web3.Eth.GetContract(contractABI, contractAddress);
        itemContract = web3.Eth.GetContract(itemContractABI, itemContractAddress);
    }

    public Function GetFunctionBalanceOf()
    {
        return contract.GetFunction("balanceOf");
    }

    public Function GetFunctionItemCount()
    {
        return itemContract.GetFunction("getItemCount");
    }

    public async Task<int> GetItemCount()
    {
        int count = await GetFunctionItemCount().CallAsync<int>(WalletManager.Instance.publicAddress);
        Debug.Log("count : " + count);

        return count;
    }

    public async Task<bool> GetPlayerItem()
    {
        account = new Account(WalletManager.Instance.privateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var contractHandler = web3.Eth.GetContractHandler(itemContractAddress);
        var getPlayerItemFunction = new GetPlayerItemFunction();

        int count = await GetItemCount();

        if(inventory.Count != 0)
            inventory.Clear();

        for (int i = 0; i < count; i++)
        {
            getPlayerItemFunction.Index = i;
            var getPlayerItemOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetPlayerItemFunction, GetPlayerItemOutputDTO>(getPlayerItemFunction);
            inventory.Add(getPlayerItemOutputDTO.ItemIndex + "/" + getPlayerItemOutputDTO.Count);
            Debug.Log(getPlayerItemOutputDTO.ItemIndex + "/" + getPlayerItemOutputDTO.Count);
        }

        return true;
    }

    public async void Transfer()
    {
        account = new Account(WalletManager.Instance.privateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

        var transfer = new TransferFunction()
        {
            To = toAddress,
            TokenAmount = UnitConversion.Convert.ToWei(transferAmount)
        };

        var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);

        var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
        var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
        var allTransferEventsForContract = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
        Debug.Log("Transfer event TransactionHash : " + allTransferEventsForContract[0].Log.TransactionHash);

        //string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        //Application.OpenURL(Wegoscan + allTransferEventsForContract[0].Log.TransactionHash);
    }

    public async void ReceiveTransfer(string myAddress, int amount)
    {
        account = new Account(masterPrivateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

        var transfer = new TransferFunction()
        {
            To = myAddress,
            TokenAmount = UnitConversion.Convert.ToWei(amount)
        };

        var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);

        var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
        var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
        var allTransferEventsForContract = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
        Debug.Log("Transfer event TransactionHash : " + allTransferEventsForContract[0].Log.TransactionHash);

        //string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        //Application.OpenURL(Wegoscan + allTransferEventsForContract[0].Log.TransactionHash);
    }

    public async Task<bool> TransferMaster(int amount)
    {
        account = new Account(WalletManager.Instance.privateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

        var transfer = new TransferFunction()
        {
            To = masterAddress,
            TokenAmount = UnitConversion.Convert.ToWei(amount)
        };

        var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);

        var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
        var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
        var allTransferEventsForContract = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
        Debug.Log("Transfer event TransactionHash : " + allTransferEventsForContract[0].Log.TransactionHash);

        return true;
        //string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        //Application.OpenURL(Wegoscan + allTransferEventsForContract[0].Log.TransactionHash);
    }

    public async void PurchaseItem(int index, int add)
    {
        account = new Account(WalletManager.Instance.privateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var purchaseHandler = web3.Eth.GetContractTransactionHandler<PurchaseItemFunction>();

        var purchase = new PurchaseItemFunction()
        {
            Index = index,
            Add = add
        };

        Debug.Log(purchase.Index + "/" + purchase.Add);
        var transactionReceipt = await purchaseHandler.SendRequestAndWaitForReceiptAsync(itemContractAddress, purchase);

        //string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        //Application.OpenURL(Wegoscan + transactionReceipt.TransactionHash);
    }

    public async void UseItem(int index, int add)
    {
        account = new Account(WalletManager.Instance.privateKey);
        web3 = new Web3(account, WalletManager.Instance.URL);

        var useItemHandler = web3.Eth.GetContractTransactionHandler<UseItemFunction>();

        var useItem = new UseItemFunction()
        {
            ItemIndex = index,
            Count = add
        };

        var transactionReceipt = await useItemHandler.SendRequestAndWaitForReceiptAsync(itemContractAddress, useItem);

        //string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        //Application.OpenURL(Wegoscan + transactionReceipt.TransactionHash);
    }

    [Function("transfer", "bool")]
    public class TransferFunction : FunctionMessage
    {
        [Parameter("address", "recipient", 1)]
        public string To { get; set; }

        [Parameter("uint256", "amount", 2)]
        public BigInteger TokenAmount { get; set; }
    }

    [Event("Transfer")]
    public class TransferEventDTO : IEventDTO
    {
        [Parameter("address", "_from", 1, true)]
        public string From { get; set; }

        [Parameter("address", "_to", 2, true)]
        public string To { get; set; }

        [Parameter("uint256", "_value", 3, false)]
        public BigInteger Value { get; set; }
    }

    [Function("purchaseItem")]
    public class PurchaseItemFunction : FunctionMessage
    {
        [Parameter("uint", "Index", 1)]
        public int Index { get; set; }

        [Parameter("uint", "add", 2)]
        public int Add { get; set; }
    }

    [Function("useItem")]
    public class UseItemFunction : FunctionMessage
    {
        [Parameter("uint", "itemIndex", 1)]
        public int ItemIndex { get; set; }

        [Parameter("uint", "count", 2)]
        public int Count { get; set; }
    }

    public class GetPlayerItemFunction : GetPlayerItemFunctionBase { }

    [Function("getPlayerItem", typeof(GetPlayerItemOutputDTO))]
    public class GetPlayerItemFunctionBase : FunctionMessage
    {
        [Parameter("uint", "index", 1)]
        public int Index { get; set; }
    }

    public class GetPlayerItemOutputDTO : GetPlayerItemOutputDTOBase { }

    [FunctionOutput]
    public class GetPlayerItemOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint", "itemIndex", 1)]
        public int ItemIndex { get; set; }

        [Parameter("uint", "count", 2)]
        public int Count { get; set; }
    }
}