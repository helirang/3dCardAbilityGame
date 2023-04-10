using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "Layers/BattleLayer")]
public class SOBattleLayer : ScriptableObject
{
    //@todo 커스텀 에디터 삽입해서 1개 입력만 받게 하기
    public LayerMask enemyBody;
    public LayerMask enemyWeapon;
    public LayerMask playerBody;
    public LayerMask playerWeapon;
}
