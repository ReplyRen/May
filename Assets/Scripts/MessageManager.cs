using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public class queue
    {
        public int[] line;
        public int head;
        public int tail;
        public int length;
        public Button MessageButton;

        public queue(Button button)
        {
            line = new int[100];
            head = tail = 0;
            length = 0;
            MessageButton = button;
        }

        public void insert(int x)
        {
            if (length < 100)
            {
                line[tail] = x;
                tail = (tail + 1) % 100;
                length++;
                MessageButton.GetComponentInChildren<Text>().text = length.ToString();
                MessageButton.GetComponent<Image>().sprite = MessageButton.GetComponent<PictureContainer>().images[1];
            }
        }

        public int change(int x)
        {
            if (length > 0)
            {
                line[head] = x;
            }
            return x;
        }

        public int delete()
        {
            int x = get();
            if (length > 0)
            {
                head = (head + 1) % 100;
                length--;
            }
            if (length > 0)
            {
                MessageButton.GetComponentInChildren<Text>().text = length.ToString();
                MessageButton.GetComponent<Image>().sprite = MessageButton.GetComponent<PictureContainer>().images[1];
            }
            else if (length == 0)
            {
                MessageButton.GetComponentInChildren<Text>().text = null;
                MessageButton.GetComponent<Image>().sprite = MessageButton.GetComponent<PictureContainer>().images[0];
            }
            return x;
        }

        public int get()
        {
            if (length > 0)
            {
                return line[head];
            }
            else
                return 0;
        }
    }
    //[HideInInspector]
    public queue line;
    [HideInInspector]
    public int status;//0为演出空闲，可演出新剧情，1为剧情演出中,2为等待选择
    StreamReader sr;
    public PlayerAsset asset;
    public Text scrolltext;
    public GameObject PickMessage;
    public LogManager logmanager;
    public GameObject message;
    public GameObject onechoice;
    public Text left;
    public Text right;
    public GameObject twochoice;
    public Text middle;
    [HideInInspector]
    public int lastnode;
    public Button MessageButton;
    public GameObject hexgrid;
    public Scrollbar Scroll;
    public TipController tip;
    [HideInInspector]
    public int[][][] NodeTable;
    public int wait;
    private int waitting;
    string[] str;
    int strnum;
    public MessagerContainer container;
    public GameObject MessageTip;
    public GameObject[] notes;
    bool[] noteflags;
    public GameObject[] notepoint;
    public GridContent gridcontent;
    public SolveInput solveinput;
    public GameObject NodeNodeNode;

    bool[] flags;

    public int[] FlagsControll;//[0]为是否到过portal,[1]为到过的任务点个数

    // Start is called before the first frame update
    void Start()
    {
        line = new queue(MessageButton);
        line.insert(1001);
        status = 0;
        lastnode = 0;
        FlagsControll = new int[] { 0, 0 };
        NodeTable = new int[3][][];
        NodeTable[1] = new int[50][];
        NodeTable[2] = new int[50][];
        flags = new bool[11];
        for (int j = 0; j < 11; j++)
        {
            flags[j] = true;
        }
        noteflags = new bool[7];
        for (int j = 0; j < noteflags.Length; j++)
        {
            noteflags[j] = false;
        }

        string[] temp, str;

        str = container.NodeTable.Split('\n');
        foreach (string tstr in str)
        {
            Debug.Log(tstr);
        }
        int i;
        for (i = 0; i < str.Length; i++)
        {
            str[i].Replace("\n", "");
            temp = str[i].Split(' ');
            if (int.Parse(temp[0]) < 2000)
            {
                int j = int.Parse(temp[0]) - 1000;
                NodeTable[1][j] = new int[4];
                int n = 0;
                foreach (string tstr in temp)
                {
                    NodeTable[1][j][n] = int.Parse(tstr);
                    n++;
                }
            }
            else if (int.Parse(temp[0]) >= 2000)
            {
                int j = int.Parse(temp[0]) - 2000;
                NodeTable[2][j] = new int[4];
                int n = 0;
                foreach (string tstr in temp)
                {
                    NodeTable[2][j][n] = int.Parse(tstr);
                    n++;
                }
            }
        }

    }


    private void getwords(int x)
    {
        int t = x;
        int i;
        for (i = 0; (t - 1000) > 0; t -= 1000, i++) ;
        string words = container.messages[i][t];
        str = words.Split('\n');
        strnum = 0;
        waitting = 0;
    }
    private void printwords()
    {
        waitting++;
        if (waitting >= wait)
        {
            waitting = 0;

            if (strnum < str.Length)
            {
                Scroll.value = 0;
                Text text = Instantiate<Text>(scrolltext);
                text.text = str[strnum];
                strnum++;
                text.rectTransform.SetParent(PickMessage.transform, false);
                Scroll.value = 0;
            }
            else
            {
                int n = 0;
                int x = line.get();
                Debug.Log(x);
                nodeIncident(x);
                int t = line.get();
                while (t >= 1000)
                {
                    t -= 1000;
                    n++;
                }
                if (NodeTable[n][t][1] == 1)
                {
                    status = 2;
                    OneButtonDisplay(x);
                }
                else if (NodeTable[n][t][1] == 2)
                {
                    status = 2;
                    TwoButtonDisplay(x);
                }
                else if (NodeTable[n][t][1] == 0)
                {
                    lastnode = line.delete();
                    status = 0;
                    NodeNodeNode.SetActive(true);
                    if (lastnode == 1009)
                        tip.insert3014();
                }
            }
        }
    }



    IEnumerator messagetip(string[] str)
    {
        MessageTip.SetActive(true);
        Text text = MessageTip.GetComponentInChildren<Text>();
        for (int i = 0; i < str.Length; i++)
        {
            text.text = str[i];
            yield return new WaitForSeconds(2f);
        }
        MessageTip.SetActive(false);
    }
    IEnumerator messagetip(string str)
    {
        MessageTip.SetActive(true);
        Text text = MessageTip.GetComponentInChildren<Text>();
        text.text = str;
        yield return new WaitForSeconds(2f);
        MessageTip.SetActive(false);
    }

    void nodeIncident(int x)
    {
        if (x == 1006)
        {
            asset.increaseFavorability(15);
            StartCoroutine(messagetip("同步率有所变化"));
        }
        else if (x == 1001)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[0] = true;
        }
        else if (x == 1009)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[1] = true;
            gridcontent.portalsee = true;
        }
        else if (x == 1010)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[2] = true;
        }
        else if (x == 1012)
        {
            asset.increaseFavorability(15);
            StartCoroutine(messagetip("同步率有所变化"));
        }
        else if (x == 1015)
        {
            asset.increaseChip(1);
            string[] str = new string[2];
            str[0] = "获得芯片X1";
            str[1] = "日志已更新";
            StartCoroutine(messagetip(str));
            logmanager.Logflags[3] = true;
            solveinput.viewportalflag = true;
        }
        else if (x == 1016)
        {
            asset.increaseChip(1);
            string[] str = new string[2];
            str[0] = "获得芯片X1";
            str[1] = "日志已更新";
            StartCoroutine(messagetip(str));
            logmanager.Logflags[3] = true;
            solveinput.viewportalflag = true;
        }
        else if (x == 1018)
        {
            logmanager.Logflags[4] = true;
            asset.increaseFavorability(25);
            string[] str = new string[2];
            str[0] = "同步率有所变化";
            str[1] = "日志已更新";
            StartCoroutine(messagetip(str));
        }
        else if (x == 1019)
        {
            logmanager.Logflags[5] = true;
            asset.decreaseFavorability(-10);
            string[] str = new string[2];
            str[0] = "同步率有所变化";
            str[1] = "日志已更新";
            StartCoroutine(messagetip(str));
        }
        else if (x == 1020)
        {
            solveinput.viewsiflag = true;
            gridcontent.sisee = true;
        }
        else if (x == 1024)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[6] = true;
        }
        else if (x == 1025)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[6] = true;
        }
        else if (x == 1027)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[7] = true;
        }
        else if (x == 1029)
        {
            StartCoroutine(messagetip("日志已更新"));
            logmanager.Logflags[7] = true;
        }
        else if (x == 1030)
        {
            logmanager.Logflags[8] = true;
            asset.decreaseFavorability(-10);
            string[] str = new string[2];
            str[0] = "同步率有所变化";
            str[1] = "日志已更新";
            StartCoroutine(messagetip(str));
        }
        else if (x == 1040)
        {
            asset.increaseFavorability(15);
            StartCoroutine(messagetip("同步率有所变化"));
        }
        else if (x == 2005)
        {
            asset.increaseHp(40);
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[0] = true;
            notepoint[0].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2006)
        {
            asset.increaseHp(40);
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[0] = true;
            notepoint[0].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2024)
        {
            asset.increaseResource(60);
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[3] = true;
            notepoint[2].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2015)
        {
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[1] = true;
            notepoint[1].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2016)
        {
            asset.increaseFavorability(25);
            StartCoroutine(messagetip("同步率有所变化"));
        }
        else if (x == 2018)
        {
            asset.increaseFavorability(25);
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[2] = true;
            notepoint[1].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2030)
        {
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[5] = true;
            notepoint[3].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2031)
        {
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[4] = true;
            notepoint[3].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2034)
        {
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[6] = true;
            notepoint[4].SetActive(true);
            StartCoroutine(messagetip(str));
        }
        else if (x == 2035)
        {
            string[] str = new string[2];
            str[0] = "生命值提高";
            str[1] = "日志已更新";
            noteflags[6] = true;
            notepoint[4].SetActive(true);
            StartCoroutine(messagetip(str));
        }
    }

    public void note1()
    {
        if (noteflags[0])
        {
            notepoint[0].SetActive(false);
            Image image = notes[0].GetComponent<Image>();
            image.sprite = notes[0].GetComponent<PictureContainer>().images[1];
        }
    }
    public void note2()
    {
        if (noteflags[1])
        {
            notepoint[1].SetActive(false);
            Image image = notes[1].GetComponent<Image>();
            image.sprite = notes[1].GetComponent<PictureContainer>().images[1];
        }
        if (noteflags[2])
        {
            notepoint[1].SetActive(false);
            Image image = notes[1].GetComponent<Image>();
            image.sprite = notes[1].GetComponent<PictureContainer>().images[2];
        }
    }
    public void note3()
    {
        if (noteflags[3])
        {
            notepoint[2].SetActive(false);
            Image image = notes[2].GetComponent<Image>();
            image.sprite = notes[2].GetComponent<PictureContainer>().images[1];
        }
    }
    public void note4()
    {
        if (noteflags[4])
        {
            notepoint[3].SetActive(false);
            Image image = notes[3].GetComponent<Image>();
            image.sprite = notes[3].GetComponent<PictureContainer>().images[1];
        }
        if (noteflags[5])
        {
            notepoint[3].SetActive(false);
            Image image = notes[3].GetComponent<Image>();
            image.sprite = notes[3].GetComponent<PictureContainer>().images[2];
        }
    }
    public void note5()
    {
        if (noteflags[6])
        {
            notepoint[4].SetActive(false);
            Image image = notes[4].GetComponent<Image>();
            image.sprite = notes[4].GetComponent<PictureContainer>().images[1];
        }
    }


    public void TwoButtonDisplay(int x)
    {
        twochoice.SetActive(true);

        int t = x;
        int i;
        for (i = 0; (t - 1000) > 0; t -= 1000, i++) ;
        string str = String.Empty;
        str = container.choices[i][t][0];

        Debug.Log(str);

        left.text = str;

        str = container.choices[i][t][1];

        Debug.Log(str);

        right.text = str;
    }

    public void OneButtonDisplay(int x)
    {
        onechoice.SetActive(true);
        int t = x;
        int i;
        for (i = 0; (t - 1000) > 0; t -= 1000, i++) ;
        string str = String.Empty;
        str = container.choices[i][t][0];
        middle.text = str;
    }

    public void LeftButton()
    {
        int x = line.get();
        int n = 0;
        int t = x;
        while (t >= 1000)
        {
            t -= 1000;
            n++;
        }
        line.change(NodeTable[n][t][2]);
        status = 0;
        twochoice.SetActive(false);
    }
    public void RightButton()
    {
        int x = line.get();
        int n = 0;
        int t = x;
        while (t >= 1000)
        {
            t -= 1000;
            n++;
        }
        line.change(NodeTable[n][t][3]);

        status = 0;
        twochoice.SetActive(false);
    }

    public void MiddleButton()
    {
        int x = line.get();
        int n = 0;
        int t = x;
        while (t >= 1000)
        {
            t -= 1000;
            n++;
        }
        line.change(NodeTable[n][t][2]);

        status = 0;
        onechoice.SetActive(false);
    }

    public void IncidentCheck(GridContent.Content content, int step)
    {
        if (step == 6 && flags[0])
        {
            flags[0] = false;
            line.insert(1010);
        }
        if (content == GridContent.Content.Portal && flags[1])
        {
            flags[1] = false;
            line.insert(1017);
        }
        if (content == GridContent.Content.specialitem1 && flags[2])
        {
            if (!flags[1])
            {
                flags[2] = false;
                line.insert(1021);
            }
        }
        if (content == GridContent.Content.specialitem2 && flags[3])
        {
            if (!flags[1])
            {
                flags[3] = false;
                line.insert(1026);
            }
        }
        if (content == GridContent.Content.Portal && flags[4] && !flags[2] && !flags[3])
        {
            flags[4] = false;
            line.insert(1031);
        }
        if (flags[6] && (content == GridContent.Content.MElectric || content == GridContent.Content.MResource || content == GridContent.Content.MFirstAid))
        {
            flags[6] = false;
            line.insert(2007);
        }
        if (content == GridContent.Content.Incident && flags[5])
        {
            flags[5] = false;
            line.insert(2001);
        }
        else if (content == GridContent.Content.Incident && flags[7] && !flags[5])
        {
            flags[7] = false;
            line.insert(2012);
        }
        else if (content == GridContent.Content.Incident && flags[8] && !flags[7])
        {
            flags[8] = false;
            line.insert(2019);
        }
        else if (content == GridContent.Content.Incident && flags[9] && !flags[8])
        {
            flags[9] = false;
            line.insert(2025);
        }
        else if (content == GridContent.Content.Incident && flags[10] && !flags[9])
        {
            flags[10] = false;
            line.insert(2032);
        }



    }

    void Update()
    {
        if (line.length > 0 && status == 0 && message.activeSelf == true)//有剧情时且可演出时
        {
            status = 1;

            Debug.Log(line.get());

            int x = line.get();
            if (x > 0 && x < 1000)
            {
                sr = new StreamReader("Assets/text/0/" + x.ToString() + ".txt", System.Text.Encoding.Default);
                string str = String.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    //tbx_content.Text = tbx_content.Text + str + '\n';
                }
                sr.Close();
                line.delete();
            }
            else if (x > 1000 && x < 3000)
            {
                getwords(line.get());
            }
            else if (x > 3000 && x < 4000)
            {

            }
            else if (x > 4000 && x < 5000)
            {

            }
        }
        else if (status == 1 && line.length != 0)
        {
            NodeNodeNode.SetActive(false);
            printwords();

        }
        else if (line.length == 0 && status == 1)//无剧情时
        {
            status = 0;
        }

        switch (lastnode)
        {
            case 1040:
            case 1041:
            case 1045:
            case 1047:
            case 1048: UnityEngine.SceneManagement.SceneManager.LoadScene("lose"); break;
        }
    }
}
