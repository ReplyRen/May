using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverManager : MonoBehaviour
{
    public GameObject op;
    public GameObject cover;
    // Start is called before the first frame update
    [RuntimeInitializeOnLoadMethod]
    void Start()
    {
        op.SetActive(false);
        SceneManager.UnloadSceneAsync("SampleScene");
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
