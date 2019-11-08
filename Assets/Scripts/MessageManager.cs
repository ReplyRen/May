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
            }
            else if (length == 0)
            {
                MessageButton.GetComponentInChildren<Text>().text = null;
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
    public int status;//0为演出空闲，可演出新剧情，1为剧情演出中
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

    public int[] FlagsControll;//[0]为是否到过portal,[1]为到过的任务点个数

    // Start is called before the first frame update
    void Start()
    {
        line = new queue(MessageButton);
        line.insert(1001);
        status = 0;
        lastnode = 0;
        FlagsControll = new int[] { 0, 0 };
    }

    IEnumerator Display(int x)
    {
        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + ".txt", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        while ((str = sr.ReadLine()) != null)
        {
            yield return new WaitForSeconds(1f);
            Text message = Instantiate<Text>(scrolltext);
            message.text = str;
            message.rectTransform.SetParent(PickMessage.transform, false);
            Scroll.value = 0;
        }
        sr.Close();
        fs.Close();
        switch (x)
        {
            case 1001:
            case 1003:
            case 1004:
            case 1005:
            case 1006:
            case 1010:
            case 1013:
            case 1014:
            case 1017:
            case 1021:
            case 1022:
            case 1023:
            case 1026:
            case 1028:
            case 1031:
            case 1032:
            case 1033:
            case 1034:
            case 1035:
            case 1036:
            case 1039:
            case 1042:
            case 1043:
            case 1044: TwoButtonDisplay(x); break;
            case 1002:
            case 1007:
            case 1008:
            case 1011:
            case 1012:
            case 1018:
            case 1019:
            case 1037:
            case 1038:
            case 1046: OneButtonDisplay(x); break;
            default: lastnode = line.delete(); status = 0; break;
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
        left.text = str;
        left.text = str;

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
        switch (x)
        {
            case 1001: line.change(1002); break;
            case 1003: line.change(1004); break;
            case 1004: line.change(1005); break;
            case 1005:
            case 1006: line.change(1007); break;
            case 1010: line.change(1011); break;
            case 1013: line.change(1014); break;
            case 1014: line.change(1015); break;
            case 1017: line.change(1018); break;
            case 1021: line.change(1022); break;
            case 1022: 
            case 1023: line.change(1024); break;
            case 1026: line.change(1027); break;
            case 1028: line.change(1029); break;
            case 1031: line.change(1032); break;
            case 1032: line.change(1033); break;
            case 1033: line.change(1034); break;
            case 1034: line.change(1036); break;
            case 1035: line.change(1036); break;
            case 1036: line.change(1037); break;
            case 1039: line.change(1040); break;
            case 1042: line.change(1043); break;
            case 1043: line.change(1044); break;
            case 1044: line.change(1045); break;
            case 1002:
            case 1007:
            case 1008:
            case 1011:
            case 1012:
            case 1018:
            case 1019:
            case 1037:
            case 1038:
            case 1046: break;
            default: lastnode = line.delete(); break;
        }

        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + "left.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        str = sr.ReadLine();

        sr.Close();
        fs.Close();

        Text message = Instantiate<Text>(scrolltext);
        message.text = "<肖恩>" + str;
        message.rectTransform.SetParent(PickMessage.transform, false);

        status = 0;
        twochoice.SetActive(false);
    }
    public void RightButton()
    {
        int x = line.get();
        switch (x)
        {
            case 1001: line.change(1002); break;
            case 1003: line.change(1004); break;
            case 1004: line.change(1006); break;
            case 1005:
            case 1006: line.change(1008); break;
            case 1010: line.change(1012); break;
            case 1013: line.change(1014); break;
            case 1014: line.change(1016); break;
            case 1017: line.change(1019); break;
            case 1021: line.change(1023); break;
            case 1022: line.change(1025); break;
            case 1023: line.change(1025); break;
            case 1026: line.change(1028); break;
            case 1028: line.change(1030); break;
            case 1031: line.change(1042); break;
            case 1032: line.change(1033); break;
            case 1033: line.change(1035); break;
            case 1034: line.change(1036); break;
            case 1035: line.change(1036); break;
            case 1036: line.change(1038); break;
            case 1039: line.change(1041); break;
            case 1042: line.change(1046); break;
            case 1043: line.change(1047); break;
            case 1044: line.change(1048); break;
            case 1002:
            case 1007:
            case 1008:
            case 1011:
            case 1012:
            case 1018:
            case 1019:
            case 1037:
            case 1038:
            case 1046: break;
            default: lastnode = line.delete(); break;
        }

        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + "right.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        str = sr.ReadLine();

        sr.Close();
        fs.Close();

        Text message = Instantiate<Text>(scrolltext);
        message.text = "<肖恩>" + str;
        message.rectTransform.SetParent(PickMessage.transform, false);


        status = 0;
        twochoice.SetActive(false);
    }

    public void MiddleButton()
    {
        int x = line.get();
        switch (x)
        {
            case 1001: 
            case 1003: 
            case 1004: 
            case 1005:
            case 1006: 
            case 1010: 
            case 1013: 
            case 1014: 
            case 1017: 
            case 1021: 
            case 1022: 
            case 1023: 
            case 1026: 
            case 1028:
            case 1031: 
            case 1032: 
            case 1033: 
            case 1034: 
            case 1035:
            case 1036: 
            case 1039: 
            case 1042: 
            case 1043: 
            case 1044: break;
            case 1002: line.change(1003); break;
            case 1007: line.change(1009); break;
            case 1008: line.change(1009); break;
            case 1011: line.change(1013); break;
            case 1012: line.change(1013); break;
            case 1018: line.change(1020); break;
            case 1019: line.change(1020); break;
            case 1037: line.change(1039); break;
            case 1038: line.change(1039); break;
            case 1046: line.change(1033); break;
            default: line.delete(); break;
        }

        FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + "middle.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        string str = String.Empty;
        str = sr.ReadLine();

        sr.Close();
        fs.Close();

        Text message = Instantiate<Text>(scrolltext);
        message.text = "<肖恩>" + str;
        message.rectTransform.SetParent(PickMessage.transform, false);


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
            else if (x > 1000 && x < 2000)
            {
                StartCoroutine("Display",line.get());
            }
            else if (x > 2000 && x < 3000)
            {
                sr = new StreamReader("Assets/text/2/" + x.ToString() + ".txt", System.Text.Encoding.Default);
                string str = String.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    Text message = Instantiate<Text>(scrolltext);
                    message.text = str;
                    message.rectTransform.SetParent(PickMessage.transform);
                }
                sr.Close();
                line.delete();
            }
            else if (x > 3000 && x < 4000)
            {

            }
            else if (x > 4000 && x < 5000)
            {

            }
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
