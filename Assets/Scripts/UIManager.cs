using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text resourcesText;
    private Text heartText;
    private Text electricityText;
    private Text firstAidText;
    private GameObject Player;
    private Image resourcesUI;
    private Image elecUI;
    private Image heartUI;
    public Sprite resourceSpriteGreen;
    public Sprite resourceSpriteYellow;
    public Sprite heartSpriteGreen;
    public Sprite heartSpriteYellow;
    public Sprite elecSpriteGreen;
    public Sprite elecSpriteYellow;

    /*方政言操作区间（笑）*/
    public GameObject NormalMode;
    public GameObject MessageUI;
    public GameObject PickMessage;
    public GameObject PickLog;
    public GameObject PickIntro;
    public GameObject TwoChoice;
    public GameObject OneChoice;
    public GameObject hexgrid;
    public Image mask;
    /*方政言操作区间over*/

    private void Start()
    {
        resourcesText = GameObject.FindWithTag("ResourcesText").GetComponent<Text>();
        heartText = GameObject.FindWithTag("HeartText").GetComponent<Text>();
        electricityText = GameObject.FindWithTag("ElectricityText").GetComponent<Text>();
        firstAidText = GameObject.FindWithTag("FirstAidText").GetComponent<Text>();
        Player = GameObject.FindWithTag("Player");
        resourcesUI= GameObject.FindWithTag("ResourcesUI").GetComponent<Image>();
        elecUI = GameObject.FindWithTag("ElecUI").GetComponent<Image>();
        heartUI = GameObject.FindWithTag("HeartUI").GetComponent<Image>();

        /*方政言操作区间（笑）,初始化UI*/
        NormalMode.SetActive(true);
        PickMessage.SetActive(true);
        OneChoice.SetActive(false);
        TwoChoice.SetActive(false);
        PickLog.SetActive(false);
        MessageUI.SetActive(false);
        /*方政言操作区间over*/
    }
    private void Update()
    {
        resourcesText.text = Player.GetComponent<PlayerAsset>().Resource.ToString();
        heartText.text = Player.GetComponent<PlayerAsset>().Hp.ToString();
        electricityText.text = Player.GetComponent<PlayerAsset>().Electric.ToString();
        firstAidText.text = Player.GetComponent<PlayerAsset>().FirstAid.ToString();
        ChangeSprtie(Player.GetComponent<PlayerAsset>().Resource, resourceSpriteGreen, resourceSpriteYellow, resourcesUI);
        ChangeSprtie(Player.GetComponent<PlayerAsset>().Hp, heartSpriteGreen, heartSpriteYellow, heartUI);
        ChangeSprtie(Player.GetComponent<PlayerAsset>().Electric, elecSpriteGreen, elecSpriteYellow, elecUI);
    }
    public void EatFirstAid()
    {
        if (Player.GetComponent<PlayerAsset>().FirstAid > 0)
        {
            Player.GetComponent<PlayerAsset>().FirstAid--;
            Player.GetComponent<PlayerAsset>().useFirstAid();
        }
    }
    public void ChangeSprtie(int i,Sprite green,Sprite yellow,Image image)
    {
        if (i <= 10)
            image.sprite = yellow;
        else
            image.sprite = green;
    }

    /*方政言操作区间（笑）*/
    public void NormalToMessage()
    {
        mask.enabled = false;
        NormalMode.SetActive(false);
        hexgrid.SetActive(false);
        MessageUI.SetActive(true);
        PickMessage.SetActive(true);
        PickLog.SetActive(false);
        PickIntro.SetActive(false);
    }

    public void PickMessageToLog()
    {
        mask.enabled = false;
        PickMessage.SetActive(false);
        PickLog.SetActive(true);
        PickIntro.SetActive(false);

    }

    public void PickLogToMessage()
    {
        mask.enabled = false;
        PickLog.SetActive(false);
        PickMessage.SetActive(true);
        PickIntro.SetActive(false);
    }

    public void MessageToNormal()
    {
        mask.enabled = true;
        MessageUI.SetActive(false);
        hexgrid.SetActive(true);
        NormalMode.SetActive(true);
    }

    public void toIntro()
    {
        PickIntro.SetActive(true);
        PickLog.SetActive(false);
        PickMessage.SetActive(false);
    }

    public void NormaltoPickMessage()
    {
        mask.enabled = false;
        NormalToMessage();
        PickLogToMessage();
    }

    public void NormaltoPickLog()
    {
        mask.enabled = false;
        NormalToMessage();
        PickMessageToLog();
    }

    public void NormaltoPickIntro()
    {
        mask.enabled = false;
        NormalToMessage();
        toIntro();
    }
    /*方政言操作区间over*/
}
