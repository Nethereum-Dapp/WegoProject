using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GM gm; // GM 스크립트를 이용하기 위한 GM형 변수
    Vector3 currentPosition; // 플레이어의 현재 좌표
    [SerializeField]
    private float playerSpeed; // 플레이어 스피드

    [SerializeField]
    private Text rubyScoreText; // Ruby score text UI
    private float rubyScore; // Ruby score 점수 변수

    [SerializeField]
    private Text scoreText; // score text UI
    private float score; // score 점수 변수

    [SerializeField]
    private Image hp; // HP Bar Image UI

    [SerializeField]
    private AudioSource ruby_SFX;

    [SerializeField]
    private AudioSource hurt_SFX;

    [SerializeField]
    private Text highScoreText; // highScoreText UI
    private float highScore; // high score를 담을 변수

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();
        ruby_SFX = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        rubyScore = 0;
        hp.fillAmount = 1;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.isGameOver)
        {
            PlayerController();
            currentPosition = transform.position;
            score += Time.deltaTime;
            scoreText.text = "Score : " + score.ToString("N0");
        }
    }

    // 플레이어 이동 함수 및 이동 제한 범위 설정
    private void PlayerController()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 nextPosition = currentPosition + new Vector3(horizontal * playerSpeed * Time.deltaTime, vertical * playerSpeed * Time.deltaTime);

        float distance = Vector3.Distance(new Vector3(0, 0, 0), nextPosition);

        if (distance < 4.2)
        {
            transform.position += new Vector3(horizontal * playerSpeed * Time.deltaTime, vertical * playerSpeed * Time.deltaTime);
        }

    }

    // 플레이어 충돌체크
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lightning" || collision.gameObject.tag == "_Lightning")
        {
            hp.fillAmount -= 0.05f;
            hurt_SFX.Play();
            anim.SetTrigger("Hurt");

            if (hp.fillAmount <= 0)
            {
                gm.isGameOver = true;
                gm.gameOverText.SetActive(true);
                HighScoreSave();
            }
        }

        if (collision.gameObject.tag == "Ruby")
        {
            rubyScore++;
            rubyScoreText.text = " : " + rubyScore;
            score += 150;
            ruby_SFX.Play();

            for (int i = 0; i < gm.lightning_list.Count; i++)
            {
                Destroy(gm.lightning_list[i]);
            }

            gm.lightning_list.Clear();
            Destroy(collision.gameObject);
        }

    }

    public void HighScoreSave()
    {
        highScore = PlayerPrefs.GetFloat("HighScore_LightningGame");

        float ruby = PlayerPrefs.GetFloat("RubyScore") + rubyScore;
        PlayerPrefs.SetFloat("RubyScore", ruby);

        if (highScore <= 0)
        {
            highScore = score;
            
            PlayerPrefs.SetFloat("HighScore_LightningGame", highScore);
            highScoreText.text = "HighScore : " + highScore.ToString("N0");
        }
        else
        {
            if (score > highScore)
            {
                highScore = score;
                
                PlayerPrefs.SetFloat("HighScore_LightningGame", highScore);
                highScoreText.text = "HighScore : " + highScore.ToString("N0");
            }
            else
            {
                highScore = PlayerPrefs.GetFloat("HighScore_LightningGame");
                highScoreText.text = "HighScore : "+ highScore.ToString("N0");
            }
        }
    }

}
