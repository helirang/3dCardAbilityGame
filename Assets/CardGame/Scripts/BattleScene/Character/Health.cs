using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour,IDamageable
{
    ETeamNum teamNum = ETeamNum.Enemy;
    [SerializeField] SOBattleLayer battleLayer;

    [Header("체력 관련 필드")]
    [SerializeField] Slider hpSlider;
    [SerializeField] int baseHp=100,runtimeHp=100;
    bool isDeadable = true;

    [Header("데미지 팝업")]
    [SerializeField] DmgTextPool pool;

    public event Action DeadEvent;
    public event Action HitEvent;

    #region 설정 및 초기화 함수
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

    public void SetSlider(Slider slider)
    {
        this.hpSlider = slider;
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
