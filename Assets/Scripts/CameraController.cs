using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
 {
    private float speed = 1f;
    private Vector3 targetPos;
    private SolveInput input;
    private void Start()
    {
        targetPos = transform.position;
        input = GetComponentInChildren<SolveInput>();
    }
    private void LateUpdate()
    {
        if (input.moveflag == 0)
        {
            Debug.Log("moving 0");
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
            Debug.Log("moving 1");
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