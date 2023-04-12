using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent),typeof(Rigidbody), typeof(Health))]
[RequireComponent(typeof(AnimController))]
public class CardCharacterController : MonoBehaviour,ICardTargetable
{
    [Header("Movement")]
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] NavMeshAgent nav;

    [Header("WorldUI")]
    [SerializeField] Transform canvasTransform;

    [Header("Battle")]
    [SerializeField] Combat combat;
    [SerializeField] Collider weaponCollider;
    [SerializeField] Health health;
    [SerializeField] ETeamNum teamNum = ETeamNum.Enemy;
    [SerializeField] NameController charactername;

    [Header("Animation")]
    [SerializeField] AnimController animController;

    [Header("Ability")]
    [SerializeField] Ability_DropSlot abilityDropSlot;
    List<Ability_Origin> abilityList = new List<Ability_Origin>();

    [Header("�׽�Ʈ�� ����")]
    List<CardCharacterController> enemyList;
    List<CardCharacterController> teamList;
    int spawnID;

    Vector3 basePosition;
    Quaternion baseRotation;

    //�׽�Ʈ�� SerializeField ���� ���� �����
    [SerializeField] int baseDamage = 10;
    int buffDamage = 0;
    bool isAlive = true;

    public delegate void EnemyDeadDel(int spawnID, ETeamNum teamNum);
    public event EnemyDeadDel DeadEvent;
    public delegate void ActionEndDel();
    public event ActionEndDel ActionEndEvent;

    private void Awake()
    {
        if (nav == null) nav = this.GetComponent<NavMeshAgent>();
        if (health == null) health = this.GetComponent<Health>();
        if (myRigidbody == null) myRigidbody = this.GetComponent<Rigidbody>();
        if (animController == null) animController = this.GetComponent<AnimController>();

        if (combat == null) combat = weaponCollider.GetComponent<Combat>();

        health.DeadEvent += OnDead;
        health.HitEvent += OnHit;

        weaponCollider.enabled = false;

        canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
    }

    public void Setting(int spawnID, ETeamNum teamNum, int hp,string characterName,int damage)
    {
        this.spawnID = spawnID;
        this.teamNum = teamNum;

        //hp ����
        health.SetTeamNum(teamNum);
        health.SetHP(hp);

        //weapon ����
        combat.Setting(teamNum,damage);

        //�� �����, �Ҵ�� ��ġ�� ȸ��
        basePosition = this.transform.position;
        baseRotation = this.transform.rotation;

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
    /// <para> HP�� 0�̵Ǹ� ȣ��Ǵ� �Լ� </para>
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        animController.Dead();
        DeadEvent?.Invoke(spawnID,teamNum);
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
                this.transform.position = basePosition;
                this.transform.rotation = baseRotation;
                canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
                buffDamage = 0; //@todo ���ӵ� �� ������ �����غ���
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
        if(abilityList?.Count > 0)
        {
            foreach (var ability in abilityList)
                ability.Active(this);
        }

        if(enemyList.Count > 0)
        {
            StartCoroutine(Action());
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
    /// <para>���� �Լ�</para>
    /// </summary>
    void Attack()
    {
        //������ �Ҵ�
        combat.DamageSet(baseDamage+buffDamage);

        animController.StopMove();
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
    /// Ÿ������ �̵� �� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        bool isAttackEnd = false;

        int targetNum = Random.Range(0, enemyList.Count);

        // navmeshAgent�� Action������ �����Ѵ�. �׼� ���ۿ� Ȱ��ȭ, �׼� �� �κп� ��Ȱ��ȭ
        nav.isStopped = false; 
        nav.SetDestination(enemyList[targetNum].transform.position);

        animController.StartMove();

        if (nav.pathPending)
            yield return null;

        while (!isAttackEnd)
        {
            if (nav.pathPending)
                yield return null;

            //���� ���� ���Ϸ� �����ϸ� ���� ����
            if (nav.remainingDistance <= nav.stoppingDistance) 
            {
                Attack();
                isAttackEnd = true; //While�� ����
            }

            canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;

            yield return null;
        }

        animController.StopMove();
        nav.isStopped = true;
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

    private void OnDestroy()
    {
        abilityList.Clear();
        ActionEndEvent = null;
        DeadEvent = null;
    }

    #region ICardTargetable �������̽� ����
    public void DamageControl(int value)
    {
        buffDamage += value;
    }

    public void HPControl(int value)
    {
        if (value >= 0)
            health.Heal(value);
        else
            health.Hit(Mathf.Abs(value));
    }

    public void AbilityEquip(Ability_Origin ability)
    {
        abilityList.Add(ability);
    }

    public ETeamNum GetTeamNum()
    {
        return teamNum;
    }
    #endregion
}
