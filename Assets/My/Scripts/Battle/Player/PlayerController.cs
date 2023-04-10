using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AnimController),typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] PlayerMovement movement;

    [Header("Aniamtion")]
    [SerializeField] AnimController animController;

    [Header("Battle")]
    [SerializeField] Collider weaponCollider;
    [SerializeField] Combat combat;
    [SerializeField] Health health;

    [Header("World UI")]
    [SerializeField] NameController nameController;

    GameInput inputActions;

    [Header("ManageData")]
    int spawnID;
    ETeamNum teamNum;
    EState state;

    enum EState
    {
        Ready,
        Attack
    }

    public delegate void PlayerDeadDel(int spawnID, ETeamNum teamNum);
    public event PlayerDeadDel DeadEvent;

    #region ���� �� �ʱ�ȭ �Լ�
    private void Awake()
    {
        if (animController == null) animController = this.GetComponent<AnimController>();
        if (movement==null) movement = this.GetComponent<PlayerMovement>();
        if (health==null) health = this.GetComponent<Health>();
        if (combat == null) combat = weaponCollider.GetComponent<Combat>();

        weaponCollider.enabled = false;

        teamNum = ETeamNum.User;

        //@todo ���� ���� �ʿ�
        Stats stat = new Stats()
        {
            str = 10
        };
        health.SetTeamNum(teamNum);
        health.SetHP(100);
        combat.Setting(teamNum, stat);

        SetInputSystem();
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    /// <param name="isPlayable">true�� ��� ���� ���� / false�� ���� ���� �Ǹ� ���� ����</param>
    /// <param name="playerTeamNum">�Ҽ� ��</param>
    /// <param name="playerName">�÷��̾� �̸�</param>
    /// <param name="maxHp"></param>
    public void Initialize(int spwanID, ETeamNum playerTeamNum,
        string playerName, int maxHp, bool isPlayable)
    {
        this.spawnID = spwanID;
        this.teamNum = playerTeamNum;

        SetName(playerName);

        health.DeadEvent += OnDeadEvent;
        health.HitEvent += OnHit;
        health.SetTeamNum(teamNum);
        health.SetHP(maxHp);
        health.SetDeadable(true);

        Stats stat = new Stats()
        {
            str = 10
        };
        combat.Setting(teamNum, stat);

        if (isPlayable) inputActions.Player.Enable();
    }

    void SetInputSystem()
    {
        inputActions = new GameInput();
        inputActions.Player.Move.started += OnMoveStart;
        inputActions.Player.Move.canceled += OnMoveEnd;
        inputActions.Player.Attack.started += OnAttack;

        movement.SetInputSystem(inputActions);
        inputActions.Player.Enable();
    }

    void UnBindInputSystem()
    {
        inputActions.Player.Move.started -= OnMoveStart;
        inputActions.Player.Move.canceled -= OnMoveEnd;
        inputActions.Player.Attack.started -= OnAttack;
    }

    void SetName(string name)
    {
        nameController.SetName(name);
    }
    #endregion

    #region �̺�Ʈ ������ �Լ�
    void OnMoveStart(InputAction.CallbackContext context)
    {
        animController.StartMove();
    }

    void OnMoveEnd(InputAction.CallbackContext context)
    {
        animController.StopMove();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if(state == EState.Ready)
        {
            state = EState.Attack; 
            animController.Attack(null, () => weaponCollider.enabled = true, OnAttackFinish);
        }
    }

    void OnAttackFinish()
    {
        weaponCollider.enabled = false;
        state = EState.Ready;
    }

    void OnHit()
    {
        if(state != EState.Attack)
            animController.Hit();
    }

    void OnDeadEvent()
    {
        health.SetDeadable(false);
        DeadEvent?.Invoke(spawnID, teamNum);
        GameEnd();
        animController.Dead();
    }

    public void OnGameState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.������:
                inputActions.Player.Enable();
                break;
            case EGameState.��������:
                GameEnd();
                health.SetDeadable(false);
                break;
        }
    }
    #endregion

    void GameEnd()
    {
        inputActions.Player.Disable();
        health.DeadEvent -= OnDeadEvent;
        animController.StopAnim();
    }

    public void EventNullCheck()
    {
        if(DeadEvent != null)
        {
            string msg = $"DeadEvent : {DeadEvent == null} \n" +
                $"���ε� �� �Լ��� : {DeadEvent?.Method.Name} \n" +
                $"���ε� �� �Լ� ���� : {DeadEvent?.GetInvocationList().Length}";
            CustomDebugger.Debug(LogType.LogWarning,(msg));
            DeadEvent = null;
        }
    }

    private void OnDestroy()
    {
        health.DeadEvent -= OnDeadEvent;
        health.HitEvent -= OnHit;
        UnBindInputSystem();
#if UNITY_EDITOR
        EventNullCheck();
#endif
        DeadEvent = null;
        EventNullCheck();
    }
}
