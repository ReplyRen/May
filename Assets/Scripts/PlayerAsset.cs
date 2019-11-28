using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerAsset : MonoBehaviour
{
    private static PlayerAsset _instance;
    public static PlayerAsset Instance
    {
        get
        {
            return _instance;
        }
    }

    public int Hp;
    public int Resource;
    public int Electric;
    public int FirstAid;
    public int Chip;
    public int ElectricMoveCost = 1;//移动电力消耗
    public int ResourceMoveCost = 1;//移动物资消耗
    public int HPMoveCost = 5;//移动物资消耗
    public int IncidentNeed = 0;
    public int Incident = 0;
    public int FirstAidRecover = 10;
    private float Timer = 0f;
    private bool[] Switcher = new bool[5];
    private float[] SwitchTime = new float[5];
    [SerializeField]
    public Transform[] SwitchImage;
    private void Start()
    {
        for(int i = 0; i < Switcher.Length; i++)
        {
            Switcher[i] = false;
            SwitchTime[i] = 0;
        }
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        for (int i = 0; i < Switcher.Length; i++)
        {
            if (Switcher[i])
            {
                if (Timer - SwitchTime[i] > 2)
                {
                    SwitchImage[i].gameObject.SetActive(false);
                }
                else
                {
                    
                    SwitchImage[i].gameObject.SetActive(true);
                }
            }
        }
    }
    public void increaseHp(int x)
    {
        Hp += x;
        if (Hp > 100)
            Hp = 100;
        Switcher[0] = true;
        SwitchTime[0] = Timer;
        SwitchImage[0].GetComponentInChildren<Text>().text = x.ToString();
    }

    public void useFirstAid()
    {
        Hp += FirstAidRecover;
        if (Hp > 100)
            Hp = 100;
        Switcher[4] = true;
        SwitchTime[4] = Timer;
        SwitchImage[4].GetComponentInChildren<Text>().text = FirstAidRecover.ToString();

    }

    public void increaseResource(int x)
    {
        Resource += x;
        Switcher[3] = true;
        SwitchTime[3] = Timer;

    }

    public void increaseElectric(int x)
    {
        Electric += x;
        Switcher[2] = true;
        SwitchTime[2] = Timer;

    }

    public void increaseFirstAid(int x)
    {
        FirstAid += x;
    }

    public void increaseChip(int x)
    {
        Chip += x;
    }

    public void increaseIncident(int x)
    {
        Incident += x;
    }

    public void decreaseHp(int x)
    {
        if ((Hp -= x) < 0) Hp = 0;
        Switcher[1] = true;
        SwitchTime[1] = Timer;

    }

    public void decreaseResource(int x)
    {
        if ((Resource -= x) < 0) Resource = 0;
    }

    public void decreaseFirstAid(int x)
    {
        FirstAid -= x;
    }

    public void decreaseChip(int x)
    {
        Chip -= x;
    }

    public int movecost()
    {
        int flag = 0;
        if (Resource < ResourceMoveCost)
        {
            if ((Hp -= HPMoveCost) < 0) Hp = 0;
            flag = 1;
        }
        else {
            if ((Resource -= ResourceMoveCost) < 0) Resource = 0;
        }
        if (Electric < ElectricMoveCost)
        {
            if (flag == 1)
            {
                flag = 3;
            }
            else
            {
                flag = 2;
            }
        }
        else
        {
            if ((Electric -= ElectricMoveCost) < 0) Electric = 0;
        }
        return flag;
    }


}
