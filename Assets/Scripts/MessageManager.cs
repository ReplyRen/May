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
    public Text scrolltext;
    public GameObject PickMessage;
    public GameObject PickLog;
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

        FileStream fs = new FileStream("Assets/text/Nodes.txt", FileMode.Open, FileAccess.Read);

        string[] temp, str;

        StreamReader sr = new StreamReader(fs);
        str = new string[100];
        int i;
        for (i = 0; (str[i] = sr.ReadLine()) != null; i++)
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


        sr.Close();
        fs.Close();
    }
    

    private void getwords(int x)
    {
        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + ".txt", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string words = String.Empty;
        words = sr.ReadToEnd();
        str = words.Split('\n');
        sr.Close();
        fs.Close();

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
                    if (lastnode == 1009)
                        tip.insert3014();
                }
            }
        }
    }


    public void TwoButtonDisplay(int x)
    {
        twochoice.SetActive(true);

        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + "left.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        str = sr.ReadLine();

        Debug.Log(str);

        left.text = str;
        sr.Close();
        fs.Close();

        fs = new FileStream("Assets/text/1/" + x.ToString() + "right.txt", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        str = String.Empty;
        str = sr.ReadLine();

        Debug.Log(str);

        right.text = str;
        sr.Close();
        fs.Close();
    }

    public void OneButtonDisplay(int x)
    {
        onechoice.SetActive(true);
        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + "middle.txt", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        str = sr.ReadLine();
        middle.text = str;
        sr.Close();
        fs.Close();
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
        if (step == 6)
        {
            line.insert(1010);
        }
        if(content== GridContent.Content.Portal)
        {
            if(FlagsControll[0] == 0)
            {
                line.insert(1017);
                FlagsControll[0]++;
            }
            else if (FlagsControll[1] >= 2)
            {
                line.insert(1031);
                FlagsControll[0]++;
            }
        }
        if (content == GridContent.Content.Incident)
        {
            if (FlagsControll[1] == 0)
            {
                line.insert(1021);
                FlagsControll[1]++;
            }
            if (FlagsControll[1] == 2)
            {
                line.insert(1026);
                FlagsControll[1]++;
            }
        }
    }

    void Update()
    {
        if (line.length > 0 && status == 0 && message.activeSelf==true)//有剧情时且可演出时
        {
            StreamReader sr;
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
            case 1048: UnityEngine.SceneManagement.SceneManager.LoadScene("lose");break;
        }
    }
}
