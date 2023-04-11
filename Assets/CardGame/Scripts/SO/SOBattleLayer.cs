using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "Layers/BattleLayer")]
[System.Serializable]
public class SOBattleLayer : ScriptableObject
{
    //@todo 커스텀 에디터 삽입해서 1개 입력만 받게 하기
    public SingleUnityLayer enemyBody;
    public SingleUnityLayer enemyWeapon;
    public SingleUnityLayer playerBody;
    public SingleUnityLayer playerWeapon;
}
