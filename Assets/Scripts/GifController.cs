using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifController : MonoBehaviour
{
    public GameObject NodeNodeNode;
    public Image image;
    float i = 0;
    public int wait = 10;
    int waitting = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        waitting++;
        if (waitting >= wait)
        {
            waitting = 0;
            if (NodeNodeNode.activeSelf)
            {
                i += 0.75f;
                if (i > 1)
                {
                    i = 0;
                }
                image.color = new Color(1, 1, 1, 1 - i);
            }
        }

    }
}
