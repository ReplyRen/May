using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
 {
    private float speed = 1f;
    private Vector3 targetPos;
    private SolveInput input;
    //方政言偷偷加一句
    public GameObject op;
    public Camera camera;
    private void Start()
    {
        camera = Camera.main;
        camera.aspect = 1.78f;
        targetPos = transform.position;
        input = GetComponentInChildren<SolveInput>();
        //方政言偷偷加一句
        op.SetActive(true);
    }
    private void LateUpdate()
    {
        if (input.moveflag == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                targetPos = input.playerPos;
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            }
            if ((targetPos - transform.position).magnitude > 1f)
                transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            targetPos = input.targetpos;
            targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if ((targetPos - transform.position).magnitude > 1f)
                transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
    private Vector3 getPoint()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(inputRay, out hit);
        return hit.point;
    }
}