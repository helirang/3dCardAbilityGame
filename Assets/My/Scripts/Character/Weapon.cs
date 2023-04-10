using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ProjectileManager))]
public class Weapon : MonoBehaviour
{
    [Tooltip("���� ������ SO")]
    [SerializeField] SOGun gundata;
    [SerializeField] ProjectileManager pools;
    [SerializeField] Transform muzzleTransform;

    public EWeaponState WeaponState { get; private set; }
    ETeamNum teamNum;
    IEnumerator fireCorutin;

    [Header("���� ����")]
    int ammoMax, ammoCurrent, dmg;
    float reloadTime;
    public int RoundPerSecond { get; private set; }

    [Header("")]
    AmmoUIController ammoUi;//@todo ammoUi ���� �����

    public event System.Action StopFireEvent;

    public enum EWeaponState
    {
        Ready,
        Fire,
        Relod
    }

    #region ���� �� �ʱ�ȭ �Լ�
    private void Awake()
    {
        ammoMax = gundata.ammoMax;
        ammoCurrent = ammoMax;
        dmg = gundata.dmg;
        RoundPerSecond = gundata.roundPerSecond;
        reloadTime = gundata.reloadTime;
    }

    public void SetTeamNum(ETeamNum ownerTeam)
    {
        teamNum = ownerTeam;
    }

    public void SetAmmoUI(AmmoUIController bulletUI)
    {
        this.ammoUi = bulletUI;
        ammoUi.SetMaxAMMO(ammoMax);
    }
    #endregion

    #region �̺�Ʈ ������ �Լ�
    public void OnStartFire(InputAction.CallbackContext context)
    {
        StartFire();
    }

    public void OnStopFire(InputAction.CallbackContext context)
    {
        StopFire();
    }

    public void OnStartReload(InputAction.CallbackContext context)
    {
        StartReload();
    }
    #endregion

    public void StartFire()
    {
        if (WeaponState == EWeaponState.Ready)
        {
            fireCorutin = Fire();
            WeaponState = EWeaponState.Fire;
            StartCoroutine(fireCorutin);
        }
    }

    public void StopFire()
    {
        if(fireCorutin != null)
            StopCoroutine(fireCorutin);
        if (WeaponState != EWeaponState.Relod)
            WeaponState = EWeaponState.Ready;
        StopFireEvent?.Invoke();
    }

    public void StartReload()
    {
        if (WeaponState != EWeaponState.Relod)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Fire()
    {
        WaitForSeconds seconds = new WaitForSeconds(1f / RoundPerSecond);
        for (int i = ammoCurrent; i >= 0; i--)
        {
            ammoCurrent = i;
            ammoUi?.UpdateCurrentAMMO(ammoCurrent);

            //pool Get
            Projectile projectile = pools.projectilePool.Get();

            //��ġ ����
            projectile.gameObject.transform.position = muzzleTransform.position;
            projectile.transform.rotation = (Quaternion)muzzleTransform.rotation;

            //�߻�
            projectile.Launch(teamNum,dmg,muzzleTransform.forward);

            yield return seconds;
        }
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        ammoUi?.ReloadAMMO(true);

        WeaponState = EWeaponState.Relod;
        StopFire();
        yield return new WaitForSeconds(reloadTime);

        ammoCurrent = ammoMax;
        WeaponState = EWeaponState.Ready;

        ammoUi?.ReloadAMMO(false);
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        if (StopFireEvent != null)
        {
            Debug.Log($"StopFireEvent{StopFireEvent == null}");
            Debug.Log(StopFireEvent?.Method.Name);
            Debug.Log(StopFireEvent?.GetInvocationList().Length);
        }
#endif
    }
}
