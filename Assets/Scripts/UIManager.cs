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
    private void Start()
    {
        resourcesText = GameObject.FindWithTag("ResourcesText").GetComponent<Text>();
        heartText = GameObject.FindWithTag("HeartText").GetComponent<Text>();
        electricityText = GameObject.FindWithTag("ElectricityText").GetComponent<Text>();
        firstAidText = GameObject.FindWithTag("FirstAidText").GetComponent<Text>();
        Player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        resourcesText.text = Player.GetComponent<PlayerAsset>().Resource.ToString();
        heartText.text = Player.GetComponent<PlayerAsset>().Hp.ToString();
        electricityText.text = Player.GetComponent<PlayerAsset>().Electric.ToString();
        firstAidText.text = "医疗包 "+Player.GetComponent<PlayerAsset>().FirstAid.ToString();

    }
    public void EatFirstAid()
    {
        if (Player.GetComponent<PlayerAsset>().FirstAid > 0)
        {
            Player.GetComponent<PlayerAsset>().FirstAid--;
            Player.GetComponent<PlayerAsset>().useFirstAid();
        }
    }
}
