using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public bool[] Logflags;
    bool[] printflags;
    public Text scrolltext;
    public GameObject PickLog;
    string[] str;
    // Start is called before the first frame update
    void Start()
    {
        Logflags = new bool[9];
        printflags = new bool[9];
        for(int i = 0; i < Logflags.Length; i++)
        {
            Logflags[i] = false;
            printflags[i] = true;
        }
        str = new string[10];
        str[0]= "究竟是我疯了还是这世界疯了？\n"
            + "不过是睡了一觉，天啊，究竟发生了什么？\n"
            + "乔？凯莉？伯纳德大叔？你们去哪了？";
        str[1]= "万幸我不是一个人\n"
            + "如果把计算机小姐也算一个人的话\n"
            + "但我觉得可以算吧，她说话真的很像人类\n"
            + "（为人工智能科学家们鼓掌）\n"
            + "要是只有我一个人真的会疯掉";
        str[2]= "事情向着我最不希望的方向发展了\n"
            + "那些怪物究竟是什么，\n"
            + "EMA每年的安全评估报告难道是拿脚指头写\n"
            + "的吗？\n"
            + "拜托我只是一个研究花花草草的普通人，\n"
            + "我不想在外星球挑战自我OK？\n"
            + "天啊，乔，你们到底去哪了？";
        str[3]= "我这辈子都不会想到有一天我们的那辆老爷\n"
            + "车能派上用场\n"
            + "更别提它很有可能是一根救命稻草\n"
            + "虽然它的靠谱程度和稻草还真差不多\n"
            + "那么问题来了，上次乔把它开哪去了？";
        str[4]= "梅真是太厉害了，反正我是拿这辆破车一点\n"
            + "办法也没有\n"
            + "OK，找到部件，回到这里——一切交给我无\n"
            + "比靠谱的计算机小姐\n"
            + "啊，前提是我不要在路上被这群怪物啃得骨头\n"
            + "都不剩";
        str[5]= "它很厉害，但要是被人发现我让一台计算机用\n"
            + "我的权限登录了外部设备，\n"
            + "那我就别想干了\n"
            + "当然这种情况还是自己的小命比较重要";
        str[6]= "我的朋友们……\n"
            + "我自认为的一个心大的人，\n"
            + "也许先天缺了几根神经——但这样的情况下，\n"
            + "痛苦和迷茫不由分说地击倒了我\n"
            + "是的，迷茫，前所未有的迷茫\n"
            + "我不知道发生了什么，\n"
            + "也不知道我将前往何方——我什么也不知道，\n"
            + "但我知道梅不会说谎，\n"
            + "那么他们存活的几率就真的是微乎其微\n"
            + "我的生活明明不该是这样的，\n"
            + "我有一群很好的同伴，他们是我的同事更是我\n"
            + "的朋友，\n"
            + "OS4177的工作虽然一成不变，但是我喜欢那样\n"
            + "的生活节奏\n"
            + "一夜之间，什么也没有了\n"
            + "我看出梅想努力说点什么来安慰我——尽管那\n"
            + "大概率只是出于程序设定，\n"
            + "但是这种时候有另一个声音在旁边还是让我好\n"
            + "受一些";
        str[7]= "活下去……";
        str[8]= "我不会放弃";
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Logflags.Length; i++)
        {
            if (Logflags[i] && printflags[i])
            {
                printflags[i] = false;
                Text text = Instantiate<Text>(scrolltext);
                text.text = str[i];
                text.rectTransform.SetParent(PickLog.transform, false);
            }
        }
    }
}
