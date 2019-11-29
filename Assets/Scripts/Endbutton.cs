using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endbutton : MonoBehaviour
{
    // Start is called before the first frame update

    public void end()
    {
        SceneManager.LoadScene("Begin");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
