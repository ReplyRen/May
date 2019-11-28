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
    string[] words;
    public int wait;
    int waitting;
    int strnum;
    bool onetouch;

    // Use this for initialization
    void Start()
    {
        words = new string[6];
        words[1]= "20▉年7月▉日  ▉时29分  联合政府讯\n"
            + "告驻CX470全体人员\n"
            + "距全境封锁还剩48小时";
        words[2]= "搜索可见设备\n"
            + "请求通讯 优先级：D 密级：D\n"
            + "通讯内容代码：505\n"
            + "【注意】此为紧急通讯编号，请谨慎使用";
        words[3]= "数据上传中\n"
            + "0 %……48 %……64 %……77 %……89 %……\n"
            + "数据传输失败 错误 请求超时\n"
            + "【警告】非法信号来源\n"
            + "请您检查您与「火种」的连接，或更换设备后重试";
        words[4]= "60秒后重新发送请求 或在您的终端界面输入任意字符\n"
            + "60……59……58……57……\n"
            + "第134次通讯捕获\n"
            + "检测到您在短时间内密集发送通讯请求，将为您修改通讯适配范围\n"
            + "搜索可见设备";
        words[5]= "请求通讯 优先级：D 密级：D\n"
            + "通讯内容代码：505\n"
            + "【注意】此为紧急通讯编号，请谨慎使用\n"
            + "通讯接入中……\n"
            + "观测到大型智能计算机，将为您接入人工智能服务";

        str = words[1];
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
                if (myInput.isButtonDown&&onetouch)
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