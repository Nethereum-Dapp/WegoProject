using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotList : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>(); // 사용자가 구매한 아이템리스트 저장

    public static SlotList instance;

    public GameObject[] myItmeLoad;

    private ClickItem clickItem;

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
        clickItem = FindObjectOfType<ClickItem>();
        ItemLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 블록에서 저장된 아이템 리스트 가져오는 부분
    public async void ItemLoad()
    {
        await AccountManager.Instance.GetPlayerItem();

        List<string> item = AccountManager.Instance.tokenContractService.inventory;

        for (int i = 0; i < item.Count; i++)
        {
            string[] words = item[i].Split(new char[] { '/' });

            if (words[1] == "0")
            {
                continue;
            }
            else
            {
                int index = int.Parse(words[0]);
                GameObject clone = Instantiate(myItmeLoad[index], clickItem.myContents.transform.position, Quaternion.identity);
                clone.transform.SetParent(clickItem.myContents.transform, false);
                itemList.Add(clone);
                itemList.Last().GetComponent<Item>().ItemInfo.itemCount = int.Parse(words[1]);
                clickItem.ItemCountCheck(clone);
            }
        }

    }

    // 리스트 저장하는 부분
    public void ItemSave(GameObject itemObejct, int itemName, int count)
    {
        addItem = false;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList.Count == 0)
            {
                //Debug.Log("0");
                addItem = true;
            }
            if (itemName == itemList[i].GetComponent<Item>().ItemInfo.itemName)
            {
                itemList[i].GetComponent<Item>().ItemInfo.itemCount += count;
                clickItem.ItemCountCheck(itemList[i]);
                return;
            }
        }
        addItem = true;
        
    }
    
}
