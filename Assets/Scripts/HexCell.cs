using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这是一个单元格类，保存每个格子的属性
public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;//格子的颜色
    public int status;//0为未知，1为可探测，2为已通过
}
