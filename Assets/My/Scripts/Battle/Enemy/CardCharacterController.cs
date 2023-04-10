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

    [Header("테스트용 노출")]
    List<CardCharacterController> enemyList;
    List<CardCharacterController> teamList;
    int spawnID;

    Vector3 basePosition;
    Quaternion baseRotation;

    int baseDamage = 10;
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

        //hp 셋팅
        health.SetTeamNum(teamNum);
        health.SetHP(hp);

        //weapon 셋팅
        combat.Setting(teamNum,damage);

        //턴 종료시, 할당될 위치와 회전
        basePosition = this.transform.position;
        baseRotation = this.transform.rotation;

        //카드 드롭 슬롯 셋팅 및 활성화
        abilityDropSlot.Setting(teamNum);
        abilityDropSlot.gameObject.SetActive(true);

        //이름 셋팅
        charactername.SetName(characterName);

        isAlive = true;
    }

    #region 이벤트 리스너 함수
    public void OnHit()
    {
        animController.Hit();
    }

    /// <summary>
    /// <para> 캐릭터 사망 처리 및 애니메이션 재생 </para>
    /// <para> HP가 0이되면 호출되는 함수 </para>
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        animController.Dead();
        abilityDropSlot.gameObject.SetActive(false);
        DeadEvent?.Invoke(spawnID,teamNum);
    }

    public void OnGameState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.게임중:
                break;
            case EGameState.게임종료:
                health.SetDeadable(false);
                break;
            case EGameState.턴시작:
                break;
            case EGameState.턴종료:
                this.transform.position = basePosition;
                this.transform.rotation = baseRotation;
                canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
                buffDamage = 0;
                break;
        }
    }

    /// <summary>
    /// <para>살아있는 아군 및 적군 객체 리스트를 전달 받는 함수</para>
    /// </summary>
    /// <param name="teamNum"></param>
    /// <param name="changeList"></param>
    public void OnTargetsChange(ETeamNum teamNum ,List<CardCharacterController> changeList)
    {
        if (this.teamNum == teamNum) teamList = changeList;
        else enemyList = changeList;
    }
    #endregion

    #region 캐릭터 행동 시작 및 종료 함수
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

    void ActionEnd()
    {
        if (ActionEndEvent != null)
            ActionEndEvent.Invoke();
        else
            CustomDebugger.Debug(LogType.LogError, "ActionEndEvent에 바인딩된 함수가 없습니다.");
    }
    #endregion

    /// <summary>
    /// <para>공격 함수</para>
    /// </summary>
    void Attack()
    {
        //데미지 할당
        combat.DamageSet(baseDamage+buffDamage);

        animController.StopMove();
        //공격 애니메이션 실행 및 콜백 함수 전달
        animController.Attack(
            ()=> weaponCollider.enabled = true,
            ()=> 
            {
                weaponCollider.enabled = false;
                StartCoroutine(ActionEndWait());
            });
    }

    /// <summary>
    /// 캐릭터가 살아있는지 파악
    /// </summary>
    /// <returns>살아 있으면 true </returns>
    public bool GetIsAlive()
    {
        return isAlive;
    }

    private void OnDestroy()
    {
        abilityList.Clear();
        ActionEndEvent = null;
        DeadEvent = null;
    }

    /// <summary>
    /// 타겟으로 이동 및 공격하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        bool isAttackEnd = false;

        int targetNum = Random.Range(0, enemyList.Count);

        nav.isStopped = false; // navmeshAgent는 Action에서만 관리한다. 액션 시작에 활성화, 액션 끝 부분에 비활성화
        nav.SetDestination(enemyList[targetNum].transform.position);

        animController.StartMove();

        if (nav.pathPending)
            yield return null;

        while (!isAttackEnd)
        {
            if (nav.pathPending)
                yield return null;

            if (nav.remainingDistance <= nav.stoppingDistance) //일정 간격 이하로 진입하면 공격 시작
            {
                Attack();
                isAttackEnd = true; //While문 해제
            }

            canvasTransform.rotation = Quaternion.Euler(30f, 0f, 0f);
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;

            yield return null;
        }

        animController.StopMove();
        nav.isStopped = true;
    }

    IEnumerator ActionEndWait()
    {
        yield return new WaitForSeconds(1f);
        ActionEnd();
    }

    public void DamageControl(int value)
    {
        buffDamage += value;
    }

    public void HPControl(int value)
    {
        if (value >= 0)
            health.Heal(value);
        else
            health.Hit(value);
    }

    public void AbilitySave(Ability_Origin ability)
    {
        abilityList.Add(ability);
    }
}
