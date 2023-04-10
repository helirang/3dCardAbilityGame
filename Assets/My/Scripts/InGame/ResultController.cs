using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnterLoading))]
public class ResultController : MonoBehaviour,IDataPipeInjection
{
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Tooltip("로비 이동 대기 시간 [ 초 단위 ]")]
    [SerializeField] float waitTime;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Canvas resultCanvas;
    [SerializeField] EnterLoading enterLoading;

    enum EResult
    {
        게임클리어,
        게임오버
    }

    #region 셋팅 및 초기화
    private void Awake()
    {
        resultCanvas.gameObject.SetActive(false);
    }

    public void SetDatapipe(SOStageDataPipe dataPipe)
    {
        stageDataPipe = dataPipe;
    }

    private void Start()
    {
        stageDataPipe.GameClearEvent += OnGameClear;
        stageDataPipe.GameOverEvent += OnGameOver;
    }

    public void RemoveDatapipe()
    {
        stageDataPipe.GameClearEvent -= OnGameClear;
        stageDataPipe.GameOverEvent -= OnGameOver;
    }

    /// <summary>
    /// 게임 종료 후, 이동 대기 시간 설정
    /// </summary>
    /// <param name="delayEndTime"> 이동 대기 시간 </param>
    public void DataSetting(float delayEndTime)
    {
        waitTime = delayEndTime;
    }
    #endregion

    #region 주요 동작
    void OnGameClear()
    {
        Result(EResult.게임클리어);
    }

    void OnGameOver()
    {
        Result(EResult.게임오버);
    }

    void Result(EResult result)
    {
        switch (result)
        {
            case EResult.게임클리어:
                resultText.text = "승리";
                resultText.color = Color.yellow;
                break;
            case EResult.게임오버:
                resultText.text = "패배";
                resultText.color = Color.red;
                break;
        }
        resultCanvas.gameObject.SetActive(true);
        StartCoroutine(WaitEndTime(this.waitTime));
    }

    IEnumerator WaitEndTime(float waitTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        for (int i = 0; i <= waitTime; i++)
        {
            yield return waitForSeconds;
        }
        enterLoading.Enter();
    }
    #endregion

    private void OnDestroy()
    {
        RemoveDatapipe();
    }
}
