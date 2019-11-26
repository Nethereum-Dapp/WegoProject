using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ClickItem : MonoBehaviour
{
    [SerializeField]
    private GameObject countMessage; // 대량 구매시 개수 선택하는 창

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Text rubyCoinUI; // 코인 UI
    
    public int rubyCoin; // 코인 갯수

    [SerializeField]
    private GameObject myContents; // 구매한 내 아이템

    [SerializeField]
    private Text warningText; // 돈이 부족할때 뜨는 메세지
    [SerializeField]
    private Text buttonText; // 대량구매시 누르는 버튼의 text;
    [SerializeField]
    private InputField inputCount; // 대량 구매시 입력하는 아이템의 갯수;

    private int multiplePrice; // 대량 구매시 저장할 아이템의 가격
    private GameObject multipleItem; // 대량 구매시 저장할 게임오브젝트

    private GameObject myItemClones; // 아이템 구매시 생기는 Clone Objects

    Ray ray;
    RaycastHit2D hit;
    Vector3 mousePos;

    bool flag = false;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    // Start is called before the first frame update
    async void Start()
    {
        countMessage.SetActive(false);
        inputCount.characterLimit = 3;
        warningText.enabled = false;

        rubyCoin = 10000;//await AccountManager.Instance.GetTokenBalanceOf();
        rubyCoinUI.text = " : " + rubyCoin;
    }

    // Update is called once per frame
    void Update()
    {
        PurchaseItem();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
    }

   // 아이템 다중 일때 슬롯에 개수 보이도록 수정하기 
    private async void PurchaseItem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hit.collider != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
                {
                    countMessage.SetActive(true);
                    //rubyCoin += hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                    multiplePrice = hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                    multipleItem = hit.collider.gameObject;

                    buttonText.text = "'" + hit.collider.gameObject.name + "' 아이템 구매?";
                    return;
                }

                if (rubyCoin - hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost >= 0)
                {
                    //AccountManager.Instance.TokenTransferMaster(hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost);
                    //AccountManager.Instance.PurchaseItem(hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemName, 1);
                    //AccountManager.Instance.UseItem(hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemName, 1);
                    //rubyCoin = await AccountManager.Instance.GetTokenBalanceOf() + 10000;

                    rubyCoin -= hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                    
                    SlotList.instance.ItemSave(hit.collider.gameObject, hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemName, 1);

                    if (SlotList.instance.addItem)
                    {
                        myItemClones = Instantiate(hit.collider.gameObject, myContents.transform.position, Quaternion.identity);
                        myItemClones.transform.SetParent(myContents.transform, false);
                        myItemClones.GetComponent<BoxCollider2D>().enabled = false;
                        SlotList.instance.itemList.Add(myItemClones);
                    }

                    //rubyCoin = await AccountManager.Instance.GetTokenBalanceOf();
                    rubyCoinUI.text = " : " + rubyCoin;
                } else
                {
                    warningText.enabled = true;
                    StartCoroutine(WarningTextFalse());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            countMessage.SetActive(false);
        }
    }
    
    public void MultipleItemPurchase()
    {
        if (inputCount.text != null && rubyCoin >=0)
        {
            int _count;
            _count = int.Parse(inputCount.text);

            if(_count <1)
            {
                warningText.text = "1보다 큰 값을 입력해주세요";
                warningText.enabled = true;
                StartCoroutine(WarningTextFalse());
                return;
            }

            if(rubyCoin - multiplePrice * _count >= 0)
            {
                rubyCoin -= multiplePrice * _count;
                rubyCoinUI.text = " : " + rubyCoin;

                SlotList.instance.ItemSave(multipleItem, multipleItem.GetComponent<Item>().ItemInfo.itemName, _count);

                if (SlotList.instance.addItem)
                {
                    myItemClones = Instantiate(multipleItem, myContents.transform.position, Quaternion.identity);
                    myItemClones.transform.SetParent(myContents.transform, false);
                    myItemClones.GetComponent<BoxCollider2D>().enabled = false;
                    SlotList.instance.itemList.Add(myItemClones);
                }

                ItemCountCheck(myItemClones);

                countMessage.SetActive(false);
            } else
            {
                warningText.enabled = true;
                StartCoroutine(WarningTextFalse());
            }
        } 
    }

    IEnumerator WarningTextFalse()
    {
        yield return new WaitForSeconds(3f);
        warningText.enabled = false;
        warningText.text = "돈이 부족합니다.";
    }

    public void ItemCountCheck(GameObject item)
    {
        item.GetComponent<Tooltip>().itemCount = item.GetComponent<Item>().ItemInfo.itemCount;

        if (item.GetComponent<Tooltip>().itemCount >= 2)
        {
            item.GetComponent<Tooltip>().countBG.SetActive(true);
            item.GetComponent<Tooltip>().itemCountUI.text = "" + item.GetComponent<Tooltip>().itemCount;
            Debug.Log(item.GetComponent<Tooltip>().itemCountUI.text);
        }
    }
}
