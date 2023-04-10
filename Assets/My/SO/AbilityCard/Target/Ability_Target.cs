using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_Target : ScriptableObject
{
    public string desc;
    public abstract Transform Active(List<Transform> enemyList);
}
