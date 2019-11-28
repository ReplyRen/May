using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HexGrid : MonoBehaviour
{

    public Color[] CellColor =new Color[4];

    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    [HideInInspector]
    public HexCell[] cells;
    
    //方政言加，为现实格子内容
    [HideInInspector]
    public Text[] texts;
    [HideInInspector]
    public Image[] images;
    public GridContent gridcontent;
    //方政言加end

    public Text cellLablePrefab;

    //格子边界和图片
    public Image borderPrefab;
    public Image contentImage;
    public Sprite[] contentSprite;//0芯片，1电力，2事件，3医疗包，4怪物，5资源，6特殊道具1
                                  //7特殊道具2，8终点

    private Canvas gridCanvas;
    [HideInInspector]
    public HexMesh hexMesh;

    private void Awake()
    {
        
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];
        
        //方政言加，为现实格子内容
        texts = new Text[height * width];
        gridcontent = GetComponent<GridContent>();
        gridcontent.BuildContent(height * width);
        images = new Image[height * width];
        //方政言加end，为现实格子内容

        for (int z = 0, i = 0; z < height; z++)
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
        cell.color = CellColor[0];


        //原版代码
        //Text label = Instantiate<Text>(cellLablePrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();

        //方政言加，为现实格子内容
        gridcontent.setcontent(i);
        Text label = texts[i] = Instantiate<Text>(cellLablePrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);
        label.text = gridcontent.getcontent(i);
        if (gridcontent.contents[i].con == GridContent.Content.Portal) label.enabled = true;
        else label.enabled = false;
        //gridcontent.setcontent(i);
        //Text label = texts[i] = Instantiate<Text>(cellLablePrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);
        //label.text = gridcontent.getcontent(i);
        //if (gridcontent.contents[i].con == GridContent.Content.Portal) label.enabled = true;
        //else

        //方政言加end

        //边界和图片的实现
        Image border = Instantiate<Image>(borderPrefab);
        border.rectTransform.SetParent(gridCanvas.transform, false);
        border.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);

        Image content = images[i] = Instantiate<Image>(contentImage);
        content.rectTransform.SetParent(gridCanvas.transform, false);
        content.rectTransform.anchoredPosition = new Vector2(positon.x, positon.z);
        string con = gridcontent.ReturnContent(i);
        switch (con)
        {
            case "Nothing":
                content.color = new Color(0, 0, 0, 0);
                break;
            case "Resource":
                content.sprite = contentSprite[5];
                break;
            case "Electric":
                content.sprite = contentSprite[1];
                break;
            case "FirstAid":
                content.sprite = contentSprite[3];
                break;
            case "MResource":
                content.sprite = contentSprite[5];
                break;
            case "MElectric":
                content.sprite = contentSprite[1];
                break;
            case "MFirstAid":
                content.sprite = contentSprite[3];
                break;
            case "Chip":
                content.sprite = contentSprite[0];
                break;
            case "Incident":
                content.sprite = contentSprite[2];
                break;
            case "Portal":
                content.sprite = contentSprite[8];
                break;
            case "specialitem1":
                content.sprite = contentSprite[9];
                break;
            case "specialitem2":
                content.sprite = contentSprite[10];
                break;
            default:
                break;
        }
        if (gridcontent.contents[i].con == GridContent.Content.Portal) content.enabled = true;
        else if (gridcontent.contents[i].con == GridContent.Content.Incident) content.enabled = true;
        else content.enabled = false;
    }
}
