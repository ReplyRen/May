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

        public queue()
        {
            line = new int[100];
            head = tail = 0;
            length = 0;
        }

        public void insert(int x)
        {
            if (length < 100)
            {
                line[tail] = x;
                tail = (tail + 1) % 100;
                length++;
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

        public void delete()
        {
            if (length > 0)
            {
                head = (head + 1) % 100;
                length--;
            }
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
    public int status;//0为剧情断点，1为剧情演出中
    StreamReader sr;
    public Text scrolltext;
    public GameObject PickMessage;
    public GameObject PickLog;

    // Start is called before the first frame update
    void Start()
    {
        line = new queue();
        line.insert(1001);
        status = 0;
    }

    void Update()
    {
        if (line.length > 0 && status == 0 && PickMessage.activeSelf==true)//有剧情时且可演出时
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
                FileStream fs = new FileStream("Assets/text/1/" + x.ToString() + ".txt", FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
                string str = String.Empty;
                while ((str = sr.ReadLine()) != null)
                {
                    Text message = Instantiate<Text>(scrolltext);
                    message.text = str;
                    Debug.Log(str);
                    message.rectTransform.SetParent(PickMessage.transform,false);
                }
                sr.Close();
                line.delete();
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
    }
}
