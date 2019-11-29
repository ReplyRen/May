using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gifManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NodeNodeNode;
    public Image image;
    public PictureContainer container;
    int i=0;
    public int wait=10;
    int waitting=0;
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
                if (i < container.images.Length)
                {
                    image.sprite = container.images[i];
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
        }
        
    }
}
