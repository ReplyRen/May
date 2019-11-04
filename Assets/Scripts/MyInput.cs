using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour
{
    public bool forAndroid=false;
    [HideInInspector]
    public bool isButtonDown = false;
    [HideInInspector]
    public Vector3 position;
    void Update()
    {
        isButtonDown = false;
        if (forAndroid == true)
            AndroidInput();
        else
            WindowsInput();
    }
    private void AndroidInput()
    {
        if (Input.touchCount > 0)
        {
            isButtonDown = true;
            Touch touch = Input.GetTouch(0);
            position = touch.position;
        }

    }
    private void WindowsInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isButtonDown = true;
            position = Input.mousePosition;
        }

    }
}
