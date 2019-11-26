using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float jumpForce; // 점프 높이

    private Rigidbody2D rd2; // 플레이어의 Rigidbody
    private BoxCollider2D bx2; // 플레이어의 Collider

    [SerializeField]
    private Text rubyScoreText; // rubyScoreText의 UI
    [SerializeField]
    private Text timeText; // timeText의 UI
    [SerializeField]
    private Text scoreText; // scoreText의 UI

    [SerializeField]
    private Text gameStartText; // 시작전 UI

    private float time; // 타이머 변수
    private int rubyScore; // 루비 스코어 변수
    private float score; // 스코어 변수

    static bool gameOver = false; // game over를 체크해주는 flag
    [SerializeField]
    private GameObject gameOverText; // game over일때 띄워주는 패널

    [SerializeField]
    private Text highScoreText; // highScoreText UI
    private float highScore; // high score를 담을 변수

    [SerializeField]
    private Image burningBar_EnergyField; // burning energy field image
    [SerializeField]
    private Image burningFullImage; // burning field full image
    private bool isBurning; // burning flag 
    [SerializeField]
    private GameObject burningPlayer; // 버닝 모드일때 플레이어의 모습

    [SerializeField]
    private AudioSource ruby_SFX; // 루비를 먹었을때의 효과음
    [SerializeField]
    private AudioSource buringBGM;  // 버닝상태에서 나오는 배경음
    [SerializeField]
    private AudioSource idleBGM; // 평상시 게임시 나오는 배경음

    private Scroll scroll; // scroll script를 쓰기위한 scroll형 변수

    [SerializeField]
    private Camera camera; // screen posion을 world posion 으로 바꾸기 위한 카메라 변수

    [SerializeField]
    private GameObject burningBG; // 버닝상태일때의 배경

    Animator anim;
    public GameObject wall;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);

        time = 60;
        rubyScore = 0;
        score = 0;

        anim = GetComponent<Animator>();
        rd2 = GetComponent<Rigidbody2D>();
        bx2 = GetComponent<BoxCollider2D>();
        scroll = FindObjectOfType<Scroll>();

        rd2.isKinematic = true;
        bx2.enabled = false;
        gameOver = true;
        gameOverText.SetActive(false);
        isBurning = false;

        burningFullImage.enabled = false;
        gameStartText.enabled = true;
        gameStartText.text = "Click !!!";

        burningBG.SetActive(false);

        idleBGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (!isBurning)
            {
                time -= Time.deltaTime;
                timeText.text = time.ToString("N0");
            }

            score += Time.deltaTime;
            scoreText.text = "Score : " + score.ToString("N0");

            PlyerController();

            if (burningBar_EnergyField.fillAmount >= 0.05f)
            {
                burningBar_EnergyField.fillAmount -= 0.00025f;
            }
            else if(burningBar_EnergyField.fillAmount < 0.05f)
            {
                burningBar_EnergyField.fillAmount = 0.05f;
            }

        } else
        {
            if(isBurning)
            {
                rd2.isKinematic = true;
            }
            
            if(score == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(GameStart());
                }

            } else
            {
                gameObject.SetActive(false);
            }
            

        }

        if (time <= 0)
        {
            gameOver = true;
            gameOverText.SetActive(true);
            HighScoreSave();
            time = 60;
        }
    }

    // 플레이어 이동 함수
    public void PlyerController()
    {
        bx2.enabled = true;

        if (!isBurning)
        {
            wall.SetActive(true);

            if (score  == 0)
            {
                rd2.isKinematic = false;
            }

            if (Input.GetButtonDown("Fire1") && (rd2.velocity.y <= 0) && (gameObject.transform.position.y <= 3f))
            {
                rd2.velocity = new Vector2(0.5f, jumpForce);
                anim.SetTrigger("Jump");
            }

        }
        else
        {
            wall.SetActive(false);

            Vector3 currentPosition = camera.ScreenToWorldPoint(gameObject.transform.position);
            Vector3 mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

            if (mousePosition.x > 9 || mousePosition.y > 5 || mousePosition.x < -8 || mousePosition.y < -4.5) 
            {
                rd2.isKinematic = true;
                rd2.velocity = new Vector2(0, 0);
            } else
            {
                gameObject.transform.position = mousePosition;
            }

            //Debug.Log(currentPosition + "/" + mousePosition);
        }

        if (transform.position.x < -11 || transform.position.y < -6 || transform.position.x > 15)
        {
            gameOver = true;
            gameOverText.SetActive(true);
            HighScoreSave();
        }
    }

    IEnumerator GameStart()
    {
        gameStartText.text = "Game Start !!!";
        rd2.velocity = new Vector2(0.5f, 2f);

        yield return new WaitForSeconds(1.5f);

        gameOver = false;
        gameStartText.enabled = false;
        
        PlyerController();
    }

    // 플레이어 충돌체크
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ruby")
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                rubyScore++;
                rubyScoreText.text = " : " + rubyScore;
                score += 100;
                scoreText.text = "Score : " + score.ToString("N0");
                anim.SetTrigger("Jump");
                ruby_SFX.Play();
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //Debug.Log("false");

                if (!isBurning)
                {
                    burningBar_EnergyField.fillAmount += 0.5f;
                } 

                if (burningBar_EnergyField.fillAmount == 1.0f)
                {
                    isBurning = true;

                    idleBGM.Stop();
                    buringBGM.Play();

                    if (isBurning)
                    {
                        burningBar_EnergyField.fillAmount = 1.0f;

                        StartCoroutine(ISBurningTrue());
                    }
                }
            }
        }
    }

    IEnumerator ISBurningTrue()
    {
        scroll.burningSpeed += 2.0f;

        if (scroll.burningSpeed >= 10)
        {
            scroll.burningSpeed = 10f;
        }
        score += 2;
        scoreText.text = "Score : " + score.ToString("N0");

        burningBar_EnergyField.fillAmount = 1.0f;

        anim.SetTrigger("Jump");
        isBurning = true;
        //rd2.isKinematic = true;
        burningBG.SetActive(true);
        burningBG.transform.position = new Vector3(0, 0, 1);
        burningBG.gameObject.GetComponent<Scroll>().burningSpeed += 5.0f;

        if(burningBG.gameObject.GetComponent<Scroll>().burningSpeed >= 20.0f)
        {
            burningBG.gameObject.GetComponent<Scroll>().burningSpeed = 15.0f;
        }

        burningFullImage.enabled = true;
        burningBar_EnergyField.enabled = false;
        burningPlayer.SetActive(true);

        yield return new WaitForSeconds(10f);

        isBurning = false;

        rd2.isKinematic = true;
        rd2.velocity = new Vector2(0, 0);

        scroll.burningSpeed = 1;

        burningBG.SetActive(false);
        burningPlayer.SetActive(false);
        burningFullImage.enabled = false;
        burningBar_EnergyField.enabled = true;

        burningBar_EnergyField.fillAmount = 0.05f;

        buringBGM.Stop();
        idleBGM.Play();

        yield return new WaitForSeconds(1.5f);
        rd2.isKinematic = false;
        
    }

    // Replay
    public void ReplayGame()
    {
        SceneManager.LoadScene("Main");
    }


    private void HighScoreSave()
    {
        if (highScore <= 0)
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore_JumpGame", highScore);
            highScoreText.text = "HighScore : " + highScore.ToString("N0");
        }
        else
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetFloat("HighScore_JumpGame", highScore);
                highScoreText.text = "HighScore : " + highScore.ToString("N0");
            }
            else
            {
                highScore = PlayerPrefs.GetFloat("HighScore_JumpGame");
                highScoreText.text = "HighScore : " + highScore.ToString("N0");
            }
        }
        AccountManager.Instance.ReceiveTokenTransfer(rubyScore);
    }


}
