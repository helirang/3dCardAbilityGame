using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemAbilties", menuName = "ItemAbilties/Buff")]
public class SOBuff_ItemAbilty : SOItemAbilty
{
    [SerializeField] Stats stat;
    [SerializeField] bool isRuntimeBuff;
    public override void Active()
    {
        PlayerDatas.Add(stat, isRuntimeBuff);
    }
}
