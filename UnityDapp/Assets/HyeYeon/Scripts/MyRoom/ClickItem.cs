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
    private GameObject warningText; // 돈이 부족할때 뜨는 메세지
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
        warningText.SetActive(false);

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

   // 아이템 다중 일때 슬롯에 개수 보이도록 수정하기 & Json 파일 형식으로 저장한 후 로드하기
    private async void PurchaseItem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hit.collider != null)
            {
                if(rubyCoin - hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost >= 0)
                {
                    //AccountManager.Instance.TokenTransferMaster(hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost);
                    rubyCoin -= hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                    myItemClones = Instantiate(hit.collider.gameObject, myContents.transform.position, Quaternion.identity);
                    myItemClones.transform.SetParent(myContents.transform, false);
                    myItemClones.GetComponent<BoxCollider2D>().enabled = false;

                    //rubyCoin = await AccountManager.Instance.GetTokenBalanceOf();
                    rubyCoinUI.text = " : " + rubyCoin;
                } else
                {
                    warningText.SetActive(true);
                    StartCoroutine(WarningTextFalse());
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            if (hit.collider != null)
            {
                rubyCoin += hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                Destroy(myItemClones);
                multiplePrice = hit.collider.gameObject.GetComponent<Item>().ItemInfo.itemCost;
                multipleItem = hit.collider.gameObject;
                countMessage.SetActive(true);
                buttonText.text = "'" + hit.collider.gameObject.name + "' 아이템 구매?";
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
            if(rubyCoin - multiplePrice * _count >= 0)
            {
                rubyCoin -= multiplePrice * _count;
                for (int i = 0; i < _count; i++)
                {
                    GameObject myItemClones = Instantiate(multipleItem, myContents.transform.position, Quaternion.identity);
                    myItemClones.transform.SetParent(myContents.transform, false);
                }
                countMessage.SetActive(false);
            } else
            {
                warningText.SetActive(true);
                StartCoroutine(WarningTextFalse());
            }
        } 
    }

    IEnumerator WarningTextFalse()
    {
        yield return new WaitForSeconds(3f);
        warningText.SetActive(false);
    }

}
