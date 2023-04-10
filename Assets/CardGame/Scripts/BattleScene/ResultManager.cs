using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(EnterLoading))]
public class ResultManager : MonoBehaviour
{
    [Header("셋팅")]
    [SerializeField] SOStageDataPipe dataPipe;
    [SerializeField] Canvas resultCanvas;
    [SerializeField] float waitEndTime = 3f;
    [SerializeField] EnterLoading loader;

    [Header("리워드 아이템 셋팅(변경 예정)")]//@todo Item객체화하기
    [SerializeField] TextMeshProUGUI resultTMP;
    [SerializeField] Image rewardImg;
    [SerializeField] Image rewardItemImg;
    [SerializeField] TextMeshProUGUI rewardItemText;


    [Header("보상")] //@todo SO로 바꾸기
    [SerializeField] int coin;
    [SerializeField] Sprite rewardSprite;


    string[] resultTexts =
    {
        "패배",
        "승리"
    };
    private void Awake()
    {
        dataPipe.GameClearEvent += OnGameClear;
        dataPipe.GameOverEvent += OnGameOver;
        resultCanvas.gameObject.SetActive(false);

        if (loader == null) this.GetComponent<EnterLoading>();
    }
    void OnGameOver()
    {
        rewardImg.gameObject.SetActive(false);
        resultTMP.color = Color.red;
        resultTMP.text = resultTexts[0];

        resultCanvas.gameObject.SetActive(true);
        StartCoroutine(WaitTime());
    }

    void OnGameClear()
    {
        rewardImg.gameObject.SetActive(true);
        resultTMP.color = Color.yellow;
        resultTMP.text = resultTexts[1];
        rewardItemImg.sprite = rewardSprite;
        rewardItemText.text = coin.ToString();

        MoneyStorage.zem += coin;

        resultCanvas.gameObject.SetActive(true);
        StartCoroutine(WaitTime());
    }

    private void OnDestroy()
    {
        dataPipe.GameClearEvent -= OnGameClear;
        dataPipe.GameOverEvent -= OnGameOver;
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitEndTime);
        loader.Enter();
    }
}
