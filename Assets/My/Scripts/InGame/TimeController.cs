using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour,IDataPipeInjection
{
    EGameState gameState;
    [SerializeField] SOStageDataPipe stageDataPipe;
    [Tooltip("게임 시작 대기 시간 [ 초 단위 ]")]
    [SerializeField] float startWaitTime;
    [Tooltip("게임 제한 시간 [ 초 단위 ]")]
    [SerializeField] float limitTime;
    [SerializeField] TextMeshProUGUI gameStateText,limitTimeText;

    #region 셋팅 및 초기화
    private void Awake()
    {
        gameState = EGameState.게임준비;
        gameStateText.text = gameState.ToString();
    }

    private void Start()
    {
        StartCoroutine(WaitGameStart(startWaitTime));
        stageDataPipe.GameClearEvent += Stop;
        stageDataPipe.GameOverEvent += Stop;
    }

    public void SetDatapipe(SOStageDataPipe dataPipe)
    {
        stageDataPipe = dataPipe;
    }

    public void RemoveDatapipe()
    {
        stageDataPipe.GameClearEvent -= Stop;
        stageDataPipe.GameOverEvent -= Stop;
    }

    /// <summary>
    /// 스테이지 시작 대기 시간 및 게임 제한 시간 설정
    /// </summary>
    /// <param name="sWaitTime"></param>
    /// <param name="lTime"></param>
    public void DataSetting(float sWaitTime , float lTime)
    {
        startWaitTime = sWaitTime;
        limitTime = lTime;
    }
    #endregion

    #region 주요 동작
    void Stop()
    {
        this.StopAllCoroutines();
    }

    IEnumerator WaitGameStart(float waitTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        for(int i=0; i<= waitTime; i++)
        {
            limitTimeText.text = (startWaitTime-i).ToString();
            yield return waitForSeconds;
        }

        gameState = EGameState.게임중;
        gameStateText.text = gameState.ToString();
        limitTimeText.text = limitTime.ToString();

        stageDataPipe.GameStart();

        StartCoroutine(WaitLimitTime(limitTime));
    }
    IEnumerator WaitLimitTime(float waitTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        for (int i = 0; i <= waitTime; i++)
        {
            limitTimeText.text = (limitTime - i).ToString();
            yield return waitForSeconds;
        }

        gameState = EGameState.게임종료;
        gameStateText.text = gameState.ToString();

        stageDataPipe.GameOver();
    }
    #endregion

    private void OnDestroy()
    {
        RemoveDatapipe();
    }
}
