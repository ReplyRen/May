using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class losecharprint : MonoBehaviour
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
    public CameraController camera;
    MyInput myInput;
    string[] words;
    public int wait;

    int waitting;
    int strnum;
    bool onetouch;

    // Use this for initialization
    void Start()
    {
        words = new string[6];
        words[1] = "【警告】目标生命值过低，请尽快接入医疗设备\n"
            + "【警告】目标生命值过低，请尽快接入医疗设备\n"
            + "【警告】目标生命值过低，请尽快接入医疗设备\n"
            + "……";
        words[2]="未检测到生命体征，连接已自动断开";

        str = words[1];
        isPrint = true;

        myInput = camera.GetComponent<MyInput>();

        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");

        if (myInput.isButtonDown)
            Debug.Log("down");
        printText();
    }

    void printText()
    {
        try
        {
            if (isPrint && x < 3)
            {
                if (myInput.isButtonDown)
                {
                    uiText.text = str;
                    timer = 10000.0F;
                }
                uiText.text = str.Substring(0, (int)(perCharSpeed * timer));//截取

                timer += Time.deltaTime;

            }
            else if (x == 3 && !isPrint)
            {
                if (myInput.isButtonDown)
                {
                    Startgame.SetActive(true);
                    Op.SetActive(false);
                }
            }
            else if (!isPrint)
            {
                if (myInput.isButtonDown && onetouch)
                {
                    isPrint = true;
                }

            }
            if (!myInput.isButtonDown)
            {
                onetouch = true;
            }
        }
        catch (System.Exception)
        {
            x = printEnd(x);
        }

    }

    int printEnd(int x)
    {
        if (onetouch)
        {
            x++;
            onetouch = false;
            if (x < 6)
            {
                str = words[x];
                timer = 0;
                isPrint = false;
            }
            else
                isPrint = false;
        }


        return x;

    }
}
