using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New StageData", menuName = "StageDatas/BattleStage")]
public class SOStageData : ScriptableObject
{
    [Tooltip("���� ���� ��� �ð� [ �� ���� ]")]
    public float startWaitTime;
    [Tooltip("���� ���� �ð� [ �� ���� ]")]
    public float limitTime;
    [Tooltip("�κ� �̵� ��� �ð� [ �� ���� ]")]
    public float waitEndTime;

    //���� �ľ� ����ȭ�� ����, ���� ��� ����
    //ĳ���� ���� ����� �������� ��Ʈ�ѷ� Ŭ������ ���� ����
    //[Tooltip("������ ���� ��")]
    //public int userCount;
    //[Tooltip("������ ���� ��")]
    //public int enemyCount;
    //[Tooltip("������ ���� ������")]
    //public GameObject userObject;
    //[Tooltip("������ ���� ������")]
    //public GameObject enemyObject;
}
