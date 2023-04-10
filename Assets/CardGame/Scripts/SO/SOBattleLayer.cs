using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "Layers/BattleLayer")]
public class SOBattleLayer : ScriptableObject
{
    //@todo Ŀ���� ������ �����ؼ� 1�� �Է¸� �ް� �ϱ�
    public LayerMask enemyBody;
    public LayerMask enemyWeapon;
    public LayerMask playerBody;
    public LayerMask playerWeapon;
}
