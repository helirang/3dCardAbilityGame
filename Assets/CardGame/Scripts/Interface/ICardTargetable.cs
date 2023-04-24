using System.Collections;
using System.Collections.Generic;

public interface ICardTargetable
{
    /// <summary>
    /// 매개변수 값에 따라 Dmg(Buff)를 조절하도록 구현할 것.
    /// </summary>
    /// <param name="value"></param>
    public void DamageControl(int value);

    /// <summary>
    /// 매개 변수 값에 따라 HP를 조절하도록 구현할 것.
    /// </summary>
    /// <param name="value"></param>
    public void HPControl(int value);

    /// <summary>
    /// 대상이 가진 TeamNum을 반환하도록 구현할 것.
    /// </summary>
    /// <returns></returns>
    public ETeamNum GetTeamNum();
}
