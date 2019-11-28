using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class TextCharPrint : MonoBehaviour
{
    public Text uiText;
    //储存中间值
    private string str;
    //每个字符的显示速度
    public float timer = 0;
    //限制条件，是否可以进行文本的输出
    public bool isPrint = false;
    public float perCharSpeed = 1;
    public GameObject Op;
    public GameObject Startgame;
    private int text_length = 0;
    private string Ctext;
    int x = 1;
    CameraController camera;
    MyInput myInput;
    // Use this for initialization
    void Start()
    {
        FileStream fs = new FileStream("Assets/text/0/0001-1.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        str = String.Empty;
        str = sr.ReadToEnd();
        Debug.Log(str);
        isPrint = true;
        camera = GameObject.FindWithTag("BMainCamera").GetComponent<CameraController>();
        if (camera == null)
            Debug.Log("camera not set");

        myInput = camera.GetComponent<MyInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myInput.isButtonDown)
            Debug.Log("down");
        printText();
    }

    void printText()
    {
        try
        {
            if (isPrint&&x<6)
            {
                if (myInput.isButtonDown)
                {
                    uiText.text = str;
                    timer = 10000.0F;
                }
                uiText.text = str.Substring(0, (int)(perCharSpeed * timer));//截取

                timer += Time.deltaTime;

            }
            else if (x == 6&& !isPrint)
            {
                if (myInput.isButtonDown)
                {
                    SceneManager.LoadScene("SampleScene");
                    SceneManager.UnloadSceneAsync("Begin");
                    Op.SetActive(false);
                }
            }
            else if (!isPrint)
            {
                if (myInput.isButtonDown)
                {
                    isPrint = true;
                }
            }
        }
        catch (System.Exception)
        {
            x = printEnd(x);
        }
        
    }

    int printEnd(int x)
    {
        x++;
        if (x < 6)
        {
            FileStream fs = new FileStream("Assets/text/0/0001-" + x.ToString() + ".txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
            str = String.Empty;
            str = sr.ReadToEnd();
            timer = 0;
            isPrint = false;
        }
        else
            isPrint = false;

        return x;

    }
}