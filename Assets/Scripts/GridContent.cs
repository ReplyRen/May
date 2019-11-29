using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

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
        MResource,//怪物+资源
        MElectric,//怪物+电力
        MFirstAid,//怪物+急救箱
        Chip,
        Incident,//事件，目前无触发效果，待完善
        Portal,//传送门，目前无触发效果，待完善
        specialitem1,
        specialitem2
    };
    [HideInInspector]
    HexGrid grid;
    [HideInInspector]
    PlayerAsset asset;//玩家资源中控
    [HideInInspector]
    SolveInput solve;
    [HideInInspector]
    public struct content//网格内容及数值
    {
        public Content con;
        public int val;
    };
    [HideInInspector]
    public int flag = 0;//用于判断移动时资源的情况,0为正常移动，1为物资不足，2为电力不足，3为二者均不足
    [HideInInspector]
    public content[] contents;
    public int ResourceNum;//资源格总数
    public int ElectricNum;//电力格总数
    public int FirstAidNum;//急救箱总数
    public int MResourceNum;//怪物+资源总数
    public int MElectricNum;//怪物+电力总数
    public int MFirstAidNum;//怪物+急救箱总数
    public int ChipNum;//芯片数量
    public int IncidentNum;//事件总数
    public int Rmin = 1;//单格资源最小值
    public int Rmax = 3;//单格资源最大值
    public int Emin = 1;//单格电力最小值
    public int Emax = 3;//单格电力最大值
    public int Fmin = 1;//单格急救包最小值
    public int Fmax = 3;//单格急救包最大值
    public int MonsterHarm = 30;//怪物伤害值

    //内部逻辑用
    private int TotalNum;
    private int Total;
    private int Resource;
    private int Electric;
    private int FirstAid;
    private int MResource;
    private int MElectric;
    private int MFirstAid;
    private int Chip;
    private int Incident;
    private int Nothing;
    private int specialitem1;
    private int specialitem2;
    [HideInInspector]
    public int Portal;
    private Coroutine coroutine;
    public TipController tip;
    private int[] printlist;
    private int printlistlen;
    private int gridnow;
    private int[] Around;
    private int[] MonsterNum;
    public int portalimage;
    public int si1image;
    public int si2image;
    public bool portalsee;
    public bool sisee;

    public MessageManager messagemanager;

    public void BuildContent(int num)//初始化，在Grid创建中调用
    {
        specialitem1 = specialitem2 = 1;
        contents = new content[num];
        TotalNum = num;
        Total = num;
        Nothing = Total - ResourceNum - ElectricNum - FirstAidNum - MResourceNum - MElectricNum - MFirstAidNum - ChipNum - IncidentNum - 2 - 2;//2为起点加终点+2个specialitem
        Resource = ResourceNum;
        Electric = ElectricNum;
        FirstAid = FirstAidNum;
        MResource = MResourceNum;
        MElectric = MElectricNum;
        MFirstAid = MFirstAidNum;
        Incident = IncidentNum;
        Chip = ChipNum;
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        asset = GameObject.FindWithTag("Player").GetComponent<PlayerAsset>();
        solve = GameObject.FindWithTag("Player").GetComponent<SolveInput>();
        do
        {
            Portal = Random.Range(1, Total);
        } while (Portal == 2 + 5 * grid.width + 5 / 2);
        printlist = new int[num];
        MonsterNum = new int[num];
        foreach (int i in MonsterNum)
            MonsterNum[i] = 1;
        portalsee = sisee = false;
    }

    public void setcontent(int x)//随机生成下标为x的格的内容
    {
        if (Total <= 0)
        {
            Debug.Log("生成内容数量出错");

        }
        int n = Random.Range(1, Total);
        if (x == Portal)
        {
            contents[x].con = Content.Portal;
            contents[x].val = 0;
            grid.cells[x].status = 3;
        }
        else if (x == 2 + 5 * grid.width + 5 / 2)//起点
        {
            contents[x].con = Content.Nothing;
            contents[x].val = 0;
        }
        else if (n <= Resource)
        {
            contents[x].con = Content.Resource;
            contents[x].val = Random.Range(Rmin, Rmax + 1);
            Resource--;
        }
        else if (n <= Resource + Electric)
        {
            contents[x].con = Content.Electric;
            contents[x].val = Random.Range(Emin, Emax + 1);
            Electric--;
        }
        else if (n <= Resource + Electric + FirstAid)
        {
            contents[x].con = Content.FirstAid;
            contents[x].val = Random.Range(Fmin, Fmax + 1);
            FirstAid--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource)
        {
            contents[x].con = Content.MResource;
            contents[x].val = Random.Range(Rmin / 2, Rmax / 2 + 1);
            MResource--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric)
        {
            contents[x].con = Content.MElectric;
            contents[x].val = Random.Range(Emin / 2, Emax / 2 + 1);
            MElectric--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric + MFirstAid)
        {
            contents[x].con = Content.MFirstAid;
            contents[x].val = 1;
            MFirstAid--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric + MFirstAid + Incident)
        {
            contents[x].con = Content.Incident;
            contents[x].val = 0;
            Incident--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric + MFirstAid + Incident + Chip)
        {
            contents[x].con = Content.Chip;
            contents[x].val = 1;
            Chip--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric + MFirstAid + Incident + Chip + specialitem1)
        {
            contents[x].con = Content.specialitem1;
            contents[x].val = 1;
            specialitem1--;
        }
        else if (n <= Resource + Electric + FirstAid + MResource + MElectric + MFirstAid + Incident + Chip + specialitem1 + specialitem2)
        {
            contents[x].con = Content.specialitem2;
            contents[x].val = 1;
            specialitem2--;
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
        if (flag < 2)
        {
            foreach (int i in index)
            {
                if (grid.cells[i].status != 3)
                {
                    grid.cells[i].status = 2;
                    grid.images[i].enabled = true;
                }
            }
        }
    }

    public void detect(int index)
    {
        if (grid.cells[index].status != 3)
        {
            grid.cells[index].status = 2;
            grid.images[index].enabled = true;
        }

    }

    public void pass(int i, int[] TextAround)//移动高对应格子
    {
        int k = 0;
        gridnow = i;
        Around = TextAround;

        flag = asset.movecost();

        grid.cells[i].status = 3;
        //grid.texts[i].enabled = true;
        //grid.images[i].enabled = true;
        for (int j = 1; j < 4; j++)
        {
            if ((contents[TextAround[j * 4]].con == Content.MResource) || (contents[TextAround[j * 4]].con == Content.MElectric) || (contents[TextAround[j * 4]].con == Content.MFirstAid)) k++;
            if ((contents[TextAround[j * 4 + 1]].con == Content.MResource) || (contents[TextAround[j * 4 + 1]].con == Content.MElectric) || (contents[TextAround[j * 4 + 1]].con == Content.MFirstAid)) k++;
        }
        //grid.texts[i].text = k.ToString();
        switch (contents[i].con)
        {
            case Content.Resource: asset.increaseResource(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Electric: asset.increaseElectric(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.FirstAid: asset.increaseFirstAid(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.MResource: asset.increaseResource(contents[i].val); asset.decreaseHp(MonsterHarm); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.MElectric: asset.increaseElectric(contents[i].val); asset.decreaseHp(MonsterHarm); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.MFirstAid: asset.increaseFirstAid(contents[i].val); asset.decreaseHp(MonsterHarm); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.Chip: asset.increaseChip(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Nothing: coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Incident:
                asset.increaseIncident(1); grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
            case Content.Portal:
                ArrivePortal(); grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
            case Content.specialitem1:
                if (!messagemanager.flags[1])
                {
                    coroutine = StartCoroutine(PassNormal(i, k));
                }
                else
                {
                    ArrivePortal();
                    grid.images[i].enabled = false;
                    grid.texts[i].enabled = true;
                    grid.texts[i].text = k.ToString();
                }
                break;
            case Content.specialitem2:
                if (!messagemanager.flags[1])
                {
                    coroutine = StartCoroutine(PassNormal(i, k));
                }
                else
                {
                    ArrivePortal();
                    grid.images[i].enabled = false;
                    grid.texts[i].enabled = true;
                    grid.texts[i].text = k.ToString();
                }
                break;
        }

        if (asset.Hp == 0)
        {
            Debug.Log("Hp=0");
            SceneManager.LoadScene("lose");
        }
        tip.passcheck(contents[i].con);
    }

    void printimage()
    {
        if (portalsee && gridnow != portalimage)
            grid.images[portalimage].enabled = true;
        if (sisee && gridnow != si1image)
            grid.images[si1image].enabled = true;
        if (sisee && gridnow != si2image)
            grid.images[si2image].enabled = true;
    }

    IEnumerator PassMonster(int i, int k)
    {
        grid.images[i].enabled = true;
        grid.images[i].sprite = grid.contentSprite[4];
        yield return new WaitForSeconds(1f);
        grid.images[i].enabled = false;
        grid.texts[i].enabled = true;
        grid.texts[i].text = k.ToString();

        contents[i].con = Content.Nothing;
        grid.images[i].sprite = null;
        grid.images[i].color = new Color(0, 0, 0, 0);
        if (solve.CurrentText != i)
            grid.texts[i].enabled = false;
    }

    IEnumerator PassNormal(int i, int k)
    {
        grid.texts[i].enabled = true;
        grid.texts[i].text = k.ToString();
        yield return new WaitForSeconds(0f);

        contents[i].con = Content.Nothing;
        grid.images[i].sprite = null;
        grid.images[i].color = new Color(0, 0, 0, 0);
    }

    public void start(int i, int[] TextAround)//移动高对应格子
    {
        int k = 0;

        grid.cells[i].status = 3;
        //grid.texts[i].enabled = true;
        //grid.images[i].enabled = true;
        for (int j = 1; j < 4; j++)
        {
            if ((contents[TextAround[j * 4]].con == Content.MResource) || (contents[TextAround[j * 4]].con == Content.MElectric) || (contents[TextAround[j * 4]].con == Content.MFirstAid)) k++;
            if ((contents[TextAround[j * 4 + 1]].con == Content.MResource) || (contents[TextAround[j * 4 + 1]].con == Content.MElectric) || (contents[TextAround[j * 4 + 1]].con == Content.MFirstAid)) k++;
        }
        //grid.texts[i].text = k.ToString();
        switch (contents[i].con)
        {
            case Content.Resource: asset.increaseResource(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Electric: asset.increaseElectric(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.FirstAid: asset.increaseFirstAid(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.MResource: asset.increaseResource(contents[i].val); asset.decreaseHp(MonsterHarm * MonsterNum[i]); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.MElectric: asset.increaseElectric(contents[i].val); asset.decreaseHp(MonsterHarm * MonsterNum[i]); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.MFirstAid: asset.increaseFirstAid(contents[i].val); asset.decreaseHp(MonsterHarm * MonsterNum[i]); coroutine = StartCoroutine(PassMonster(i, k)); break;
            case Content.Chip: asset.increaseChip(contents[i].val); coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Nothing: coroutine = StartCoroutine(PassNormal(i, k)); break;
            case Content.Incident:
                asset.increaseIncident(1); grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
            case Content.Portal:
                ArrivePortal(); grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
            case Content.specialitem1:
                grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
            case Content.specialitem2:
                grid.images[i].enabled = false; grid.texts[i].enabled = true;
                grid.texts[i].text = k.ToString(); break;
        }
        getimages();
    }

    void getimages()
    {
        for (int i = 0; i < grid.images.Length; i++)
        {
            if (contents[i].con == Content.specialitem1)
                si1image = i;
            if (contents[i].con == Content.specialitem2)
                si2image = i;
            if (contents[i].con == Content.Portal)
                portalimage = i;
        }
    }

    public void lostAround(int[] index)//移动后原本被探测到的格恢复到未知
    {
        foreach (int i in index)
        {
            if (grid.cells[i].status != 3)
            {
                grid.cells[i].status = 0;
                grid.images[i].enabled = false;
            }
            else
            {
                if (contents[i].con != Content.Portal)
                    grid.images[i].enabled = false;
            }

        }

    }

    public void JudgePortal(int i)
    {
        if (contents[i].con == Content.Portal)
            grid.images[i].enabled = true;
    }

    public void ArrivePortal()
    {
        if (asset.IncidentNeed == asset.Incident)
            SceneManager.LoadScene("lose");
    }

    public string getcontent(int x)//获得下标为x的网格的内容
    {
        return contents[x].con.ToString();
    }
    public string getcontent(HexCell cell)//获得下标为x的网格的内容
    {
        int x = cell.coordinates.X + cell.coordinates.Z * grid.width + cell.coordinates.Z / 2;
        return contents[x].con.ToString();
    }
    public string ReturnContent(int x)
    {
        return contents[x].con.ToString();
    }

    public HexCell[] TypePrint(string type)
    {
        printlistlen = 0;
        switch (type)
        {
            case "monster": GetPrintElement(Content.MResource); GetPrintElement(Content.MElectric); GetPrintElement(Content.MFirstAid); break;
            case "resource": GetPrintElement(Content.Resource); break;
            case "electric": GetPrintElement(Content.Electric); break;
            case "chip": GetPrintElement(Content.Chip); break;
            case "firstaid": GetPrintElement(Content.FirstAid); break;
        }
        if (type != "monster")
        {
            print(type);
        }
        HexCell[] cells = new HexCell[printlistlen];
        for (int i = 0; i < printlistlen; i++)
        {
            cells[i] = grid.cells[printlist[i]];
        }
        return cells;
    }

    public void GetPrintElement(Content typename)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (typename == contents[i].con)
            {
                printlist[printlistlen] = i;
                printlistlen++;
            }
        }
    }

    public void print(string type)
    {

        for (int i = 0; i < printlistlen; i++)
        {
            grid.images[printlist[i]].enabled = true;
        }
    }

    public void TypeLost()
    {
        for (int i = 0; i < printlistlen; i++)
        {
            if (grid.cells[printlist[i]].status == 0)
                grid.images[printlist[i]].enabled = false;
        }
    }

    public void Double(HexCell x, int count)
    {
        int index = x.coordinates.X + x.coordinates.Z * grid.width + x.coordinates.Z / 2;
        contents[index].val = (count + 1) * contents[index].val;
        MonsterNum[index] = MonsterNum[index] * (count + 1);
    }

    public void Half(HexCell x, int count)
    {
        int index = x.coordinates.X + x.coordinates.Z * grid.width + x.coordinates.Z / 2;
        contents[index].val = contents[index].val / (count + 1);
        MonsterNum[index] = 1;
    }

    private void Update()
    {
        printimage();
    }

}
