using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private float maxHeight; // Ground 최대 높이
    [SerializeField]
    private float minHeight; // Ground 최소 높이
    [SerializeField]
    private float location_x; // Ground x 좌표
  

    // Start is called before the first frame update
    void Start()
    {
        ChangeLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeLocation()
    {
        
        float height = Random.Range(minHeight, maxHeight);
        transform.localPosition = new Vector3(location_x, height, 0.0f);
    }
}
