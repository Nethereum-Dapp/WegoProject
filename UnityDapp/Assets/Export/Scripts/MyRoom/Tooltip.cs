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

    private Item item;

    Vector2 MousPosition;
    Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        toolTip.SetActive(false);
        item = gameObject.GetComponent<Item>();
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
