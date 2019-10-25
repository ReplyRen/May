using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text resourcesText;
    private Text heartText;
    private Text electricityText;
    private GameObject Player;
    private void Start()
    {
        resourcesText = GameObject.FindWithTag("ResourcesText").GetComponent<Text>();
        heartText = GameObject.FindWithTag("HeartText").GetComponent<Text>();
        electricityText = GameObject.FindWithTag("ElectricityText").GetComponent<Text>();
        Player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        resourcesText.text = Player.GetComponent<PlayerAsset>().Resource.ToString();
        heartText.text = Player.GetComponent<PlayerAsset>().Hp.ToString();
        electricityText.text = Player.GetComponent<PlayerAsset>().Electric.ToString();
    }
}
