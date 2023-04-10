using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Weapon weapon;
    [SerializeField] Animator weaponAnimator;
    Transform target;
    ETeamNum teamNum;
    int spawnID;
    bool isAttackable = false;
    Vector3 targetPosition = new Vector3(0,0,0);

    IEnumerator attackCorutin;
    public delegate void EnemyDeadDel(int spawnID, ETeamNum teamNum);
    public event EnemyDeadDel DeadEvent;

    #region ���� �� �ʱ�ȭ �Լ�
    public void Initialized(Transform target,int spawnID, ETeamNum teamNum ,int hp,Slider hpSlider)
    {
        this.target = target;
        this.teamNum = teamNum;
        this.spawnID = spawnID;

        health.DeadEvent += OnEnemyDeadEvent;
        health.SetTeamNum(this.teamNum);
        health.SetSlider(hpSlider);
        health.SetHP(hp);

        weapon.SetTeamNum(this.teamNum);
        weapon.StopFireEvent += OnStopFire;
        weaponAnimator.SetFloat("fireSpeed", weapon.RoundPerSecond);
    }
    #endregion

    #region �̺�Ʈ ������ �Լ�
    void OnEnemyDeadEvent()
    {
        health.DeadEvent -= OnEnemyDeadEvent;
        health.SetDeadable(false);
        GameEnd();
        DeadEvent?.Invoke(spawnID, teamNum);
    }

    public void OnGameState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.������:
                isAttackable = true;
                break;
            case EGameState.��������:
                GameEnd();
                health.SetDeadable(false);
                break;
        }
    }

    public void OnStopFire()
    {
        StopAttack();
        StartCoroutine(WaitReload());
    }
    #endregion

    private void Update()
    {
        if (target != null) 
        {
            targetPosition.x = target.position.x;
            targetPosition.y = transform.position.y;
            targetPosition.z = target.position.z;
            transform.LookAt(targetPosition);
        }
        if (isAttackable)
        {
            int a = Random.Range(0, 5);
            Attack(a);
            isAttackable = false;
        }
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="attackNum"></param>
    void Attack(int attackNum)
    {
        float atkTime = 1f;
        float intervalTime = 1f;
        switch (attackNum)
        {
            case 0:
                atkTime = 1f;
                intervalTime = 2f;
                break;
            case 1:
                atkTime = 1f;
                intervalTime = 3f;
                break;
            case 2:
                atkTime = 2f;
                intervalTime = 3f;
                break;
            case 3:
                atkTime = 2f;
                intervalTime = 4f;
                break;
            case 4:
                atkTime = 2f;
                intervalTime = 5f;
                break;
        }
        attackCorutin = AttackInterval(atkTime, intervalTime);
        StartCoroutine(attackCorutin);
    }

    void StopAttack()
    {
        if (attackCorutin != null)
            StopCoroutine(attackCorutin);
        isAttackable = false;
        weaponAnimator.SetBool("isFire", false);
    }

    void GameEnd()
    {
        health.DeadEvent -= OnEnemyDeadEvent;
        weapon.StopFire(); //���ε嵵 �ߴ��Ϸ��� : weapon.StopAllCoroutines();
        StopAllCoroutines();
        StopAttack();
    }

    /// <summary>
    /// <para>attackTime ���� ���� ����</para>
    /// <para>������ ������ interverTime ���� ���</para>
    /// ��Ⱑ ������ isAttackable�� true�� �ٲ㼭 ����� ����
    /// </summary>
    /// <param name="attackTime"></param>
    /// <param name="interverTime"></param>
    /// <returns></returns>
    IEnumerator AttackInterval(float attackTime,float interverTime)
    {
        weaponAnimator.SetBool("isFire", true);
        WaitForSeconds attackEndWait = new WaitForSeconds(attackTime);
        WaitForSeconds interverWait = new WaitForSeconds(interverTime);    
        weapon.StartFire();
        yield return attackEndWait;
        weapon.StopFire();
        yield return interverWait;
        isAttackable = true;
    }

    IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(1f);
        isAttackable = true;
    }

    public void EventEmptyCheck()
    {
#if UNITY_EDITOR
        if (DeadEvent != null)
        {
            Debug.Log($"DeadEvent Cleard : {DeadEvent == null}");
            Debug.Log(DeadEvent?.Method.Name);
            Debug.Log(DeadEvent?.GetInvocationList().Length);
        }
        else
        {
            Debug.Log($"DeadEvent Cleared : {DeadEvent == null}");
        }
#endif
    }

    private void OnDestroy()
    {
        health.DeadEvent -= OnEnemyDeadEvent;
        weapon.StopFireEvent -= OnStopFire;

#if UNITY_EDITOR
        EventEmptyCheck();
#endif
    }
}
