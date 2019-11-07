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
    public GameObject TwoChoice;
    public GameObject OneChoice;
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
        NormalMode.SetActive(false);
        MessageUI.SetActive(true);
    }

    public void PickMessageToLog()
    {
        PickMessage.SetActive(false);
        PickLog.SetActive(true);
    }

    public void PickLogToMessage()
    {
        PickLog.SetActive(false);
        PickMessage.SetActive(true);
    }

    public void MessageToNormal()
    {
        MessageUI.SetActive(false);
        NormalMode.SetActive(true);
    }
    /*方政言操作区间over*/
}
