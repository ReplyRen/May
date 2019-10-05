using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public Color[] CellColor =new Color[4];

    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    [HideInInspector]
    public HexCell[] cells;

    public Text cellLablePrefab;

    private Canvas gridCanvas;
    [HideInInspector]
    public HexMesh hexMesh;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];

        for(int z = 0, i = 0; z < height; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }
    private void Start()
    {
        hexMesh.Triangulate(cells);
    }
    void CreateCell(int x,int z,int i)//在这里初始化格子的status并通过status设置格子的颜色
    {
        Vector3 positon;
        positon.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        positon.y = 0f;
        positon.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = positon;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = Color.black;
        

        Text label = Instantiate<Text>(cellLablePrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
}
