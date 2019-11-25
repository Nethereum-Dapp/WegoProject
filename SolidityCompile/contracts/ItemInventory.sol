pragma solidity ^0.4.23;

contract ItemInventory{
    
    struct Item{
        uint itemIndex;
        uint count;
    }
    
    mapping (address => Item[]) public itemToOwner;
    
    function purchaseItem(uint index, uint add) public{
        for(uint i = 0; i < itemToOwner[msg.sender].length; i++){
            if(itemToOwner[msg.sender][i].itemIndex == index){
                itemToOwner[msg.sender][i].count += add;
                return;
            }
        }
        itemToOwner[msg.sender].push(Item(index, add));
    }
    
    function useItem(uint index, uint add) public{
        for(uint i = 0; i < itemToOwner[msg.sender].length; i++){
            if(itemToOwner[msg.sender][i].itemIndex == index){
                itemToOwner[msg.sender][i].count -= add;
                return;
            }
        }
    }
    
    function getPlayerItem(uint index) public view returns (uint, uint) {
        return (itemToOwner[msg.sender][index].itemIndex, itemToOwner[msg.sender][index].count);
    }
    
    function getItemCount(address account) public view returns (uint){
        return itemToOwner[account].length;
    }
}