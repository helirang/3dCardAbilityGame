using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataSetter : MonoBehaviour
{
    [Tooltip("데이터 파이프")]
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Tooltip("스테이지 시간 데이터")]
    [SerializeField] SOStageData stageData;

    [Tooltip("게임 시간 체크 스크립트")]
    [SerializeField] TimeController timeController;
    [Tooltip("결과창 관리 스크립트")]
    [SerializeField] ResultController resultController;
    [Tooltip("캐릭터(유저/적군) 스폰 및 관리 스크립트")]
    [SerializeField] StageController stageController;
    [Tooltip("데이터 파이프 주입이 가능한 스크립트를 가진 오브젝트들")]
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

        ///유저 데이터 주입 기능. 유저 관련 기능을 StageController로 위임해서 제거.
        //stageController.DataSetting(
        //    stageData.userCount,stageData.enemyCount,
        //    stageData.userObject,stageData.enemyObject);
    }
}
