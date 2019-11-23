using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Device : MonoBehaviour
{
    public Sprite Unlocked;
    public Sprite Cooling;
    public bool Unlock = false;
    public bool inCooling;
    public int Count;
    public Text text;
    private Image myImage;
    private int stepCount;
    private SolveInput player;
    public int coolingCount = 12;
    private int lastStepCount;
    private int countDown;
    public GameObject panel;
    public GameObject select;
    public GameObject inter;
    public bool panelBool = false;
    public bool selectBool = false;
    public bool interBool = false;
    public Image ITFImage;
    public Image CloneImage;
    public Image ProbeImage;
    public GridContent gridContent;
    private void Awake()
    {
        gridContent = GameObject.FindWithTag("Grid").GetComponent<GridContent>();
        if (gameObject.tag == "Probe")
        {
            panel.SetActive(false);
        }
        
        player = GameObject.FindWithTag("Player").GetComponent<SolveInput>();
        text = gameObject.GetComponentInChildren<Text>();
        text.text = "";
        myImage = gameObject.GetComponent<Image>();
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        countDown = coolingCount;
        Unlock = true;
        inCooling = false;
    }
    private void Update()
    {
        if (panelBool == true && gameObject.tag == "Probe")
        {
            ProbeCheck();
        }
        if (selectBool == true && gameObject.tag == "Clone")
        {
            CloneCheck();
        }
        if (interBool == true && gameObject.tag == "Interference")
        {
            InterCheck();
        }


    }
    private void LateUpdate()
    {
        stepCount = player.footCount;
        if (Unlock)
        {
            myImage.sprite = Unlocked;
        }
        if (inCooling)
        {
            if (stepCount > lastStepCount)
                countDown--;
            myImage.sprite = Cooling;
            text.text = countDown.ToString();
            if (countDown == 0)
            {
                text.text = "";
                inCooling = false;
                myImage.sprite = Unlocked;
            }
        }

        lastStepCount = player.footCount;
    }
    private void OnClick()
    {

        if (!Unlock)
            Debug.Log("装置未解锁");
        else if (inCooling)
            Debug.Log("装置在冷却中");
        else
        {

            if (gameObject.tag == "Probe")
            {
                player.locked = 2;
                panelBool = true;
                panel.SetActive(true);
                ProbeImage.enabled = true;
            }
            else if (gameObject.tag == "Clone")
            {
                player.locked = 1;
                selectBool = true;
                select.SetActive(true);
                CloneImage.enabled = true;
            }
            else if(gameObject.tag== "Interference")
            {
                player.locked = 3;
                interBool = true;
                inter.SetActive(true);
                ITFImage.enabled = true;
            }
        }
    }
    private void ProbeCheck()
    {
        string result = panel.GetComponent<Panel>().result;
        if (result != null)
        {
            switch (result)
            {
                case "Concel":
                    panel.SetActive(false);
                    panelBool = false;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;

                case "Elec":
                    Show("electric");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Aid":
                    Show("firstaid");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Dev":
                    Show("chip");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Mos":
                    Show("monster");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Res":
                    Show("resource");       
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;

            }
            panel.GetComponent<Panel>().result = null;
            result = null;
        }

    }
    private void CloneCheck()
    {
        string result = select.GetComponent<Select>().result;
        if (result == "false")
        {
            select.SetActive(false);
            selectBool = false;
            CloneImage.enabled = false;
            player.locked = 0;
        }
        else if (result == "true")
        {
            if (player.cloneCell != null)
            {
                MutiContent(player.cloneCell, 2);
                
                select.SetActive(false);
                selectBool = false;
                CloneImage.enabled = false;
                player.locked = 0;
                player.cloneCell = null;
            }
        }
        select.GetComponent<Select>().result = null;
        result = null;


    }
    private void InterCheck()
    {
        string result = inter.GetComponent<Select>().result;
        if (result == "fales")
        {
            inter.SetActive(false);
            interBool = false;
            ITFImage.enabled = false;
            player.locked = 0;
        }
        inter.GetComponent<Select>().result = null;
        result = null;
    }
    void MutiContent(HexCell cell,int count)
    {
        gridContent.Double(cell, count);
    }
    void Show(string type)
    {
        gridContent.TypePrint(type);
        player.ChangeColor(gridContent.TypePrint(type),type);
    }
}
