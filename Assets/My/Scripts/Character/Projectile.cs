using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    IObjectPool<Projectile> projectilePool;
    [SerializeField] Collider myCollider;
    ETeamNum attackerTeam = ETeamNum.Enemy;
    [SerializeField]float limitTime = 1f,speed = 20f;
    int dmg = 20;
    Vector3 direction;
    IEnumerator moveCoroutine,timeCoroutine;

    private void Awake()
    {
        if(myCollider == null)
            myCollider = this.GetComponent<Collider>();
    }

    public void SetPool(IObjectPool<Projectile> pool)
    {
        this.projectilePool = pool;
    }

    public void Launch(ETeamNum ownerTeam,
        int damage,Vector3 launchDirection)
    {
        myCollider.enabled = true;
        attackerTeam = ownerTeam;
        this.dmg = damage;
        direction = launchDirection;
        moveCoroutine = ProjectileMove();
        timeCoroutine = Timer(limitTime);
        StartCoroutine(moveCoroutine);
        StartCoroutine(timeCoroutine);
    }

    public void Stop()
    {
        myCollider.enabled = false;
        if(moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        if (moveCoroutine != null)
            StopCoroutine(timeCoroutine);
        projectilePool?.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !damageable.TeamCheck(attackerTeam))
        {
            damageable.Hit(attackerTeam, dmg);
            Stop();
        }
        else if(damageable == null)
        {
            Stop();
        }
    }

    IEnumerator ProjectileMove()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (true)
        {
            transform.position +=
                direction * speed * Time.deltaTime;
            yield return wait;
        }
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Stop();
    }
}
