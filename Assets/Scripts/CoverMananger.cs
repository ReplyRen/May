using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverMananger : MonoBehaviour
{
    public GameObject cover;
    public GameObject op;

    // Start is called before the first frame update
    void Start()
    {
        op.SetActive(false);
        cover.SetActive(true);
    }

    public void startbutton()
    {
        Debug.Log("press button");
        op.SetActive(true);
        cover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
