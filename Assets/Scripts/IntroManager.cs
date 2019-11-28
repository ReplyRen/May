using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Intro;
    public GameObject left;
    public GameObject right;
    public PictureContainer pictures;
    int index;
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    public void LeftArrow()
    {
        if (index > 0)
        {
            index--;
        }
    }
    public void RightArrow()
    {
        if (index < 2)
        {
            index++;
        }
    }

    void Update()
    {
        if (index > 0)
            left.SetActive(true);
        else
            left.SetActive(false);
        if (index < 2)
            right.SetActive(true);
        else
            right.SetActive(false);

        Intro.sprite = pictures.images[index];
    }
}
