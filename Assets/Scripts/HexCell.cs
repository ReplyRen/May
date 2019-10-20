using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//这是一个单元格类，保存每个格子的属性
public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color=Color.gray;//格子的颜色
    public int status;//0为未知，1为当前，2为可探测,3为已通过,4为不可通过
}
