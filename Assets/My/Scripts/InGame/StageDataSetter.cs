using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataSetter : MonoBehaviour
{
    [Tooltip("������ ������")]
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Tooltip("�������� �ð� ������")]
    [SerializeField] SOStageData stageData;

    [Tooltip("���� �ð� üũ ��ũ��Ʈ")]
    [SerializeField] TimeController timeController;
    [Tooltip("���â ���� ��ũ��Ʈ")]
    [SerializeField] ResultController resultController;
    [Tooltip("ĳ����(����/����) ���� �� ���� ��ũ��Ʈ")]
    [SerializeField] StageController stageController;
    [Tooltip("������ ������ ������ ������ ��ũ��Ʈ�� ���� ������Ʈ��")]
    [SerializeField] List<GameObject> pipeInjectionAbles;

    private void Awake()
    {
        foreach(var objs in pipeInjectionAbles)
        {
            var injection = objs.GetComponents<IDataPipeInjection>();
            foreach(var target in injection)
            {
                target.SetDatapipe(stageDataPipe);
            }
        }

        timeController.DataSetting(stageData.startWaitTime, stageData.limitTime);
        resultController.DataSetting(stageData.waitEndTime);

        ///���� ������ ���� ���. ���� ���� ����� StageController�� �����ؼ� ����.
        //stageController.DataSetting(
        //    stageData.userCount,stageData.enemyCount,
        //    stageData.userObject,stageData.enemyObject);
    }
}
