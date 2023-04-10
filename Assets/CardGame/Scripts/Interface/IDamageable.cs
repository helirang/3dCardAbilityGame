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
    /// <para> �����ڿ� �ǰ����� TeamNum�� �ٸ��� ������ ����</para>
    /// </summary>
    /// <param name="attackerTeam"></param>
    /// <param name="damage"></param>
    public void Hit(ETeamNum attackerTeam, int damage);

    /// <param name="attackerTeam">������</param>
    /// <returns>�ǰ��ڿ� �������� TeamNum�� ������ true</returns>
    public bool TeamCheck(ETeamNum attackerTeam);
}
