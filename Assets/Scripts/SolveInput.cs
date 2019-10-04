using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveInput : MonoBehaviour
{
    //这是处理点击事件的脚本，现在点击单元格会变色
    HexGrid grid;
    private void Awake()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
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
        cell.color = grid.touchedColor;
        grid.hexMesh.Triangulate(grid.cells);//以新的颜色重新渲染单元格
    }
    /// <summary>
    /// 通过ciirdinates寻找cell
    /// </summary>
    private HexCell FindCell(HexCoordinates coordinates)
    {
        int index = coordinates.X + coordinates.Z * grid.width + coordinates.Z / 2;
        HexCell cell = grid.cells[index];
        return cell;
    }
}
