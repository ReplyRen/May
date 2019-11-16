using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Device : MonoBehaviour
{
    public Sprite Unlocked;
    public Sprite Cooling;
    public bool Unlock=false;
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
    private void Awake()
    {
        if(gameObject.tag=="Probe")
             panel.SetActive(false);
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
                panel.SetActive(true);

            inCooling = true;
            countDown = coolingCount;
        }
    }

}
