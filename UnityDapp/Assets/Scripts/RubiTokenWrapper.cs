﻿using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using UnityEngine;

public class RubiTokenWrapper
{
    //public static string BYTECODE = "0x60806040523480156200001157600080fd5b5060405162001d0738038062001d07833981018060405260808110156200003757600080fd5b8101908080516401000000008111156200005057600080fd5b828101905060208101848111156200006757600080fd5b81518560018202830111640100000000821117156200008557600080fd5b50509291906020018051640100000000811115620000a257600080fd5b82810190506020810184811115620000b957600080fd5b8151856001820283011164010000000082111715620000d757600080fd5b5050929190602001805190602001909291908051906020019092919050505083838382600090805190602001906200011192919062000501565b5081600190805190602001906200012a92919062000501565b5080600260006101000a81548160ff021916908360ff1602179055505050506200016262000264640100000000026401000000009004565b600660006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16600073ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a36200025a6200023c6200026c640100000000026401000000009004565b8360ff16600a0a830262000296640100000000026401000000009004565b50505050620005b0565b600033905090565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905090565b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff16141515156200033c576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252601f8152602001807f45524332303a206d696e7420746f20746865207a65726f20616464726573730081525060200191505060405180910390fd5b6200036181600554620004766401000000000262001506179091906401000000009004565b600581905550620003c981600360008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054620004766401000000000262001506179091906401000000009004565b600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508173ffffffffffffffffffffffffffffffffffffffff16600073ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef836040518082815260200191505060405180910390a35050565b6000808284019050838110151515620004f7576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252601b8152602001807f536166654d6174683a206164646974696f6e206f766572666c6f77000000000081525060200191505060405180910390fd5b8091505092915050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106200054457805160ff191683800117855562000575565b8280016001018555821562000575579182015b828111156200057457825182559160200191906001019062000557565b5b50905062000584919062000588565b5090565b620005ad91905b80821115620005a95760008160009055506001016200058f565b5090565b90565b61174780620005c06000396000f3fe6080604052600436106100db576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff16806306fdde03146100e0578063095ea7b31461017057806318160ddd146101e357806323b872dd1461020e578063313ce567146102a157806339509351146102d257806370a0823114610345578063715018a6146103aa5780638da5cb5b146103c15780638f32d59b1461041857806395d89b4114610447578063a457c2d7146104d7578063a9059cbb1461054a578063dd62ed3e146105bd578063f2fde38b14610642575b600080fd5b3480156100ec57600080fd5b506100f5610693565b6040518080602001828103825283818151815260200191508051906020019080838360005b8381101561013557808201518184015260208101905061011a565b50505050905090810190601f1680156101625780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b34801561017c57600080fd5b506101c96004803603604081101561019357600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919080359060200190929190505050610735565b604051808215151515815260200191505060405180910390f35b3480156101ef57600080fd5b506101f8610753565b6040518082815260200191505060405180910390f35b34801561021a57600080fd5b506102876004803603606081101561023157600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803573ffffffffffffffffffffffffffffffffffffffff1690602001909291908035906020019092919050505061075d565b604051808215151515815260200191505060405180910390f35b3480156102ad57600080fd5b506102b661087a565b604051808260ff1660ff16815260200191505060405180910390f35b3480156102de57600080fd5b5061032b600480360360408110156102f557600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919080359060200190929190505050610891565b604051808215151515815260200191505060405180910390f35b34801561035157600080fd5b506103946004803603602081101561036857600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190505050610944565b6040518082815260200191505060405180910390f35b3480156103b657600080fd5b506103bf61098d565b005b3480156103cd57600080fd5b506103d6610aca565b604051808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060405180910390f35b34801561042457600080fd5b5061042d610af4565b604051808215151515815260200191505060405180910390f35b34801561045357600080fd5b5061045c610b53565b6040518080602001828103825283818151815260200191508051906020019080838360005b8381101561049c578082015181840152602081019050610481565b50505050905090810190601f1680156104c95780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b3480156104e357600080fd5b50610530600480360360408110156104fa57600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919080359060200190929190505050610bf5565b604051808215151515815260200191505060405180910390f35b34801561055657600080fd5b506105a36004803603604081101561056d57600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff16906020019092919080359060200190929190505050610d06565b604051808215151515815260200191505060405180910390f35b3480156105c957600080fd5b5061062c600480360360408110156105e057600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190803573ffffffffffffffffffffffffffffffffffffffff169060200190929190505050610d24565b6040518082815260200191505060405180910390f35b34801561064e57600080fd5b506106916004803603602081101561066557600080fd5b81019080803573ffffffffffffffffffffffffffffffffffffffff169060200190929190505050610dab565b005b606060008054600181600116156101000203166002900480601f01602080910402602001604051908101604052809291908181526020018280546001816001161561010002031660029004801561072b5780601f106107005761010080835404028352916020019161072b565b820191906000526020600020905b81548152906001019060200180831161070e57829003601f168201915b5050505050905090565b6000610749610742610e33565b8484610e3b565b6001905092915050565b6000600554905090565b600061076a8484846110bc565b61086f84610776610e33565b61086a85606060405190810160405280602881526020017f45524332303a207472616e7366657220616d6f756e742065786365656473206181526020017f6c6c6f77616e6365000000000000000000000000000000000000000000000000815250600460008b73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000610820610e33565b73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020546114449092919063ffffffff16565b610e3b565b600190509392505050565b6000600260009054906101000a900460ff16905090565b600061093a61089e610e33565b8461093585600460006108af610e33565b73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008973ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205461150690919063ffffffff16565b610e3b565b6001905092915050565b6000600360008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020549050919050565b610995610af4565b1515610a09576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260208152602001807f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e657281525060200191505060405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff16600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a36000600660006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905090565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16610b37610e33565b73ffffffffffffffffffffffffffffffffffffffff1614905090565b606060018054600181600116156101000203166002900480601f016020809104026020016040519081016040528092919081815260200182805460018160011615610100020316600290048015610beb5780601f10610bc057610100808354040283529160200191610beb565b820191906000526020600020905b815481529060010190602001808311610bce57829003601f168201915b5050505050905090565b6000610cfc610c02610e33565b84610cf785606060405190810160405280602581526020017f45524332303a2064656372656173656420616c6c6f77616e63652062656c6f7781526020017f207a65726f00000000000000000000000000000000000000000000000000000081525060046000610c70610e33565b73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008a73ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020546114449092919063ffffffff16565b610e3b565b6001905092915050565b6000610d1a610d13610e33565b84846110bc565b6001905092915050565b6000600460008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002054905092915050565b610db3610af4565b1515610e27576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260208152602001807f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e657281525060200191505060405180910390fd5b610e3081611590565b50565b600033905090565b600073ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff1614151515610f06576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260248152602001807f45524332303a20617070726f76652066726f6d20746865207a65726f2061646481526020017f726573730000000000000000000000000000000000000000000000000000000081525060400191505060405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1614151515610fd1576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260228152602001807f45524332303a20617070726f766520746f20746865207a65726f20616464726581526020017f737300000000000000000000000000000000000000000000000000000000000081525060400191505060405180910390fd5b80600460008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925836040518082815260200191505060405180910390a3505050565b600073ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff1614151515611187576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260258152602001807f45524332303a207472616e736665722066726f6d20746865207a65726f20616481526020017f647265737300000000000000000000000000000000000000000000000000000081525060400191505060405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1614151515611252576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260238152602001807f45524332303a207472616e7366657220746f20746865207a65726f206164647281526020017f657373000000000000000000000000000000000000000000000000000000000081525060400191505060405180910390fd5b61130281606060405190810160405280602681526020017f45524332303a207472616e7366657220616d6f756e742065786365656473206281526020017f616c616e63650000000000000000000000000000000000000000000000000000815250600360008773ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020546114449092919063ffffffff16565b600360008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555061139781600360008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205461150690919063ffffffff16565b600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef836040518082815260200191505060405180910390a3505050565b600083831115829015156114f3576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825283818151815260200191508051906020019080838360005b838110156114b857808201518184015260208101905061149d565b50505050905090810190601f1680156114e55780820380516001836020036101000a031916815260200191505b509250505060405180910390fd5b5060008385039050809150509392505050565b6000808284019050838110151515611586576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040180806020018281038252601b8152602001807f536166654d6174683a206164646974696f6e206f766572666c6f77000000000081525060200191505060405180910390fd5b8091505092915050565b600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff161415151561165b576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004018080602001828103825260268152602001807f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206181526020017f646472657373000000000000000000000000000000000000000000000000000081525060400191505060405180910390fd5b8073ffffffffffffffffffffffffffffffffffffffff16600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a380600660006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055505056fea165627a7a723058207b39e4228546c64ab6dca7ede4134fa1456490756a78aa1472d1098a9c6e60230029";
    private static string contractABI = @"[{'constant': true,'inputs': [],'name': 'name','outputs': [{'name': '','type': 'string'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'approve','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'totalSupply','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'sender','type': 'address'},{'name': 'recipient','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'transferFrom','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'decimals','outputs': [{'name': '','type': 'uint8'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'addedValue','type': 'uint256'}],'name': 'increaseAllowance','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [{'name': 'account','type': 'address'}],'name': 'balanceOf','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [],'name': 'renounceOwnership','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [],'name': 'owner','outputs': [{'name': '','type': 'address'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [],'name': 'isOwner','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': true,'inputs': [],'name': 'symbol','outputs': [{'name': '','type': 'string'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'spender','type': 'address'},{'name': 'subtractedValue','type': 'uint256'}],'name': 'decreaseAllowance','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': false,'inputs': [{'name': 'recipient','type': 'address'},{'name': 'amount','type': 'uint256'}],'name': 'transfer','outputs': [{'name': '','type': 'bool'}],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'constant': true,'inputs': [{'name': 'owner','type': 'address'},{'name': 'spender','type': 'address'}],'name': 'allowance','outputs': [{'name': '','type': 'uint256'}],'payable': false,'stateMutability': 'view','type': 'function'},{'constant': false,'inputs': [{'name': 'newOwner','type': 'address'}],'name': 'transferOwnership','outputs': [],'payable': false,'stateMutability': 'nonpayable','type': 'function'},{'inputs': [{'name': 'name','type': 'string'},{'name': 'symbol','type': 'string'},{'name': 'decimals','type': 'uint8'},{'name': 'totalSupply','type': 'uint256'}],'payable': false,'stateMutability': 'nonpayable','type': 'constructor'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'previousOwner','type': 'address'},{'indexed': true,'name': 'newOwner','type': 'address'}],'name': 'OwnershipTransferred','type': 'event'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'from','type': 'address'},{'indexed': true,'name': 'to','type': 'address'},{'indexed': false,'name': 'value','type': 'uint256'}],'name': 'Transfer','type': 'event'},{'anonymous': false,'inputs': [{'indexed': true,'name': 'owner','type': 'address'},{'indexed': true,'name': 'spender','type': 'address'},{'indexed': false,'name': 'value','type': 'uint256'}],'name': 'Approval','type': 'event'}]";
    private static string contractAddress = "0xe167Ba2c95182A6B1a30793B35dD486bb997FcFf";
    private Contract contract;

    [Tooltip("Using Unit :Ether")]
    public int transferAmount;
    [Tooltip("The Address You Want to Transfer Ether")]
    public string toAddress;

    private Account account;
    private Web3 web3;

    public RubiTokenWrapper()
    {
        var web3 = new Web3(WalletManager.Instance.URL);
        contract = web3.Eth.GetContract(contractABI, contractAddress);
    }

    public Function GetFunctionBalanceOf()
    {
        return contract.GetFunction("balanceOf");
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

        string Wegoscan = "http://125.133.75.165:8083/blocks/0/txnList/";
        Application.OpenURL(Wegoscan + allTransferEventsForContract[0].Log.TransactionHash);
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
}