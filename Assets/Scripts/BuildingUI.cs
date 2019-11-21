using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
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
    private int[] picNum =new int[9];
    private int[] buttonLevel = new int[3];
    /*确认窗口*/
    [SerializeField]
    private Transform confirmWin;
    // Start is called before the first frame update
    void Start()
    {
        //imageNow = this.transform.GetChild(buttonNum).GetComponent<Image>();
        //imageNow = this.transform.GetComponent<Image>();
        foreach(int i in picNum)
        {
            picNum[i] = 0;
        }
        foreach (int i in buttonLevel)
        {
            buttonLevel[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(buttonNum);
        //imageNow = this.transform.GetChild(buttonNum).GetComponent<Image>();
        
    }

    /// <summary>
    /// 获取需要更换的图片
    /// </summary>
    /// <param name="imageName"></param>
    public void GetImage(int imageName)
    {

        if (picNum[_buttonNum] < 2)
        {
            if (picNum[_buttonNum] == 1)
            {
                if (buttonLevel[_buttonNum % 3] >= (2 - _buttonNum / 3))
                {
                    Debug.Log(_buttonNum % 3+"    "+buttonLevel[_buttonNum % 3] + "    " + (2 - _buttonNum / 3));
                    confirmWin.gameObject.SetActive(true);
                    imageSend = imageName;
                }
            }
            else
            {
                imageName = imageName + picNum[_buttonNum];
                Debug.Log("build" + imageName.ToString());
                imageChange = Resources.Load("build" + imageName.ToString(), typeof(Sprite)) as Sprite;
                imageNow.sprite = imageChange;
                picNum[_buttonNum]++;
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
            confirmWin.gameObject.SetActive(false);

            imageSend = imageSend + picNum[_buttonNum];
            imageChange = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
            imageNow.sprite = imageChange;
            if (buttonLevel[_buttonNum % 3]>0)
            {
                imageOther = this.transform.GetChild(_buttonNum + 3).GetComponent<Image>();
                imageSend = imageSend - 2;
                Debug.Log("imageother" + imageSend);
                imageChange = Resources.Load("build" + imageSend.ToString(), typeof(Sprite)) as Sprite;
                imageOther.sprite = imageChange;
            }
            picNum[_buttonNum]++;
            
            buttonLevel[_buttonNum%3]++;
            
        }
        else
        {
            confirmWin.gameObject.SetActive(false);
        }
    }
    
}
