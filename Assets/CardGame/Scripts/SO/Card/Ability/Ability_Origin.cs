using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_Origin : ScriptableObject
{
    [Tooltip("True�� �����Ƽ ��� �ߵ�, False�� ĳ���Ͱ� �����ϴ� ������ �ߵ�")]
    [SerializeField] bool isDirectAbility = false;

    public abstract void Active(ICardTargetable target);

    public bool GetIsDirect()
    {
        return isDirectAbility;
    }
}
