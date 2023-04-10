using System.Collections;
using System.Collections.Generic;

public interface ICardTargetable
{
    public void DamageControl(int value);

    public void HPControl(int value);

    public void AbilitySave(Ability_Origin ability);
}
