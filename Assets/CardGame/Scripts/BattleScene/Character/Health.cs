using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour,IDamageable
{
    ETeamNum teamNum = ETeamNum.Enemy;
    [SerializeField] SOBattleLayer battleLayer;

    [Header("ü�� ���� �ʵ�")]
    [SerializeField] Slider hpSlider;
    int baseHp=100,runtimeHp=100;
    bool isDeadable = true;

    [Header("������ �˾�")]
    [SerializeField] DmgTextPool pool;

    /// <summary>
    /// HP�� 0�� �Ǹ� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event Action DeadEvent;
    /// <summary>
    /// �ǰ� �� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event Action HitEvent;

    private void Awake()
    {
        if(hpSlider == null)
            CustomDebugger.Debug(LogType.LogError, 
                "�Ҵ� �� HPSlider�� �����ϴ�.");
        if (battleLayer == null)
            CustomDebugger.Debug(LogType.LogError, 
                "�Ҵ� �� battleLayer�� �����ϴ�.");
    }

    #region ���� �� �ʱ�ȭ �Լ�
    public void SetHP(int num)
    {
        baseHp = num;
        runtimeHp = baseHp;
        hpSlider.value = runtimeHp;
    }

    public void SetTeamNum(ETeamNum ownerTeam)
    {
        teamNum = ownerTeam;

        switch (ownerTeam)
        {
            case ETeamNum.User:
                this.gameObject.layer = battleLayer.playerBody.LayerIndex;
                break;
            case ETeamNum.Enemy:
                this.gameObject.layer = battleLayer.enemyBody.LayerIndex;
                break;
        }
    }

    public void SetDeadable(bool isDeadable)
    {
        this.isDeadable = isDeadable;
    }
    #endregion

    public int GetHP()
    {
        return runtimeHp;
    }

    public bool TeamCheck(ETeamNum attackerTeam)
    {
        return this.teamNum == attackerTeam;
    }

    public void Hit(ETeamNum attackerTeam, int damage)
    {
        if(this.teamNum != attackerTeam)
        {
            Hit(damage);
        }      
    }

    public void Hit(int damage)
    {
        if (damage < 0) damage = 0;

        DamagePopUp(damage);

        runtimeHp = runtimeHp - damage <= 0 ?
            runtimeHp = 0 : runtimeHp - damage;

        hpSlider.value = (float)runtimeHp / baseHp;

        if (runtimeHp <= 0) Died();
        else HitEvent?.Invoke();
    }

    public void Heal(int value)
    {
        runtimeHp = runtimeHp + value <= baseHp ?
            runtimeHp = runtimeHp + value : baseHp;
        hpSlider.value = (float)runtimeHp / baseHp;
    }

    public void DamagePopUp(int damage)
    {
        DamageText dmgText = pool.dmgPool.Get();
        dmgText.SetDamage(damage);
    }

    void Died()
    {
        if (isDeadable)
        {
            DeadEvent?.Invoke();
        }
    }

    private void OnDestroy()
    {
        DeadEvent = null;
        HitEvent = null;
    }
}
