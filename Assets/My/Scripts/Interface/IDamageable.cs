using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETeamNum
{
    User,
    Enemy
}

public interface IDamageable
{
    /// <summary>
    /// <para> 공격자와 피격자의 TeamNum이 다르면 데미지 전달</para>
    /// </summary>
    /// <param name="attackerTeam"></param>
    /// <param name="damage"></param>
    public void Hit(ETeamNum attackerTeam, int damage);

    /// <param name="attackerTeam">공격자</param>
    /// <returns>피격자와 공격자의 TeamNum이 같으면 true</returns>
    public bool TeamCheck(ETeamNum attackerTeam);
}
