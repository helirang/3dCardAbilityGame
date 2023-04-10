using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour,IDataPipeInjection
{
    EGameState gameState;
    [SerializeField] SOStageDataPipe stageDataPipe;
    [Tooltip("���� ���� ��� �ð� [ �� ���� ]")]
    [SerializeField] float startWaitTime;
    [Tooltip("���� ���� �ð� [ �� ���� ]")]
    [SerializeField] float limitTime;
    [SerializeField] TextMeshProUGUI gameStateText,limitTimeText;

    #region ���� �� �ʱ�ȭ
    private void Awake()
    {
        gameState = EGameState.�����غ�;
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
    /// �������� ���� ��� �ð� �� ���� ���� �ð� ����
    /// </summary>
    /// <param name="sWaitTime"></param>
    /// <param name="lTime"></param>
    public void DataSetting(float sWaitTime , float lTime)
    {
        startWaitTime = sWaitTime;
        limitTime = lTime;
    }
    #endregion

    #region �ֿ� ����
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

        gameState = EGameState.������;
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

        gameState = EGameState.��������;
        gameStateText.text = gameState.ToString();

        stageDataPipe.GameOver();
    }
    #endregion

    private void OnDestroy()
    {
        RemoveDatapipe();
    }
}
