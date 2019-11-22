using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public GameObject op;
    public GameObject cover;
    public GameObject startgame;
    // Start is called before the first frame update
    void Start()
    {
        op.SetActive(false);
        startgame.SetActive(true);
    }

    public void startbutton()
    {
        op.SetActive(true);
        cover.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
