using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public int MoveCost = 1;//移动电力消耗

    public void increaseHp(int x)
    {
        Hp += x;
    }

    public void increaseResource(int x)
    {
        Resource += x;
    }

    public void increaseElectric(int x)
    {
        Electric += x;
    }

    public void increaseFirstAid(int x)
    {
        FirstAid += x;
    }

    public void increaseChip(int x)
    {
        Chip += x;
    }

    public void decreaseHp(int x)
    {
        Hp -= x;
    }

    public void decreaseResource(int x)
    {
        Resource -= x;
    }

    public void decreaseElectric()
    {
        Electric -= MoveCost;
    }
    public void decreaseFirstAid(int x)
    {
        FirstAid -= x;
    }

    public void decreaseChip(int x)
    {
        Chip -= x;
    }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
