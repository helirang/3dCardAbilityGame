using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New StageData", menuName = "StageDatas/BattleStage")]
public class SOStageData : ScriptableObject
{
    [Tooltip("게임 시작 대기 시간 [ 초 단위 ]")]
    public float startWaitTime;
    [Tooltip("게임 제한 시간 [ 초 단위 ]")]
    public float limitTime;
    [Tooltip("로비 이동 대기 시간 [ 초 단위 ]")]
    public float waitEndTime;

    //구조 파악 간략화를 위해, 관련 기능 제거
    //캐릭터 관련 기능은 스테이지 컨트롤러 클래스에 전부 위임
    //[Tooltip("생성할 유저 수")]
    //public int userCount;
    //[Tooltip("생성할 적군 수")]
    //public int enemyCount;
    //[Tooltip("생성할 유저 프리팹")]
    //public GameObject userObject;
    //[Tooltip("생성할 적군 프리팹")]
    //public GameObject enemyObject;
}
