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
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MousPosition = Input.mousePosition;
        MousPosition = Camera.ScreenToWorldPoint(MousPosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = new Vector2(MousPosition.x-1f, MousPosition.y-1f); // >> 툴팁의 포지션을 마우스 좌표의 옆으로 바꿔주기
        toolTipTextString = "" + item.ItemInfo.itemDesc;
        toolTipTextUI.text = toolTipTextString;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }
}
