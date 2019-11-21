using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using static UnityEngine.Mathf;
using System.Linq;
using System.Collections.Generic;

public class GM : MonoBehaviour
{
    [SerializeField]
    private GameObject player; // 플레이어 캐릭터

    [SerializeField]
    private Text timeText; // 타이머 UI
    [SerializeField]
    private Text startText; // 게임시작을 알려주는 텍스트 UI

    private float time; // 타이머 text

    [SerializeField]
    private GameObject lightning; // 번개모양 장애물 
    [SerializeField]
    private GameObject lightningClones; // 장애물들을 하이어라키뷰에서 정리해줄 부모오브젝트
    public List<GameObject> lightning_list = new List<GameObject>(); // 장애물들을 배열에 저장
 

    [SerializeField]
    private GameObject ruby; // 루비
    [SerializeField]
    private GameObject rubyClones; // 루비들을 하이어라키뷰에서 정리해줄 부모오브젝트

    public bool isGameOver = true; // 게임오버 체크 플래그
    
    public GameObject gameOverText; // 게임오버 화면

    [SerializeField]
    private float r; // 원의 반지름
    private float degree; // Field의 원주에서 랜덤하게 생성할 장애물의 각도

    [SerializeField]
    private float term; // 몇 초에 한 번씩 장애물을 생성할지 결정할 변수 (기준점)
    private float currentTime; // 현재 시간을 받아올 변수

    Player _player; // player script를 사용하기 위한 player형 변수

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 60;
        currentTime = 0;

        gameOverText.SetActive(false);
        isGameOver = true;

        startText.text = "Click !!";

        InvokeRepeating("GenerateRuby", 10, 10);

        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            time -= Time.deltaTime;
            timeText.text = time.ToString("N0");

            degree = Random.Range(0, 360f);

            currentTime += Time.deltaTime;

            if (currentTime > term)
            {
                currentTime = 0.0f;
                GenerateLightning();
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startText.text = "GameStart !!";
                StartCoroutine(GameStartText());
            }

        }
        if (time <= 0)
        {
            time = 60;
            isGameOver = true;
            gameOverText.SetActive(true);
            _player.HighScoreSave();
            
        }
    }

    // 시작 텍스트 코루틴
    IEnumerator GameStartText ()
    {
        yield return new WaitForSeconds(1.5f);
        startText.enabled = false;
        isGameOver = false;
    }
   
    // 장애물 instantiate
    private void GenerateLightning ()
    {
        lightning_list.Add(Instantiate(lightning, new Vector3(r * Cos(degree), r * Sin(degree)), Quaternion.identity));
        lightning_list.Last().transform.SetParent(lightningClones.transform);
        
    }

    private void GenerateRuby ()
    {
        float _r;
        _r = Random.Range(0, 3f);
        GameObject ru_go = Instantiate(ruby, new Vector3(_r * Cos(degree), _r * Sin(degree)), Quaternion.identity);
        Destroy(ru_go, 10f);
    }


    // 리플레이 함수
    public void ReStart ()
    {
        SceneManager.LoadScene("LightningGame");
    }

}
