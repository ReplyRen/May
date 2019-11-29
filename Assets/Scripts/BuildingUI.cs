using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    private float timer=0f;
    /*要更换的图片*/
    public Sprite imageChange;

    /*当前图片*/
    public Image imageNow;
    public Image imageOther;
    private int imageSend;
    /*按钮序号*/
    public int buttonNum;
    private int _buttonNum;
    /*图片序列*/
    private int[] picNum = new int[9];
    private int[] buttonLevel = new int[3];
    public GameObject[] Image;
    /*确认窗口*/
    [SerializeField]
    private Transform confirmWindows;
    [SerializeField]
    private Transform Tip;
    private bool buildChange;
    /*获取Player资源*/
    public GameObject player;
    private int flag;
    private int player_chip;
    private int player_Resource;
    private int player_Electric;
    private int chipNum;
    public Text ChipText;
    public Text ResourceText;
    public Text ElectricText;
    public Text TipText;
    private string[] textBuild = new string[9] { "干扰装置（level 1）：10s内不会受到装置作用范围内怪物的伤害\n装置CD：20步\n建造消耗：1芯片+100物资+100电力",
        "干扰装置（level 2）：10s内不会受到装置作用范围内怪物的伤害，作用范围扩大6格\n装置CD：20步\n建造消耗：1芯片+100物资+100电力",
        "干扰装置（level 3）：10s内不会受到装置作用范围内怪物的伤害，作用范围扩大6格\n装置CD：20步\n建造消耗：1芯片+100物资+100电力",
        "克隆装置（level 1）：20s内使选定格子资源增加1倍，若含有怪物则数量同样增加；当前等级可克隆1次\n装置CD：16步\n建造消耗：1芯片+140物资+60电力",
        "克隆装置（level 2）：20s内使选定格子资源增加1倍，若含有怪物则数量同样增加；当前等级可克隆2次（可选择同一格子）\n装置CD：16步\n建造消耗：1芯片+140物资+60电力",
        "克隆装置（level 3）：20s内使选定格子资源增加1倍，若含有怪物则数量同样增加；当前等级可克隆3次（可选择同一格子）\n装置CD：16步\n建造消耗：1芯片+140物资+60电力",
        "探测装置（level 1）：可在物资/电力/医疗包/芯片/怪物中选择任一类型格子进行探测，当前等级持续时间为10s\n装置CD：25步\n建造消耗：1芯片+60物资+140电力",
        "探测装置（level 2）：可在物资/电力/医疗包/芯片/怪物中选择任一类型格子进行探测，当前等级持续时间为20s\n装置CD：25步\n建造消耗：1芯片+60物资+140电力",
        "探测装置（level 3）：可在物资/电力/医疗包/芯片/怪物中选择任一类型格子进行探测，当前等级持续时间为30s\n装置CD：25步\n建造消耗：1芯片+60物资+140电力"};
    private string[] textRemove = new string[3] {"确定拆除该装置？\n（返还1个芯片、 50个物资、 50个电力）",
        "确定拆除该装置？\n（返还1个芯片、 70个物资、 30个电力）",
        "确定拆除该装置？\n（返还1个芯片、 70个物资、 30个电力）"};
    /*消耗的资源*/
    private int[] costPlayer = new int[9] { 1, 100, 100, 1, 140, 60, 1, 60, 140 };
    // Start is called before the first frame update
    void Start()
    {
        //imageNow = this.transform.GetChild(buttonNum).GetComponent<Image>();
        //imageNow = this.transform.GetComponent<Image>();
        Tip.gameObject.SetActive(false);
        foreach (int i in picNum)
        {
            picNum[i] = 0;
        }
        picNum[6] = 1;
        picNum[7] = 1;
        picNum[8] = 1;
        foreach (int i in buttonLevel)
        {
            buttonLevel[i] = 0;
        }

        player = GameObject.FindWithTag("Player");
        downloadData();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        //Debug.Log(buttonNum);
        //imageNow = this.transform.GetChild(buttonNum).GetComponent<Image>();
        /*显示文本的更新*/
        downloadData();
        ChipText.text = player_chip.ToString();
        ResourceText.text = player_Resource.ToString();
        ElectricText.text = player_Electric.ToString();
        chipNum = player.GetComponent<PlayerAsset>().Chip;
        Debug.Log(buttonLevel[0]);
        if (chipNum > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttonLevel[i] == 0)
                {
                    Debug.Log("改图为能");
                    imageSend = 4 + i * 9;
                    Image[i].GetComponent<Image>().sprite = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
                }

            }

        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttonLevel[i]==0)
                {
                    Debug.Log("改图为不能");
                    Image[i].GetComponent<Image>().sprite = Resources.Load("build1", typeof(Sprite)) as Sprite;
                }

            }

        }
        if (Tip.gameObject.active)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
            {
                Tip.gameObject.SetActive(false);
                timer = 0f;
            }
        }
        /*装置图标切换*/
        //if (buttonLevel[0] > 0)
        //{
        //    GameObject.FindWithTag("Interference").GetComponent<Device>().Unlock = false;
        //}
        //else
        //{
        //    GameObject.FindWithTag("Interference").GetComponent<Device>().Unlock = true;
        //}
        //if (buttonLevel[1] > 0)
        //{
        //    GameObject.FindWithTag("Clone").GetComponent<Device>().Unlock = true;
        //}
        //else
        //{
        //    GameObject.FindWithTag("Clone").GetComponent<Device>().Unlock = true;
        //}
        //if (buttonLevel[2] > 0)
        //{
        //    GameObject.FindWithTag("Probe").GetComponent<Device>().Unlock = true;
        //}
        //else
        //{
        //    GameObject.FindWithTag("Probe").GetComponent<Device>().Unlock = true;
        //}
    }

    /// <summary>
    /// 获取需要更换的图片
    /// </summary>
    /// <param name="imageName"></param>
    public void GetImage(int imageName)
    {
        TipText.text = "";
        if (picNum[_buttonNum] == 1 && buttonLevel[_buttonNum % 3] == (2 - _buttonNum / 3))
        {
            flag = (_buttonNum % 3) * 3;
            if (player_chip >= costPlayer[flag] && player_Resource >= costPlayer[flag + 1] && player_Electric >= costPlayer[flag + 2])
            {
                buildChange = true;
                Debug.Log(textBuild[(_buttonNum % 3) * 3 + _buttonNum / 3]);
                confirmWindows.GetComponentInChildren<Text>().text = textBuild[(_buttonNum % 3) * 3 + 2 - _buttonNum / 3];
                confirmWindows.gameObject.SetActive(true);
                imageSend = imageName;
            }
            else
            {
                Tip.gameObject.SetActive(true);
                TipText.text = "资源不足";
            }
        }
        else if (picNum[_buttonNum] == 2)
        {
            Tip.gameObject.SetActive(true);
            TipText.text = "设备工作中";
            if (buttonLevel[_buttonNum % 3] == (3 - _buttonNum / 3))
            {
                buildChange = false;
                confirmWindows.GetComponentInChildren<Text>().text = textRemove[_buttonNum % 3];
                confirmWindows.gameObject.SetActive(true);
                imageSend = imageName;
            }

        }
    }

    /// <summary>
    /// 获取当前按钮的序号
    /// </summary>
    /// <param name="num"></param>
    public void GetNum(int num)
    {
        this.buttonNum = num;
        _buttonNum = num;
        imageNow = this.transform.GetChild(buttonNum).GetComponent<Image>();
    }
    public void confirmWIN(bool resualt)
    {
        if (resualt == true)
        {
            confirmWindows.gameObject.SetActive(false);
            if (buttonLevel[_buttonNum % 3] < 3 && buildChange)
            {
                imageSend = imageSend + picNum[_buttonNum];
                imageChange = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
                imageNow.sprite = imageChange;
                player_chip = player_chip - costPlayer[flag];
                player_Resource = player_Resource - costPlayer[flag + 1];
                player_Electric = player_Electric - costPlayer[flag + 2];
                picNum[_buttonNum]++;
                buttonLevel[_buttonNum % 3]++;

                updateData();

                int imageSend1 = imageSend, imageSend2 = imageSend;
                if (buttonLevel[_buttonNum % 3] < 3)//level高的改图
                {
                    imageOther = this.transform.GetChild(_buttonNum - 3).GetComponent<Image>();
                    imageSend2 = imageSend2 + 2;
                    imageChange = Resources.Load("build" + imageSend2.ToString(), typeof(Sprite)) as Sprite;
                    imageOther.sprite = imageChange;
                    picNum[_buttonNum - 3]++;
                }
                if (buttonLevel[_buttonNum % 3] > 0)//level低的改图
                {
                    imageOther = this.transform.GetChild(_buttonNum + 3).GetComponent<Image>();
                    imageSend1 = imageSend1 - 2;
                    imageChange = Resources.Load("build" + imageSend1.ToString(), typeof(Sprite)) as Sprite;
                    imageOther.sprite = imageChange;
                    picNum[_buttonNum + 3]++;
                }
            }
            if (buttonLevel[_buttonNum % 3] > 0 && !buildChange)
            {
                picNum[_buttonNum] = 1;
                imageSend = imageSend + picNum[_buttonNum] - 1;
                imageChange = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
                imageNow.sprite = imageChange;
                player_chip = player_chip + costPlayer[flag];
                player_Resource = player_Resource + costPlayer[flag + 1];
                player_Electric = player_Electric + costPlayer[flag + 2];
                buttonLevel[_buttonNum % 3]--;

                updateData();

                int imageSend1 = imageSend;
                if (buttonLevel[_buttonNum % 3] > 0)//level低的改图
                {
                    imageOther = this.transform.GetChild(_buttonNum + 3).GetComponent<Image>();
                    imageSend1 = imageSend1 - 2;
                    imageChange = Resources.Load("build" + imageSend1.ToString(), typeof(Sprite)) as Sprite;
                    imageOther.sprite = imageChange;
                    picNum[_buttonNum + 3]--;
                }
                if (buttonLevel[_buttonNum % 3] < 3)//level高的改图
                {
                    imageOther = this.transform.GetChild(_buttonNum - 3).GetComponent<Image>();
                    imageSend = 3 - (_buttonNum % 3);
                    imageChange = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
                    imageOther.sprite = imageChange;
                    picNum[_buttonNum - 3] = 0;
                }
            }

        }
        else
        {
            confirmWindows.gameObject.SetActive(false);
        }
    }
    private void updateData()
    {
        player.GetComponent<PlayerAsset>().Chip = player_chip;
        player.GetComponent<PlayerAsset>().Resource = player_Resource;
        player.GetComponent<PlayerAsset>().Electric = player_Electric;
    }
    private void downloadData()
    {
        player_chip = player.GetComponent<PlayerAsset>().Chip;
        player_Resource = player.GetComponent<PlayerAsset>().Resource;
        player_Electric = player.GetComponent<PlayerAsset>().Electric;
    }

}
