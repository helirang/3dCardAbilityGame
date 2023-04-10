using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "Layers/BattleLayer")]
public class SOBattleLayer : ScriptableObject
{
    public LayerMask enemyBodyLayer;
    public LayerMask enemyWeaponLayer;
    public LayerMask playerBodyLayer;
    public LayerMask playerWeaponLayer;
}
