using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnterLoading))]
public class ResultController : MonoBehaviour,IDataPipeInjection
{
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Tooltip("�κ� �̵� ��� �ð� [ �� ���� ]")]
    [SerializeField] float waitTime;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Canvas resultCanvas;
    [SerializeField] EnterLoading enterLoading;

    enum EResult
    {
        ����Ŭ����,
        ���ӿ���
    }

    #region ���� �� �ʱ�ȭ
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
    /// ���� ���� ��, �̵� ��� �ð� ����
    /// </summary>
    /// <param name="delayEndTime"> �̵� ��� �ð� </param>
    public void DataSetting(float delayEndTime)
    {
        waitTime = delayEndTime;
    }
    #endregion

    #region �ֿ� ����
    void OnGameClear()
    {
        Result(EResult.����Ŭ����);
    }

    void OnGameOver()
    {
        Result(EResult.���ӿ���);
    }

    void Result(EResult result)
    {
        switch (result)
        {
            case EResult.����Ŭ����:
                resultText.text = "�¸�";
                resultText.color = Color.yellow;
                break;
            case EResult.���ӿ���:
                resultText.text = "�й�";
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
