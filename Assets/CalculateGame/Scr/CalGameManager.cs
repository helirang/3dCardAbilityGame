using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CalGameManager : MonoBehaviour
{
    Calculator calculator;
    SubCalculator subCalculator;

    private int grade = 0;
    private int points = 0;
    [Header("Calculate")]
    public int calLevel;
    [SerializeField]
    GameObject calObj;
    [SerializeField]
    List<GameObject> texts = new List<GameObject>();
    [SerializeField]
    List<GameObject> signs = new List<GameObject>();
    List<bool> signCheckArr;

    [Header("Timer")]
    [SerializeField]
    Timer timer;

    bool isPlay = true;
    [Header("UI")]
    [SerializeField]
    private GameObject myRank;
    [SerializeField]
    private GameObject[] topRanks;
    [SerializeField]
    private TextMeshProUGUI pointText;
    [SerializeField]
    private GameObject endUI;
    [SerializeField]
    private GameObject successUI;
    Vector3 calBasePosition;
    [HideInInspector]
    public int CalLevel
    {
        get { return calLevel; }
        set
        {
            calLevel = value;
            calculator.numCount = calLevel + 1;
        }
    }
    [HideInInspector]
    public int Points
    {
        get { return points; }
        set
        {
            points = value;
            pointText.text = $"{string.Format("{0:#,###}", points)}";
        }
    }


    void Start()
    {
        calculator = this.GetComponent<Calculator>();
        subCalculator = this.GetComponent<SubCalculator>();
        CalLevel = calLevel;
        signCheckArr = new List<bool>(new bool[calLevel]);
        signCheckArr[0] = false;
        CalGameStart();
        calBasePosition = calObj.GetComponent<RectTransform>().position;
    }

    private void Initialized()
    {
        for (int i = 2; i < texts.Count; i++)
        {
            Destroy(texts[i]);
        }
        for (int i = 1; i < signs.Count; i++)
        {
            Destroy(signs[i]);
        }
        if (texts.Count > 2) texts.RemoveRange(2, texts.Count-2);
        if (signs.Count > 1) signs.RemoveRange(1, signs.Count-1);
        //Debug.Log(texts.Count);
        CalLevel = 1;
        grade = 0;
        signCheckArr.Clear();
        signCheckArr.Add(false);
        isPlay = true;
        Points = 0;
        pointText.text = $"0";
        calObj.GetComponent<RectTransform>().position = calBasePosition;
    }

    private void InitializedSign()
    {
        //Debug.Log(signs.Count);
        for (int i = 0; i < signs.Count; i++)
        {
            //Debug.Log(i);
            TextMeshProUGUI tmp = signs[i].GetComponentInChildren<TextMeshProUGUI>();
            DropSlot dropSlot = signs[i].GetComponent<DropSlot>();
            tmp.text = "";
            dropSlot.slotSign = -1;
            signCheckArr[i] = false;
        }
    }

    public void CalGameStart()
    {
        InitializedSign();
        calculator.CalculationStart();
        SetNumText();
    }

    private void NextStep()
    {
        grade++;
        Points += calLevel * 100;
        bool isLastLv = false;
        if (calLevel >= 4) isLastLv = true;

        if (isLastLv && grade >= 3) 
        {
            GameClear();
            return;
        }

        //@todo [완료] 요기에 정답입니다. UI 활성화 코드 넣기
        successUI.SetActive(true);
        //@todo [완료] 다음거 진행 함수 주석 처리
        //CalGameStart();
    }

    public void NextCalGame()
    {
        if(grade >= 5) CalLevelUp();
        CalGameStart();
    }

    private void CalLevelUp()
    {
        CalLevel += 1;
        grade = 0;
        AddSign();
        AddNum();
        Debug.Log("CalLevelUp");
    }

    private void AddSign()
    {
        GameObject gameObject = Instantiate(signs[0], calObj.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        //Vector2 basePosition = texts[texts.Count - 1].GetComponent<RectTransform>().position;
        //rectTransform.position = new Vector2(basePosition.x + 145, basePosition.y);
        signs.Add(gameObject);
        signCheckArr.Add(false);

        DropSlot dropSlot = gameObject.GetComponent<DropSlot>();
        dropSlot.slotNum = signs.Count - 1;
    }

    private void AddNum()
    {
        GameObject gameObject = Instantiate(texts[0], calObj.transform);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        //Vector2 basePosition = signs[signs.Count - 1].GetComponent<RectTransform>().position;
        texts.Add(gameObject);
        RectTransform calRecTransform = calObj.GetComponent<RectTransform>();
        Vector2 basePosition = calRecTransform.position;
        calRecTransform.position = new Vector2(basePosition.x - 70, basePosition.y);
    }

    public void GameClear()
    {
        Debug.Log("GAME CLEAR");
        isPlay = false;
        TextMeshProUGUI[] myDataTmps = myRank.GetComponentsInChildren<TextMeshProUGUI>();
        Points += (int)(timer.LookTime * 10000);
        Debug.Log(myDataTmps.Length);
        foreach (TextMeshProUGUI tmp in myDataTmps)
        {
            switch (tmp.name)
            {
                case "Num(TMP)":
                    break;
                case "Name(TMP)":
                    tmp.text = "Guest";
                    break;
                case "Point(TMP)":
                    tmp.text = $"{string.Format("{0:#,###}", points)}";
                    break;
                case "Date(TMP)":
                    tmp.text = System.DateTime.Now.ToString("yyyy.MM.dd");
                    break;
            }
        }
        endUI.SetActive(true);
        this.GetComponent<Timer>().IsPause = true;
    }
    public void CalGameReset()
    {
        Initialized();
        CalGameStart();
    }
    private void SetNumText()
    {
        List<int> nums = calculator.BackUpnumArr;
        for (int i = 0; i < texts.Count; i++)
        {
            TextMeshProUGUI tmp = texts[i].GetComponent<TextMeshProUGUI>();
            tmp.alignment = TMPro.TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
            if (i >= texts.Count - 1)
            {
                tmp.text = $"{nums[i]} = {calculator.Answer}";
                tmp.alignment = TMPro.TextAlignmentOptions.Left;
            }
            else tmp.text = nums[i].ToString();
        }
    }

    public void VerifyStart(int slotNum, int signNum)
    {
        signCheckArr[slotNum] = signNum == calculator.signArr[slotNum] ?
                true : false;
        bool isNext = AllCheck();
        if(!isNext) SubCal();
    }
    private bool AllCheck()
    {
        bool isClear = true;
        for (int i = 0; i < signCheckArr.Count; i++)
        {
            if (!signCheckArr[i])
            {
                isClear = false;
                break;
            }
        }
        if (isClear) NextStep();

        return isClear;
    }

    private void SubCal()
    {
        bool isReady = true;
        List<int> sendSignArr = new List<int>();
        for (int i = 0; i < signs.Count; i++)
        {
            int slotSign = signs[i].GetComponent<DropSlot>().slotSign;
            if (slotSign == -1)
            {
                isReady = false;
                break;
            }
            else sendSignArr.Add(slotSign);
        }
        if (isReady)
        {
            Debug.Log("Ready : Start SubCalculate");
            int resultNum = subCalculator.CalculationStart(calculator.BackUpnumArr, sendSignArr);
            Debug.Log("SubCalculate Result : " + resultNum);
            if (resultNum == calculator.Answer) NextStep();
        }
    }
}
