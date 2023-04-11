using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "Layers/BattleLayer")]
[System.Serializable]
public class SOBattleLayer : ScriptableObject
{
    //@todo Ŀ���� ������ �����ؼ� 1�� �Է¸� �ް� �ϱ�
    public SingleUnityLayer enemyBody;
    public SingleUnityLayer enemyWeapon;
    public SingleUnityLayer playerBody;
    public SingleUnityLayer playerWeapon;
}
