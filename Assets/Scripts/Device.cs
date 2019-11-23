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
    public bool panelBool = false;
    public bool selectBool = false;
    public Image ITFImage;
    public Image CloneImage;
    public Image ProbeImage;
    private void Awake()
    {
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
                    Debug.Log("寻找电力");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Aid":
                    Debug.Log("寻找医疗包");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Dev":
                    Debug.Log("寻找芯片");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Mos":
                    Debug.Log("寻找怪物");
                    panel.SetActive(false);
                    panelBool = false;
                    inCooling = true;
                    countDown = coolingCount;
                    ProbeImage.enabled = false;
                    player.locked = 0;
                    break;
                case "Res":
                    Debug.Log("寻找资源");
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
    void MutiContent(HexCell cell,int count)
    {
        Debug.Log("double");
    }
}
