using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftScene : MonoBehaviour
{
    public MyInput myinput;
    public GameObject endscene;
    public int n;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endscene.activeSelf)
        {

            if (myinput.isButtonDown)
            {
                SceneManager.LoadScene("Begin");
            }
        }
    }
}
