using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject toolTip; // 툴팁창
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
        toolTip = GameObject.Find("/Canvas").transform.Find("Tootip").gameObject; 
        toolTipTextUI = GameObject.Find("/Canvas").transform.Find("Tootip").transform.Find("Text").gameObject.GetComponent<Text>();

        if (toolTip.activeSelf)
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
        toolTipTextString = "" + item.ItemInfo.itemDesc + "\n\n" + item.ItemInfo.itemCost + " 원";
        TextEnterLine(toolTipTextString);
        toolTipTextUI.text = toolTipTextString;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    private string TextEnterLine(string text)
    {
        text = text.Replace("\\n", "\n");
        return text;
    }

}
