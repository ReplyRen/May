using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SolveInput : MonoBehaviour
{
    //这是处理点击事件的脚本，现在点击单元格会变色
    public GameObject start;//摄像头赋值
    public float speed;//摄像头移动速度
    private HexCell CurrentCell, NextCell, Cell;//存当前格和下一个格
    private HexCell[] CurrentCellAround = new HexCell[18];//存当前周围格
    private HexCell[] NextCellAround = new HexCell[18];//存下一个周围格
    private CameraController camera;
    HexGrid grid;
    //于沛琦加,绑定地图探索text
    public int StepCount = 1;//记录已探测格子数
    private Text MapExplore;
    private Image ExplorePic;
    //于沛琦加end
    //方政言加，为实现网格内容探测
    GridContent gridcontent;
    [HideInInspector]
    public int CurrentText, NextText;
    private int[] CurrentTextAround = new int[18];
    private int[] NextTextAround = new int[18];
    //方政言加end
    [HideInInspector]
    public Vector3 playerPos;//用于摄像机移动
    [HideInInspector]
    public Vector3 targetpos;
    [HideInInspector]
    public int moveflag = 0;//0时摄像机正常移动，1时为触发探索率后的移动
    private double MapRate;
    private int[] RateFlag = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public MessageManager Message;

    private MyInput myInput;
    public int footCount = 0;
    public int locked = 0;
    Vector3 lastHitPoint;
    public HexCell cloneCell;
    public GameObject muti;
    public HexCell[] cloneAround;
    private bool shaded = false;
    public HexCell[] interferenceCells;
    private void Awake()
    {
        locked = 0;
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        //于沛琦加,绑定地图探索text
        MapExplore = GameObject.FindWithTag("MapExplore").GetComponent<Text>();
        ExplorePic = GameObject.FindWithTag("ExplorePic").GetComponent<Image>();
        ExplorePic.fillAmount = 0.0f;
        //于沛琦加end
        //方政言加，为实现网格内容探测
        gridcontent = GameObject.FindWithTag("Grid").GetComponent<GridContent>();
        //方政言加end
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        if (camera == null)
            Debug.Log("camera not set");

        myInput = camera.GetComponent<MyInput>();
        //设定初始格,并修改颜色
        int index;
        //for (int i = 0; i < grid.height; i++)
        //{
        //    if (i == 0 || i == grid.height - 1)
        //    {
        //        for (int j = 0; j < grid.width; j++)
        //        {
        //            index = i * grid.height + j;
        //            Cell = grid.cells[index];
        //            Cell.color = grid.CellColor[4];
        //            grid.hexMesh.Triangulate(grid.cells);
        //        }
        //    }
        //    else
        //    {
        //        index = i * grid.height;
        //        Cell = grid.cells[index];
        //        Cell.color = grid.CellColor[4];
        //        grid.hexMesh.Triangulate(grid.cells);
        //        index = (i + 1) * grid.height - 1;
        //        Cell = grid.cells[index];
        //        Cell.color = grid.CellColor[4];
        //        grid.hexMesh.Triangulate(grid.cells);
        //        grid.hexMesh.Triangulate(grid.cells);
        //    }
        //}
        index = 2 + 5 * grid.width + 5 / 2;
        CurrentCell = grid.cells[index];
        CurrentCell.color = grid.CellColor[1];
        grid.hexMesh.Triangulate(grid.cells);
        UpdateArround(CurrentCell, CurrentCellAround, CurrentTextAround);
        PrintArround(grid.CellColor[2], CurrentCellAround);

        playerPos = grid.cells[index].transform.position;
        //方政言加，为实现网格内容探测
        CurrentText = index;
        gridcontent.start(CurrentText, CurrentTextAround);
        gridcontent.detectAround(CurrentTextAround);
        //方政言加end
    }

    private void Update()
    {
        if (locked == 1)
        {
            SelectCell(lastHitPoint);
            if (myInput.isButtonDown)
            {
                HandleInput();
            }
        }
        if (locked == 3)
        {
            InterferenceSelectCell(lastHitPoint);
            if (myInput.isButtonDown)
            {
                HandleInput();
            }
        }
        if (myInput.isButtonDown)
        {
            HandleInput();
        }
        //于沛琦加,绑定地图探索text
        int GridSum = grid.width * grid.height - 2 * (grid.width + grid.height - 2);
        MapRate = (double)StepCount / GridSum;
        ExplorePic.fillAmount = (float)MapRate;
        MapRate = MapRate * 100.0;
        MapExplore.text = MapRate.ToString("0.00") + "%";
        CheckRate();
        //于沛琦加end
    }
    private void CheckRate()
    {
        if (RateFlag[0] == 1 && MapRate >= 1.0)
        {
            RateFlag[0] = 0;
            targetpos = grid.cells[gridcontent.Portal].transform.position;
            Vector3 oldposition = camera.transform.position;
            StartCoroutine(MapRateAchieve(oldposition));
        }
    }
    IEnumerator MapRateAchieve(Vector3 oldposition)
    {
        moveflag = 1;
        Debug.Log("change moveflag to 1");
        yield return new WaitForSeconds(2f);
        targetpos = oldposition;
        yield return new WaitForSeconds(1f);
        moveflag = 0;
        Debug.Log("change moveflag to 0");
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(myInput.position);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && locked == 0)
        {
            TouchCell(hit.point);
        }
        if (Physics.Raycast(inputRay, out hit) && locked == 1)
        {
            CloneCell(hit.point);
        }
        if (Physics.Raycast(inputRay, out hit) && locked == 3)
        {
            InterferenceCell(hit.point);
        }
    }
    void TouchCell(Vector3 position)
    {
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        HexCell cell = FindCell(coordinates);

        //保存保存下一格数据，方便对比
        NextCell = cell;
        //判断点击格是否是当前格的周围且不能是当前格
        if (((Mathf.Abs(NextCell.coordinates.X - CurrentCell.coordinates.X) + Mathf.Abs(NextCell.coordinates.Y - CurrentCell.coordinates.Y) + Mathf.Abs(NextCell.coordinates.Z - CurrentCell.coordinates.Z)) < 3)
            && ((NextCell.coordinates.X != CurrentCell.coordinates.X) || (NextCell.coordinates.Z != CurrentCell.coordinates.Z) || (NextCell.coordinates.Y != CurrentCell.coordinates.Y)))
        {
            //if (NextCell.coordinates.X> Math.Ceiling((double)-NextCell.coordinates.Z/2)&& NextCell.coordinates.X < Math.Ceiling((double)16 -NextCell.coordinates.Z / 2)-1
            //    && NextCell.coordinates.Z>0&& NextCell.coordinates.Z<grid.height-1)
            //{
            lastHitPoint = position;
            footCount++;
            //走到新格子就加一
            if (NextCell.status != 3) StepCount++;

            cell.color = grid.CellColor[1];
            grid.hexMesh.Triangulate(grid.cells);//以新的颜色重新渲染单元格，每一次只要需要改变格子颜色
                                                 //都需要这句话来重新渲染
            playerPos = cell.transform.position;
            //使被点击格与摄像头Y轴一致并移动
            //Vector3 CellPos = cell.transform.position;
            //CellPos.y = start.transform.position.y;
            //Debug.Log(CellPos);

            //移动
            //start.transform.position = Vector3.MoveTowards(start.transform.position, CellPos, speed * Time.deltaTime); ;
            //更新当前格
            PrintArround(grid.CellColor[0], CurrentCellAround);
            UpdateArround(NextCell, NextCellAround, NextTextAround);
            PrintArround(grid.CellColor[2], NextCellAround);
            PrintCell(grid.CellColor[3], CurrentCell);
            CurrentCell = NextCell;

            //方政言加，为实现网格内容探测
            //gridcontent.CoroutineStop();
            gridcontent.lostAround(CurrentTextAround);
            gridcontent.pass(NextText, NextTextAround);
            gridcontent.detectAround(NextTextAround);
            grid.texts[CurrentText].enabled = false;
            gridcontent.JudgePortal(CurrentText);
            CurrentText = NextText;
            Message.IncidentCheck(gridcontent.contents[CurrentText].con, StepCount);
            //方政言加end

            UpdateArround(CurrentCell, CurrentCellAround, CurrentTextAround);
            PrintCell(grid.CellColor[1], CurrentCell);
            //}
        }
    }
    void CloneCell(Vector3 position)
    {
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        HexCell cell = FindCell(coordinates);
        if (((Mathf.Abs(cell.coordinates.X - CurrentCell.coordinates.X) + Mathf.Abs(cell.coordinates.Y - CurrentCell.coordinates.Y) + Mathf.Abs(cell.coordinates.Z - CurrentCell.coordinates.Z)) < 3)
    && ((cell.coordinates.X != CurrentCell.coordinates.X) || (cell.coordinates.Z != CurrentCell.coordinates.Z) || (cell.coordinates.Y != CurrentCell.coordinates.Y)))
        {
            cloneCell = cell;
            muti.transform.position = cell.transform.position;
        }
    }
    void InterferenceCell(Vector3 position)
    {
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        HexCell cell = FindCell(coordinates);
        if (((Mathf.Abs(cell.coordinates.X - CurrentCell.coordinates.X) + Mathf.Abs(cell.coordinates.Y - CurrentCell.coordinates.Y) + Mathf.Abs(cell.coordinates.Z - CurrentCell.coordinates.Z)) < 6)
   && ((cell.coordinates.X != CurrentCell.coordinates.X) || (cell.coordinates.Z != CurrentCell.coordinates.Z) || (cell.coordinates.Y != CurrentCell.coordinates.Y)))
        {
            List<HexCell> cells = new List<HexCell>(oneArround(cell));
            cells.Add(cell);
            PrintArround(grid.CellColor[8], cells.ToArray());
            interferenceCells = cells.ToArray();
        }
    }
    void InterferenceSelectCell(Vector3 position)
    {
        if (shaded == false)
        {
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            HexCell cell = FindCell(coordinates);
            HexCell[] cells = InterferenceAround(cell);
            PrintArround(grid.CellColor[5], cells);
            shaded = true;
        }
    }
    void SelectCell(Vector3 position)
    {
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        HexCell cell = FindCell(coordinates);
        //判断点击格是否是当前格的周围且不能是当前格
        //if (((Mathf.Abs(NextCell.coordinates.X - CurrentCell.coordinates.X) + Mathf.Abs(NextCell.coordinates.Y - CurrentCell.coordinates.Y) + Mathf.Abs(NextCell.coordinates.Z - CurrentCell.coordinates.Z)) < 3)
        //    && ((NextCell.coordinates.X != CurrentCell.coordinates.X) || (NextCell.coordinates.Z != CurrentCell.coordinates.Z) || (NextCell.coordinates.Y != CurrentCell.coordinates.Y)))
        //{
        //更新当前格
        HexCell[] cells = oneArround(cell);
        cloneAround = cells;
        PrintArround(grid.CellColor[5], cells);
        //}
        //}
    }
    /// <summary>
    /// 通过coordinates寻找cell
    /// </summary>
    private HexCell FindCell(HexCoordinates coordinates)
    {
        int index = coordinates.X + coordinates.Z * grid.width + coordinates.Z / 2;
        HexCell cell = grid.cells[index];
        //方政言加，为实现网格内容探测
        NextText = index;
        //方政言加end
        return cell;
    }

    void UpdateArround(HexCell cell, HexCell[] CellAround, int[] TextAround)//更新周围格
    {
        int[] index = new int[18];
        index[0] = cell.coordinates.X - 2 + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[1] = cell.coordinates.X - 2 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[2] = cell.coordinates.X - 2 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;

        index[3] = cell.coordinates.X - 1 + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[4] = cell.coordinates.X - 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[5] = cell.coordinates.X - 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[6] = cell.coordinates.X - 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;

        index[7] = cell.coordinates.X + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[8] = cell.coordinates.X + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[9] = cell.coordinates.X + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[10] = cell.coordinates.X + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;

        index[11] = cell.coordinates.X + 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[12] = cell.coordinates.X + 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[13] = cell.coordinates.X + 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[14] = cell.coordinates.X + 1 + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;

        index[15] = cell.coordinates.X + 2 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[16] = cell.coordinates.X + 2 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[17] = cell.coordinates.X + 2 + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;


        for (int item = 0; item < 18; item++)
        {
            if (index[item] >= 0 && index[item] <= grid.height * grid.width)
            {
                CellAround[item] = grid.cells[(int)index[item]];
                //方政言加，为实现网格内容探测
                TextAround[item] = index[item];
                //方政言加end
            }
        }
    }
    HexCell[] oneArround(HexCell cell)
    {
        int[] index = new int[6];
        index[0] = cell.coordinates.X - 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[1] = cell.coordinates.X - 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[2] = cell.coordinates.X + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[3] = cell.coordinates.X + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[4] = cell.coordinates.X + 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[5] = cell.coordinates.X + 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        HexCell[] CellAround = new HexCell[6];
        for (int item = 0; item < 6; item++)
        {
            if (index[item] >= 0 && index[item] <= grid.height * grid.width)
            {
                CellAround[item] = grid.cells[(int)index[item]];
            }
        }
        return CellAround;
    }
    HexCell[] InterferenceAround(HexCell cell)
    {
        int[] index = new int[18];
        index[0] = cell.coordinates.X - 2 + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[1] = cell.coordinates.X - 2 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[2] = cell.coordinates.X - 2 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;

        index[3] = cell.coordinates.X - 1 + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[4] = cell.coordinates.X - 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[5] = cell.coordinates.X - 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[6] = cell.coordinates.X - 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;

        index[7] = cell.coordinates.X + (cell.coordinates.Z + 2) * grid.width + (cell.coordinates.Z + 2) / 2;
        index[8] = cell.coordinates.X + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[9] = cell.coordinates.X + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[10] = cell.coordinates.X + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;

        index[11] = cell.coordinates.X + 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[12] = cell.coordinates.X + 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[13] = cell.coordinates.X + 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[14] = cell.coordinates.X + 1 + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;

        index[15] = cell.coordinates.X + 2 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[16] = cell.coordinates.X + 2 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[17] = cell.coordinates.X + 2 + (cell.coordinates.Z - 2) * grid.width + (cell.coordinates.Z - 2) / 2;

        HexCell[] CellAround = new HexCell[18];
        for (int item = 0; item < 18; item++)
        {
            if (index[item] >= 0 && index[item] <= grid.height * grid.width)
            {
                CellAround[item] = grid.cells[(int)index[item]];
            }
        }
        return CellAround;
    }
    void PrintArround(Color color, HexCell[] CellAround)//对周围格子涂色
    {
        foreach (HexCell cell in CellAround)
        {
            if (cell != null)
            {
                if (cell.color != grid.CellColor[3] && cell.color != grid.CellColor[4])
                {
                    cell.color = color;
                }
            }
        }
        grid.hexMesh.Triangulate(grid.cells);
    }
    void PrintCell(Color color, HexCell cell)//对单个格涂色
    {
        cell.color = color;
        grid.hexMesh.Triangulate(grid.cells);
    }

    public void ShowColor(HexCell[] cells,string type)
    {
        List<HexCell> list = new List<HexCell>();
        for(int i = 0; i < cells.Length; i++)
        {
            if (Array.IndexOf(CurrentCellAround, cells[i]) == -1)
                list.Add(cells[i]);
        }
        HexCell[] hexCells = list.ToArray();

        if (type == "monster")
        {
            PrintArround(grid.CellColor[7], hexCells); 
        }
        else
            PrintArround(grid.CellColor[6], hexCells);

    }
    public void UnshowColor(HexCell[] cells)
    {
        List<HexCell> list = new List<HexCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            if(cells[i].color!=grid.CellColor[1]&&cells[i].color != grid.CellColor[2]&&cells[i].color != grid.CellColor[3])
                list.Add(cells[i]);
        }
        HexCell[] hexCells = list.ToArray();
        PrintArround(grid.CellColor[0], hexCells);
    }
    public void CloneUnshow()
    {
        List<HexCell> list = new List<HexCell>();
        for (int i = 0; i < cloneAround.Length; i++)
        {
            if (cloneAround[i].color == grid.CellColor[5])
                list.Add(cloneAround[i]);
        }
        HexCell[] hexCells = list.ToArray();
        PrintArround(grid.CellColor[2], hexCells);
    }
    public void InterferenceColorReset()
    {
        for(int i = 0; i < interferenceCells.Length; i++)
        {
            //interferenceCells[i].
        }
    }
}
