using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveInput : MonoBehaviour
{
    //这是处理点击事件的脚本，现在点击单元格会变色
    public GameObject start;//摄像头赋值
    public float speed;//摄像头移动速度
    private HexCell CurrentCell, NextCell;//存当前格和下一个格
    private HexCell[] CurrentCellAround = new HexCell[6];//存当前周围格
    private HexCell[] NextCellAround = new HexCell[6];//存下一个周围格
    HexGrid grid;
    private void Awake()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
        //设定初始格,并修改颜色
        int index = 2 + 5 * grid.width + 5 / 2;
        CurrentCell = grid.cells[index];
        CurrentCell.color = grid.CellColor[1];
        grid.hexMesh.Triangulate(grid.cells);
        UpdateArround(CurrentCell,CurrentCellAround);
        PrintArround(grid.CellColor[2], CurrentCellAround);
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
            UpdateArround(NextCell, NextCellAround);
            PrintArround(grid.CellColor[2], NextCellAround);
            PrintCell(grid.CellColor[3], CurrentCell);
            CurrentCell = NextCell;
            UpdateArround(CurrentCell,CurrentCellAround);
            PrintCell(grid.CellColor[1], CurrentCell);
        }
    }
    /// <summary>
    /// 通过coordinates寻找cell
    /// </summary>
    private HexCell FindCell(HexCoordinates coordinates)
    {
        int index = coordinates.X + coordinates.Z * grid.width + coordinates.Z / 2;
        HexCell cell = grid.cells[index];
        return cell;
    }

    void UpdateArround(HexCell cell, HexCell[] CellAround)//更新周围格
    {
        int index;
        index = cell.coordinates.X - 1 + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        CellAround[0]= grid.cells[index];
        index = cell.coordinates.X - 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        CellAround[1] = grid.cells[index];
        index = cell.coordinates.X + (cell.coordinates.Z + 1) * grid.width + (cell.coordinates.Z + 1) / 2;
        CellAround[2] = grid.cells[index];
        index = cell.coordinates.X + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        CellAround[3] = grid.cells[index];
        index = cell.coordinates.X + 1 + (cell.coordinates.Z) * grid.width + (cell.coordinates.Z) / 2;
        CellAround[4] = grid.cells[index];
        index = cell.coordinates.X + 1 + (cell.coordinates.Z - 1) * grid.width + (cell.coordinates.Z - 1) / 2;
        CellAround[5] = grid.cells[index];
    }
    void PrintArround(Color color,HexCell[] CellAround)//对周围格子涂色
    {
        foreach (HexCell cell in CellAround)
        {
            if(cell.color!=grid.CellColor[3])cell.color = color;
        }
        grid.hexMesh.Triangulate(grid.cells);
    }
    void PrintCell(Color color,HexCell cell)//对单个格涂色
    {
        cell.color = color;
        grid.hexMesh.Triangulate(grid.cells);
    }
}
