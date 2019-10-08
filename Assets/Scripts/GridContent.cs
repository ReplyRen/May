using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContent : MonoBehaviour
{
    //网格内容生成脚本
    //格子种类枚举
    [HideInInspector]
    public enum Content
    {
        Nothing,//空格
        Resource,//资源格
        Electric,//电力
        FirstAid,//急救箱
        Monster,//怪物
        Incident,//事件，目前无触发效果，待完善
        Portal//传送门，目前无触发效果，待完善
    };
    [HideInInspector]
    HexGrid grid;
    [HideInInspector]
    PlayerAsset asset;//玩家资源中控
    [HideInInspector]
    public struct content//网格内容及数值
    {
        public Content con;
        public int val;
    };
    [HideInInspector]
    public content[] contents;
    public int ResourceNum;//资源格总数
    public int ElectricNum;//电力格总数
    public int FirstAidNum;//急救箱总数
    public int MonsterNum;//怪物总数
    public int IncidentNum;//事件总数
    public int Rmin=1;//单格资源最小值
    public int Rmax=3;//单格资源最大值
    public int Emin=1;//单格电力最小值
    public int Emax=3;//单格电力最大值

    //内部逻辑用
    private int TotalNum;
    private int Total;
    private int Resource;
    private int Electric;
    private int FirstAid;
    private int Monster;
    private int Incident;
    private int Nothing;
    private int Portal;

    public void BuildContent(int num)//初始化，在Grid创建中调用
    {
        contents = new content[num];
        TotalNum = num;
        Total = num;
        Nothing = Total - ResourceNum - ElectricNum - FirstAidNum - MonsterNum - IncidentNum - 1;//1为传送门数量
        Resource = ResourceNum;
        Electric = ElectricNum;
        FirstAid = FirstAidNum;
        Monster = MonsterNum;
        Incident = IncidentNum;
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        asset = GameObject.FindWithTag("Player").GetComponent<PlayerAsset>();
        do
        {
            Portal = Random.Range(1, Total);
        } while (Portal == 2 + 5 * grid.width + 5 / 2);
    }

    public void setcontent(int x)//随机生成下标为x的格的内容
    {
        if(Total <= 0)
        {
            Debug.Log("生成内容数量出错");
            
        }
        int n = Random.Range(1, Total);
        if (x == Portal)
        {
            contents[x].con = Content.Portal;
            contents[x].val = 0;
        }
        else if (n <= Resource)
        {
            contents[x].con = Content.Resource;
            contents[x].val = Random.Range(Rmin, Rmax);
            Resource--;
        }
        else if (n <= Resource + Electric)
        {
            contents[x].con = Content.Electric;
            contents[x].val = Random.Range(Emin, Emax);
            Electric--;
        }
        else if (n <= Resource + Electric+FirstAid)
        {
            contents[x].con = Content.FirstAid;
            contents[x].val = 1;
            FirstAid--;
        }
        else if (n <= Resource + Electric + FirstAid+Monster)
        {
            contents[x].con = Content.Monster;
            contents[x].val = 1;
            Monster--;
        }
        else if (n <= Resource + Electric + FirstAid + Monster+Incident)
        {
            contents[x].con = Content.Incident;
            contents[x].val = 0;
            Incident--;
        }
        else
        {
            contents[x].con = Content.Nothing;
            contents[x].val = 0;
            Nothing--;
        }
        Total--;
    }

    public void detectAround(int[] index)//探测周围格子
    {
        int count = 0;
        int detected;
        foreach(int i in index)
        {
            if (grid.cells[i].status != 3) count++;
        }
        detected = Random.Range(0, count);
        count = 0;
        foreach (int i in index)
        {
            if (grid.cells[i].status != 3)
            {
                grid.cells[i].status = 2;
                if (count == detected) grid.texts[i].enabled = true;
                count++;
            }
            else
            {
                grid.texts[i].text = getcontent(i);
                grid.texts[i].enabled = false;
            }
        }

    }

    public void detect(int index)
    {
        if (grid.cells[index].status != 3)
        {
            grid.cells[index].status = 2;
            grid.texts[index].enabled = true;
        }

    }

    public void pass(int i,int[] TextAround)//移动高对应格子
    {
        int k = 0;
        grid.cells[i].status = 3;
        grid.texts[i].enabled = true;
        foreach(int j in TextAround)
        {
            if (contents[j].con == Content.Monster) k++;
        }
        grid.texts[i].text = k.ToString();
        switch (contents[i].con)
        {
            case Content.Resource:asset.increaseResource(contents[i].val);break;
            case Content.Electric:asset.increaseElectric(contents[i].val);break;
            case Content.FirstAid:asset.increaseFirstAid(contents[i].val); break;
            case Content.Monster: asset.decreaseHp(contents[i].val); break;
            case Content.Incident:
            case Content.Portal:
            case Content.Nothing:break;
        }
        asset.decreaseElectric();
        contents[i].con = Content.Nothing;
    }

    public void lostAround(int[] index)//移动后原本被探测到的格恢复到未知
    {
        foreach (int i in index){
            if (grid.cells[i].status != 3)
            {
                grid.cells[i].status = 0;
                grid.texts[i].enabled = false;
            }

        }
        
    }

    public string getcontent(int x)//获得下标为x的网格的内容
    {
        switch (contents[x].con)
        {
            case Content.Resource:
            case Content.Electric:
            case Content.FirstAid:
                return contents[x].con.ToString() + "\n" + contents[x].val.ToString();
            case Content.Incident:
            case Content.Nothing:
            case Content.Portal:
                return contents[x].con.ToString();
            default: return contents[x].con.ToString();
        }            
    }

}
