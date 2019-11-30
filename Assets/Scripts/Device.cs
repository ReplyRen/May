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
    public int probeCoolingCount = 20;
    public int cloneCoolingCount = 12;
    public int ITFCoolingCount = 16;
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
    public GameObject ITFCountdownImage;
    public GameObject cloneCountdownImage;
    public GameObject probeCountdownImage;
    public GridContent gridContent;
    private float timer = 0f;
    public int level;
    public int mutiCount;
    public int probeDuration;
    public int cloneDuration = 20;
    public int ITFDuration = 12;
    private void Awake()
    {
        level = 0;
        
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
        if (gameObject.tag == "Probe")
            countDown = probeCoolingCount;
        if (gameObject.tag == "Clone")
            countDown = cloneCoolingCount;
        if (gameObject.tag == "Interference")
            countDown = ITFCoolingCount;
        Unlock = true;
        inCooling = false;
        timer = 0f;
    }
    private void Update()
    {
        ChooseLevel();
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
        if (level > 0)
        {
            Unlock = true;
        }
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
                    Probe("electric");
                    break;
                case "Aid":
                    Probe("firstaid");
                    break;
                case "Dev":
                    Probe("chip");
                    break;
                case "Mos":
                    Probe("monster");
                    break;
                case "Res":
                    Probe("resource");
                    break;

            }

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
            player.CloneUnshow();
            player.locked = 0;
            select.GetComponent<Select>().result = null;
            result = null;
        }
        else if (result == "true")
        {
            if (player.cloneCell != null)
            {
                if (timer == 0)
                    MutiContent(player.cloneCell, mutiCount);
                timer += Time.deltaTime;

                player.locked = 0;
                select.SetActive(false);
                player.CloneUnshow();
                CloneImage.enabled = false;
                cloneCountdownImage.SetActive(true);
                cloneCountdownImage.GetComponentInChildren<Text>().text = ((int)(cloneDuration - timer)).ToString() + "s";
                if (timer > cloneDuration)
                {
                    resetContent(player.cloneCell, 1);
                    inCooling = true;
                    countDown = cloneCoolingCount;
                    cloneCountdownImage.SetActive(false);
                    select.GetComponent<Select>().result = null;
                    selectBool = false;
                    result = null;
                    timer = 0f;
                    player.cloneCell = null;

                    
                }
            }
        }
    }
    private void InterCheck()
    {
        string result = inter.GetComponent<Select2>().result;
        if (result == "false")
        {
            player.locked = 0;
            ResetInterferenceColor();
            inter.SetActive(false);
            interBool = false;
            ITFImage.enabled = false;

            inter.GetComponent<Select2>().result = null;
            result = null;
            
        }
        else if (result == "true")
        {
            if (player.interferenceCells != null)
            {
                if(timer==0f)
                    CleanMonster(player.interferenceCells);
                timer += Time.deltaTime;

                player.locked = 0;
                player.SelectInterferenceColorReset();
                inter.SetActive(false);
                ITFImage.enabled = false;
                ITFCountdownImage.SetActive(true);
                ITFCountdownImage.GetComponentInChildren<Text>().text = ((int)(ITFDuration - timer)).ToString() + "s";
                if (timer > ITFDuration)
                {
                    ResetMonster(player.interferenceCells);
                    inCooling = true;
                    countDown = ITFCoolingCount;
                    ITFCountdownImage.SetActive(false);
                    inter.GetComponent<Select2>().result = null;
                    interBool = false;
                    result = null;
                    timer = 0f;
                    player.interferenceCells = null;
                }
            }
        }
    }
    void MutiContent(HexCell cell,int count)
    {
        gridContent.Double(cell, count);
    }
    void resetContent(HexCell cell, int count)
    {
        gridContent.Half(cell, count);
    }
    void Show(string type)
    {
        gridContent.TypePrint(type);
        player.ShowColor(gridContent.TypePrint(type),type);
    }
    void Unshow(string type)
    { 
        player.UnshowColor(gridContent.TypePrint(type));
        gridContent.TypeLost();

    }
    void Probe(string type)
    {
        timer += Time.deltaTime;
        Show(type);
        player.locked = 0;
        panel.SetActive(false);
        ProbeImage.enabled = false;
        probeCountdownImage.SetActive(true);
        probeCountdownImage.GetComponentInChildren<Text>().text = ((int)(probeDuration - timer)).ToString() + "s";
        if (timer > probeDuration)
        {
            Unshow(type);

            inCooling = true;
            countDown = probeCoolingCount;
            panelBool = false;
            probeCountdownImage.SetActive(false);
            panel.GetComponent<Panel>().result = null;
            timer = 0f;
        }
    }
    void CleanMonster(HexCell[] cells)
    {
        gridContent.avoidmonster(cells);
    }
    void ResetMonster(HexCell[] cells)
    {
        gridContent.monsterback(cells);
    }
    void ResetInterferenceColor()
    {
        player.InterferenceColorReset();
        player.SelectInterferenceColorReset();
        player.interferenceCells = null;
    }
    void ChooseLevel()
    {
        if (level == 0)
        {
            Unlock = false;
        }
        else if (level == 1)
        {
            if (gameObject.tag == "Probe")
            {
                probeDuration = 10;
            }
            else if (gameObject.tag == "Clone")
            {
                mutiCount = 1;
            }
            else if (gameObject.tag == "Interference")
            {
                ITFDuration = 6;
            }
        }
        else if (level == 2)
        {
            if (gameObject.tag == "Probe")
            {
                probeDuration = 20;
            }
            else if (gameObject.tag == "Clone")
            {
                mutiCount = 2;
            }
            else if (gameObject.tag == "Interference")
            {
                ITFDuration = 13;
            }
        }
        else if (level == 3)
        {
            if (gameObject.tag == "Probe")
            {
                probeDuration = 30;
            }
            else if (gameObject.tag == "Clone")
            {
                mutiCount = 3;
            }
            else if (gameObject.tag == "Interference")
            {
                ITFDuration = 20;
            }
        }
    }
}
