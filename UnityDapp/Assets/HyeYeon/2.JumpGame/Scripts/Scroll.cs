using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    private float randomSpeed; // 배경 이동속도 (랜덤)
    public float burningSpeed; // 버닝모드일때의 스피드

    [SerializeField]
    private float staretPos; // 배경 시작지점
    [SerializeField]
    private float endPos; // 배경 끝지점

    [SerializeField]
    private GameObject Ruby; // 스크롤 끝나는 지점에서 Ruby 오브젝트 SetActive 켜주기

    // Start is called before the first frame update
    void Start()
    {
        randomSpeed = Random.Range(2.0f, 6.0f);
        burningSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-1 * randomSpeed * burningSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x <= endPos)
        {
            transform.Translate(-1 * (endPos - staretPos), 0, 0);

            if (gameObject.tag == "Ground" || gameObject.tag == "Ruby")
            {
                Ruby.SetActive(true);

                randomSpeed = Random.Range(2.0f, 6.0f);

                gameObject.SendMessage("ChangeLocation", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
