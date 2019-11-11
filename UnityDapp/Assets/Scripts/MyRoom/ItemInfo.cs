using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ItemInfo : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite itemSprite; // 아이템 이미지
    public int itemCost; // 아이템 가격
    public string itemDesc; // 아이템 설명
}
