using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] ETeamNum ownerTeam;
    [SerializeField] SOBattleLayer battleLayer;
    int damage = 0;

    public void Setting(ETeamNum ownerTeam, int damage)
    {
        this.ownerTeam = ownerTeam;
        DamageSet(damage);
        switch (this.ownerTeam)
        {
            case ETeamNum.User:
                this.gameObject.layer = battleLayer.playerWeapon.LayerIndex;
                break;
            case ETeamNum.Enemy:
                this.gameObject.layer = battleLayer.enemyWeapon.LayerIndex;
                break;
        }
    }

    public void DamageSet(int damge)
    {
        this.damage = damge;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.attachedRigidbody?.GetComponent<IDamageable>();
        if(target != null)
        {
            target.Hit(ownerTeam, damage);
        }
    }
}
