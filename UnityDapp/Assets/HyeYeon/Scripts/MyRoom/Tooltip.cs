using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject toolTip; // 툴팁창
    [SerializeField]
    private Text toolTipTextUI; // 툴팁 설명 UI
    private string toolTipTextString; // 툴팁 설명 글

    public GameObject countBG; // 아이템 갯수
    public Text itemCountUI; // 아이템 갯수 UI
    public int itemCount; // 아이템 갯수 

    private Item item;

    Vector2 MousPosition;
    Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        toolTip.SetActive(false);
        item = gameObject.GetComponent<Item>();

        itemCount = gameObject.GetComponent<Item>().ItemInfo.itemCount;

        if (itemCount < 1)
        {
            countBG.SetActive(false);
        }

    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = new Vector2(gameObject.transform.position.x -1.5f, gameObject.transform.position.y -1.5f); 
        toolTipTextString = "" + item.ItemInfo.itemDesc;
        toolTipTextUI.text = toolTipTextString;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }
}
