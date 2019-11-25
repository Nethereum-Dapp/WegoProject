using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotList : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>();

    public static SlotList instance;

    ClickItem item;

    public bool addItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        addItem = false;
        item = FindObjectOfType<ClickItem>();
        // 저장된 list 가져오는 부분
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 리스트 저장하는 부분
    public void ItemSave(GameObject itemObejct, string itemName, int count)
    {
        addItem = false;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList.Count == 0)
            {
                Debug.Log("0");
                addItem = true;
            }
            if (itemName == itemList[i].GetComponent<Item>().ItemInfo.itemName)
            {
                itemList[i].GetComponent<Item>().ItemInfo.itemCount += count;
                Debug.Log(itemList[i].GetComponent<Item>().ItemInfo.itemCount);
                item.ItemCountCheck(itemList[i]);
                return;
            }
        }
        addItem = true;
        
    }
}
