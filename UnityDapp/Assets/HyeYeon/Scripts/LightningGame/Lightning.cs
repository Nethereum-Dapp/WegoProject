using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float speed; // 번개의 이동속도

    Vector3 playerPosition; // 별의 좌표
    Vector3 myPosition; // 번개의 좌표

    GM gm; // GM 스크립트를 가져오는 GM형 변수


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();
        speed = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gm.isGameOver)
        {
            MoveToPlayer();
        }
        
        if(4.0f <= Vector3.Distance(new Vector3(0, 0, 0), transform.position))
        {
            Destroy(gameObject);
        }
    }

    // 플레이어를 찾아 이동
    private void MoveToPlayer()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position;

        myPosition = transform.position;

        Vector3 dir = playerPosition - myPosition;

        dir.Normalize();

        transform.position += dir * 0.01f * speed;
    }
}
