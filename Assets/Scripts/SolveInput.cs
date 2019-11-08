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
    private HexCell CurrentCell, NextCell,Cell;//存当前格和下一个格
    private HexCell[] CurrentCellAround = new HexCell[6];//存当前周围格
    private HexCell[] NextCellAround = new HexCell[6];//存下一个周围格
    private CameraController camera;
    HexGrid grid;
    //于沛琦加,绑定地图探索text
    private int StepCount = 1;//记录已探测格子数
    private Text MapExplore;
    //于沛琦加end
    //方政言加，为实现网格内容探测
    GridContent gridcontent;
    [HideInInspector]
    public int CurrentText, NextText;
    private int[] CurrentTextAround = new int[6];
    private int[] NextTextAround = new int[6];
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

    private void Awake()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        //于沛琦加,绑定地图探索text
        MapExplore = GameObject.FindWithTag("MapExplore").GetComponent<Text>();
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
        for (int i = 0; i < grid.height; i++)
        {
            if (i == 0 || i == grid.height - 1)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    index = i * grid.height + j;
                    Cell = grid.cells[index];
                    Cell.color = grid.CellColor[4];
                    grid.hexMesh.Triangulate(grid.cells);
                }
            }
            else
            {
                index = i * grid.height;
                Cell = grid.cells[index];
                Cell.color = grid.CellColor[4];
                grid.hexMesh.Triangulate(grid.cells);
                index = (i + 1) * grid.height - 1;
                Cell = grid.cells[index];
                Cell.color = grid.CellColor[4];
                grid.hexMesh.Triangulate(grid.cells);
                grid.hexMesh.Triangulate(grid.cells);
            }
        }
        index = 2 + 5 * grid.width + 5 / 2;
        CurrentCell = grid.cells[index];
        CurrentCell.color = grid.CellColor[1];
        grid.hexMesh.Triangulate(grid.cells);
        UpdateArround(CurrentCell,CurrentCellAround,CurrentTextAround);
        PrintArround(grid.CellColor[2], CurrentCellAround);

        playerPos = grid.cells[index].transform.position;
        //方政言加，为实现网格内容探测
        CurrentText = index;
        gridcontent.start(CurrentText,CurrentTextAround);
        gridcontent.detectAround(CurrentTextAround);
        //方政言加end
    }

    private void Update()
    {
        if (myInput.isButtonDown)
        {
            HandleInput();
        }
        //于沛琦加,绑定地图探索text
        int GridSum = grid.width * grid.height - 2 * (grid.width + grid.height-2);
        MapRate = (double)StepCount / GridSum;
        MapRate = MapRate*100.0;
        MapExplore.text = MapRate.ToString("0.00")+"%";
        CheckRate();
        //于沛琦加end
    }
    private void CheckRate()
    {
        if (RateFlag[0]==1 && MapRate >= 1.0)
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
        if(Physics.Raycast(inputRay,out hit))
        {
            TouchCell(hit.point);
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
            &&((NextCell.coordinates.X!=CurrentCell.coordinates.X)|| (NextCell.coordinates.Z != CurrentCell.coordinates.Z) || (NextCell.coordinates.Y != CurrentCell.coordinates.Y)))
        {
            if (NextCell.coordinates.X> Math.Ceiling((double)-NextCell.coordinates.Z/2)&& NextCell.coordinates.X < Math.Ceiling((double)16 -NextCell.coordinates.Z / 2)-1
                && NextCell.coordinates.Z>0&& NextCell.coordinates.Z<grid.height-1)
            {
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
            }
        }
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

    void UpdateArround(HexCell cell, HexCell[] CellAround,int[] TextAround)//更新周围格
    {
        int[] index=new int[6];
        index[0] = cell.coordinates.X - 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[1] = cell.coordinates.X - 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[2] = cell.coordinates.X + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        index[3] = cell.coordinates.X + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        index[4] = cell.coordinates.X + 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        index[5] = cell.coordinates.X + 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        for(int item=0;item<6;item++)
        {
            CellAround[item] = grid.cells[(int)index[item]];
            //方政言加，为实现网格内容探测
            TextAround[item] = index[item];
            //方政言加end
        }
    }
    void PrintArround(Color color,HexCell[] CellAround)//对周围格子涂色
    {
        foreach (HexCell cell in CellAround)
        {
            if (cell.color != grid.CellColor[3]&&cell.color != grid.CellColor[4])
            {
                cell.color = color;
            }
        }
        grid.hexMesh.Triangulate(grid.cells);
    }
    void PrintCell(Color color,HexCell cell)//对单个格涂色
    {
        cell.color = color;
        grid.hexMesh.Triangulate(grid.cells);
    }
}
