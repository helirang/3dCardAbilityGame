using System.Collections;
using System.Collections.Generic;

public interface ICardTargetable
{
    /// <summary>
    /// �Ű����� ���� ���� Dmg(Buff)�� �����ϵ��� ������ ��.
    /// </summary>
    /// <param name="value"></param>
    public void DamageControl(int value);

    /// <summary>
    /// �Ű� ���� ���� ���� HP�� �����ϵ��� ������ ��.
    /// </summary>
    /// <param name="value"></param>
    public void HPControl(int value);

    /// <summary>
    /// ����� ���� TeamNum�� ��ȯ�ϵ��� ������ ��.
    /// </summary>
    /// <returns></returns>
    public ETeamNum GetTeamNum();
}
