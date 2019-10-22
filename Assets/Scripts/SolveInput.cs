using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class SolveInput : MonoBehaviour
{
    //这是处理点击事件的脚本，现在点击单元格会变色
    public GameObject start;//摄像头赋值
    public float speed;//摄像头移动速度
    private HexCell CurrentCell, NextCell;//存当前格和下一个格
    private HexCell[] CurrentCellAround = new HexCell[6];//存当前周围格
    private HexCell[] NextCellAround = new HexCell[6];//存下一个周围格
    HexGrid grid;

    //方政言加，为实现网格内容探测
    GridContent gridcontent;
    int CurrentText, NextText;
    private int[] CurrentTextAround = new int[6];
    private int[] NextTextAround = new int[6];
    //方政言加end

    private void Awake()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();

        //方政言加，为实现网格内容探测
        gridcontent = GameObject.FindWithTag("Grid").GetComponent<GridContent>();
        //方政言加end

        //设定初始格,并修改颜色
        int index = 2 + 5 * grid.width + 5 / 2;
        CurrentCell = grid.cells[index];
        CurrentCell.color = grid.CellColor[1];
        grid.hexMesh.Triangulate(grid.cells);
        UpdateArround(CurrentCell,CurrentCellAround,CurrentTextAround);
        PrintArround(grid.CellColor[2], CurrentCellAround);
        //方政言加，为实现网格内容探测
        CurrentText = index;
        gridcontent.start(CurrentText,CurrentTextAround);
        gridcontent.detectAround(CurrentTextAround);
        //方政言加end
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        //判断点击格是否是当前格的周围
        if ((Mathf.Abs(NextCell.coordinates.X - CurrentCell.coordinates.X) + Mathf.Abs(NextCell.coordinates.Y - CurrentCell.coordinates.Y) + Mathf.Abs(NextCell.coordinates.Z - CurrentCell.coordinates.Z)) < 3)
        {
            if (NextCell.coordinates.X> Math.Ceiling((double)-NextCell.coordinates.Z/2)&& NextCell.coordinates.X < Math.Ceiling((double)16 -NextCell.coordinates.Z / 2)-1
                && NextCell.coordinates.Z>0&& NextCell.coordinates.Z<grid.height-1)
            {
                cell.color = grid.CellColor[1];
                grid.hexMesh.Triangulate(grid.cells);//以新的颜色重新渲染单元格，每一次只要需要改变格子颜色
                                                     //都需要这句话来重新渲染

                //使被点击格与摄像头Y轴一致并移动
                Vector3 CellPos = cell.transform.position;
                CellPos.y = start.transform.position.y;
                //移动
                start.transform.position = Vector3.MoveTowards(start.transform.position, CellPos, speed);
                //更新当前格
                PrintArround(grid.CellColor[0], CurrentCellAround);
                UpdateArround(NextCell, NextCellAround, NextTextAround);
                PrintArround(grid.CellColor[2], NextCellAround);
                PrintCell(grid.CellColor[3], CurrentCell);
                CurrentCell = NextCell;

                //方政言加，为实现网格内容探测
                gridcontent.lostAround(CurrentTextAround);
                gridcontent.pass(NextText, NextTextAround);
                gridcontent.detectAround(NextTextAround);
                CurrentText = NextText;
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
            if (cell.color != grid.CellColor[3])
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
