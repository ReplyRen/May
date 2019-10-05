using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
 {
    public GameObject start;
    public float speed;
    // Update is called once per frame
    HexGrid grid;
    private void Awake()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<HexGrid>();
    }
    
    void Update()
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
        Vector3 CellPos=cell.transform.position;
        CellPos.y=start.transform.position.y;
        start.transform.position=Vector3.MoveTowards(start.transform.position,CellPos,speed);
        // Vector3 a =cell.transform.position-start.transform.position;
        // transform.Translate(a);
    }
    private HexCell FindCell(HexCoordinates coordinates)
    {
       int index = coordinates.X + coordinates.Z * grid.width + coordinates.Z / 2;
       HexCell cell = grid.cells[index];
       return cell;
    }
}