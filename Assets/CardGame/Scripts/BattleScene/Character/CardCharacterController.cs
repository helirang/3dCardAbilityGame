using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct CardCharacterStats
{
    public int baseDamage;
    public int buffDamage;
    public int baseHp;
}

[RequireComponent(typeof(AnimController), typeof(Health))]
[RequireComponent(typeof(CardCharacterMovement))]
public class CardCharacterController : MonoBehaviour,ICardTargetable
{
    [Header("Movement")]
    [SerializeField] CardCharacterMovement movement;

    [Header("WorldUI")]
    [SerializeField] Transform canvasTransform;
    [SerializeField] NameController charactername;

    [Header("Battle")]
    [SerializeField] Combat combat;
    [SerializeField] Collider weaponCollider;
    [SerializeField] Health health;
    [SerializeField] ETeamNum teamNum = ETeamNum.Enemy;

    [Header("Animation")]
    [SerializeField] AnimController animController;

    [Header("Ability")]
    [SerializeField] Ability_DropSlot abilityDropSlot;

    List<CardCharacterController> enemyList;
    List<CardCharacterController> teamList;
    int spawnID;
    CardCharacterStats stats;
    bool isAlive = true;

    public delegate void EnemyDeadDel(int spawnID, ETeamNum teamNum);
    /// <summary>
    /// ĳ���� ��� ��, ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event EnemyDeadDel DeadEvent;
    public delegate void ActionEndDel();
    /// <summary>
    /// ĳ������ ������ ������ ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public event ActionEndDel ActionEndEvent;

    private void Awake()
    {
        if (health == null) health = this.GetComponent<Health>();
        if (animController == null) animController = this.GetComponent<AnimController>();
        if (movement == null) movement = this.GetComponent<CardCharacterMovement>();
        if (combat == null) combat = weaponCollider.GetComponent<Combat>();

        weaponCollider.enabled = false;
        
        health.DeadEvent += OnDead;
        health.HitEvent += OnHit;

        movement.Setting(canvasTransform);
        movement.ArriveEvent += OnArrive;

        abilityDropSlot.Setting(this);
    }

    public void Setting(int spawnID, ETeamNum teamNum,string characterName, 
        CardCharacterStats stats)
    {
        this.spawnID = spawnID;
        this.teamNum = teamNum;
        this.stats = stats;

        //hp ����
        health.SetTeamNum(teamNum);
        health.SetHP(stats.baseHp);

        //weapon ����
        combat.Setting(teamNum);

        //ī�� ��� ���� Ȱ��ȭ
        abilityDropSlot.gameObject.SetActive(true);

        //�̸� ����
        charactername.SetName(characterName);

        isAlive = true;
    }

    #region �̺�Ʈ ������ �Լ�
    public void OnHit()
    {
        animController.Hit();
    }

    /// <summary>
    /// <para> ĳ���� ��� ó�� �� �ִϸ��̼� ��� </para>
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        animController.Dead();
        DeadEvent?.Invoke(spawnID,teamNum);
    }

    /// <summary>
    /// <para> ĳ���� �̵� �ִϸ��̼� ����</para>
    /// ���� �Լ� ȣ��
    /// </summary>
    void OnArrive()
    {
        animController.StopMove();
        Attack();
    }

    public void OnGameState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.������:
                break;
            case EGameState.��������:
                health.SetDeadable(false);
                break;
            case EGameState.�Ͻ���:
                break;
            case EGameState.������:
                movement.MoveBasePosition();
                stats.buffDamage = 0; //@todo ���ӵ� �� ������ �����غ���
                break;
        }
    }

    /// <summary>
    /// <para>����ִ� �Ʊ� �� ���� ��ü ����Ʈ�� ���� �޴� �Լ�</para>
    /// </summary>
    /// <param name="teamNum"></param>
    /// <param name="changeList"></param>
    public void OnTargetsChange(ETeamNum teamNum ,List<CardCharacterController> changeList)
    {
        if (this.teamNum == teamNum) teamList = changeList;
        else enemyList = changeList;
    }
    #endregion

    #region ĳ���� �ൿ ���� �� ���� �Լ�

    /// <summary>
    /// <para> 1. ĳ���Ϳ� ��ϵ� ��� �����Ƽ �ߵ�</para>
    /// <para> 2.ĳ���� Ȱ�� ����</para>
    /// </summary>
    public void ActionStart()
    {
        //@todo ĳ���� ���� �� ���� ��ų Ȱ��ȭ ��, �ڵ� �ֱ�

        if(enemyList.Count > 0)
        {
            int targetNum = Random.Range(0, enemyList.Count);
            animController.StartMove();
            movement.MoveStart(enemyList[targetNum].transform.position);
        }
    }

    /// <summary>
    /// <para>ActionEndEvent�� Invoke�ϴ� �Լ�</para>
    /// <para>�̺�Ʈ�� ���ε��� �Լ��� ������ ���� �α׸� ����.</para>
    /// </summary>
    void ActionEnd()
    {
        if (ActionEndEvent != null)
            ActionEndEvent.Invoke();
        else
            CustomDebugger.Debug(LogType.LogError, "ActionEndEvent�� ���ε��� �Լ��� �����ϴ�.");
    }
    #endregion

    /// <summary>
    /// <para>���� ������ �Ҵ�</para>
    /// <para>���� �ִϸ��̼� ����</para>
    /// </summary>
    void Attack()
    {
        //������ �Ҵ�
        combat.DamageSet(stats.baseDamage+stats.buffDamage);

        //���� �ִϸ��̼� ���� �� �ݹ� �Լ� ����
        animController.Attack(
            ()=> weaponCollider.enabled = true,
            ()=> 
            {
                weaponCollider.enabled = false;
                StartCoroutine(ActionEndWait());
            });
    }

    /// <summary>
    /// ĳ���Ͱ� ����ִ��� �ľ�
    /// </summary>
    /// <returns>��� ������ true </returns>
    public bool GetIsAlive()
    {
        return isAlive;
    }

    /// <summary>
    /// <para>�ൿ �Ϸ� �� ��� �ð�</para>
    /// <para>ActionEnd�Լ��� ȣ���Ѵ�.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator ActionEndWait()
    {
        yield return new WaitForSeconds(1f);
        ActionEnd();
    }

    void EventUnBind()
    {
        health.DeadEvent -= OnDead;
        health.HitEvent -= OnHit;
        movement.ArriveEvent -= OnArrive;
    }

    private void OnDestroy()
    {
        EventUnBind();
        ActionEndEvent = null;
        DeadEvent = null;
    }

    #region ICardTargetable �������̽� ����
    public void DamageControl(int value)
    {
        stats.buffDamage += value;
    }

    public void HPControl(int value)
    {
        if (value >= 0)
            health.Heal(value);
        else
            health.Hit(Mathf.Abs(value));
    }

    public ETeamNum GetTeamNum()
    {
        return teamNum;
    }

    public Transform GetTargetTransform()
    {
        return this.transform;
    }
    #endregion
}
