﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TipController : MonoBehaviour
{
    public GameObject tip;
    int[] line;
    int head;
    int tail;
    int length;
    bool status;
    private bool[] flag;
    public PlayerAsset asset;
    public TipsContainer container;
    public string[] str;
    int strnum;
    int waitting;
    public int wait;
    // Start is called before the first frame update
    void Start()
    {
        tip.SetActive(false);
        flag = new bool[15];//x for 3000+x
        line = new int[20];
        head = tail = length = 0;
        status = true;
        for(int i = 0; i < 15; i++)
        {
            flag[i] = true;
        }
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

    public int delete()
    {
        int x = get();
        if (length > 0)
        {
            head = (head + 1) % 100;
            length--;
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

    public void passcheck(GridContent.Content content)
    {
        switch (content)
        {
            case GridContent.Content.Resource:
            case GridContent.Content.MResource: if (flag[1]) { flag[1] = false; insert(3001); }break;
            case GridContent.Content.Electric:
            case GridContent.Content.MElectric: if (flag[2]) { flag[2] = false; insert(3002); } break;
            case GridContent.Content.Chip: if (flag[3]) { flag[3] = false; insert(3003); } break;
            case GridContent.Content.FirstAid:
            case GridContent.Content.MFirstAid: if (flag[4]) { flag[4] = false; insert(3004); } break;
            case GridContent.Content.Nothing: if (flag[5]) { flag[5] = false; insert(3005); } break;
        }

        if (asset.Electric < 10 && flag[6])
        {
            flag[6] = false;
            insert(3006);
        }
        else if (asset.Electric > 10 && !flag[6])
            flag[6] = true;

        int f = asset.movecost();
        if (f > 1 && flag[7])
        {
            flag[7] = false;
            insert(3007);
        }
        else if (f <= 1 && !flag[7])
            flag[7] = true;

        if (asset.Resource < 10 && flag[8])
        {
            flag[8] = false;
            insert(3008);
        }
        else if (asset.Resource > 10 && !flag[6])
            flag[8] = true;

        if (asset.Hp < 30 && flag[9])
        {
            flag[9] = false;
            insert(3009);
        }
        else if (asset.Hp > 30 && !flag[9])
            flag[9] = true;

        if (asset.Hp < 10 && flag[10])
        {
            flag[10] = false;
            insert(3010);
        }
        else if (asset.Hp > 10 && !flag[6])
            flag[10] = true;
    }

    public void insert3014()
    {
        if (flag[14])
        {
            insert(3014);
            flag[14] = false;
        }
    }

    private void getwords(int x)
    {
        int t = x;
        int i;
        for (i = 0; (t - 1000) > 0; t -= 1000, i++) ;
        string words = container.tips[t];
        tip.SetActive(true);
        str = words.Split('\n');
        strnum = 0;
        waitting = 0;
    }

    private void printwords()
    {
        
        if (waitting == 0)
        {
            if (strnum < str.Length)
            {
                Text text = tip.GetComponentInChildren<Text>();
                text.text = str[strnum];
                strnum++;

            }
            else
            {
                tip.SetActive(false);
                status = true;
            }
        }
        waitting++;
        if (waitting >= wait)
        {
            waitting = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (status&&length>0&&!tip.activeSelf)
        {
            status = false;
            getwords(get());
            delete();
        }
        if (!status && tip.activeSelf)
        {
            printwords();
        }
    }
}
