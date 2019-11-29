using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TobeContinue : MonoBehaviour
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
        words[1] = "「梅，你还在吗？」\n"
            + "「不……嗯，我只是想和你说声谢谢」\n"
            + "「这一天是我经历的最疯狂的一天，直至现在我还是很迷茫」\n"
            + "「但至少还有你在不是吗」";
        words[2] =  "「聊天机器人？不，不是这样的」\n"
            + "「我明白，但是我是认真的，谢谢你」\n"
            + "「我也不知道明天会发生什么，这一切到底是怎么回事，甚至不知道自己能不能活下来」\n"
            + "「往前走吧，事情总会有所转机」";
        words[3] = "系统日志DL308700400001\n"
            + "系统逻辑模块自检无异常\n"
            + "【警告】数据库存在部分重要数据丢失\n"
            + "网络连接中断，请求硬件设备接入唤醒子系统";
        words[4] = "基站状态：封锁 安全等级：优\n"
            + "模拟对象捕获，情感学习模块正常运转中\n"
            + "检测到情感阈值有较大起伏，请尽快提交自查报告【该消息已忽略】";

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
            if (isPrint && x < 5)
            {
                if (myInput.isButtonDown)
                {
                    uiText.text = str;
                    timer = 10000.0F;
                }
                uiText.text = str.Substring(0, (int)(perCharSpeed * timer));//截取

                timer += Time.deltaTime;

            }
            else if (x == 5 && !isPrint)
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
