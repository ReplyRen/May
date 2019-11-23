using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public Button trueButton;
    public Button falseButton;
    public string result = "";
    private void Awake()
    {
        trueButton.onClick.AddListener(TrueButton);
        falseButton.onClick.AddListener(FalseButton);
    }
    private void TrueButton() { result = "true"; }
    private void FalseButton() { result = "false"; }
}
