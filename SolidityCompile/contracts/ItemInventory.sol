pragma solidity ^0.4.21;

contract ItemInventory{
    
    address owner;
    bool isItem;
    string items;
    string result;
    
    struct Player{
        address addr;
        Item[] item;
    }
    
    struct Item{
        uint itemIndex;
        uint count;
    }
    
    function ItemInventory() public{
        owner = msg.sender;
        isItem = false;
    }
    
    mapping (address=>Player) public newPlayer;
    
    function purchaseItem(uint itemIndex, uint count) public{
        
        Player memory currentPlayer = newPlayer[msg.sender];
        
        for(uint i = 0; i > currentPlayer.item.length; i++){
           if(currentPlayer.item[i].itemIndex == itemIndex){
               currentPlayer.item[i].count += count;
               isItem = true;
               break;
           }else{
               isItem = false;
           }
        }
        
        if(!isItem){
            currentPlayer.item[currentPlayer.item.length] = Item(itemIndex, count);
        }
    }
    
    function getPlayerItem() public returns (string) {
        Player memory currentPlayer = newPlayer[msg.sender];
        
        for(uint i = 0; i > currentPlayer.item.length; i++){
            uint itemIndex = currentPlayer.item[i].itemIndex;
            uint itemCount = currentPlayer.item[i].count;
            
            items = concat(uintToString(itemIndex), ",");
            items = concat(items, uintToString(itemCount));
            items = concat(items, "/");
            
            result = concat(result, items);
        }
        return result;
    }
    
    function uintToString(uint v) pure public returns (string) {
        uint maxlength = 100;
        bytes memory reversed = new bytes(maxlength);
        uint i = 0;
        while (v != 0) {
            uint remainder = v % 10;
            v = v / 10;
            reversed[i++] = byte(48 + remainder);
        }
        bytes memory s = new bytes(i);
        for (uint j = 0; j < i; j++) {
            s[j] = reversed[i - j - 1];
        }
        string memory str = string(s);
        return str;
    }
    
    function concat(string _a, string _b) pure public returns (string) {
        bytes memory bytes_a = bytes(_a);
        bytes memory bytes_b = bytes(_b);
        string memory length_ab = new string(bytes_a.length + bytes_b.length);
        bytes memory bytes_c = bytes(length_ab);
        uint k = 0;
        for (uint i = 0; i < bytes_a.length; i++) bytes_c[k++] = bytes_a[i];
        for (i = 0; i < bytes_b.length; i++) bytes_c[k++] = bytes_b[i];
        return string(bytes_c);
    }
}