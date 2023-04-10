using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "New Guns",menuName ="Guns/Basic Gun")]
public class SOGun : ScriptableObject
{
    [Tooltip("탄창")]
    public int ammoMax;
    [Tooltip("공격력")]
    public int dmg;
    [Tooltip("초당 발사량")]
    public int roundPerSecond;
    [Tooltip("재장전 시간 [초 단위]")]
    public float reloadTime;
}
