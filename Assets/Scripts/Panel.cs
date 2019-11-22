using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Button concelBtn;
    public Button elecBtn;
    public Button aidBtn;
    public Button devBtn;
    public Button mosBtn;
    public Button resBtn;
    public string result=null;
    private void Awake()
    {
        concelBtn.onClick.AddListener(ConcelBtn);
        elecBtn.onClick.AddListener(ElecBtn);
        aidBtn.onClick.AddListener(AidBtn);
        devBtn.onClick.AddListener(DevBtn);
        mosBtn.onClick.AddListener(MosBtn);
        resBtn.onClick.AddListener(ResBtn);
    }
    public void ConcelBtn()
    {
        result = "Concel";
    }
    public void ElecBtn() { result = "Elec"; }
    public void AidBtn() { result = "Aid"; }
    public void DevBtn() { result = "Dev"; }
    public void MosBtn() { result = "Mos"; }
    public void ResBtn() { result = "Res"; }
}
