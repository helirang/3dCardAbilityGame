using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "New Guns",menuName ="Guns/Basic Gun")]
public class SOGun : ScriptableObject
{
    [Tooltip("źâ")]
    public int ammoMax;
    [Tooltip("���ݷ�")]
    public int dmg;
    [Tooltip("�ʴ� �߻緮")]
    public int roundPerSecond;
    [Tooltip("������ �ð� [�� ����]")]
    public float reloadTime;
}
