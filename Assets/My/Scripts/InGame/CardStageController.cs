using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStageController : MonoBehaviour,IDataPipeInjection
{
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Header("Users")]
    [SerializeField] GameObject userPrefab;
    [SerializeField] Transform userSpawnpoint;
    [SerializeField] int userHp = 100;
    [SerializeField] Cinemachine.CinemachineVirtualCamera followCam;

    [Header("Enemys")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnpoint;
    [SerializeField] int enemyHp = 500;

    EGameState gameState;

    [Header("���� �� ��ü ������ ������")]
    int spawnID = 0;
    Dictionary<int, PlayerController> playerBySpawnID;
    Dictionary<int, EnemyMovement> enemyBySpawnID;

    public delegate void GameStateChangeDel(EGameState gameState);
    public event GameStateChangeDel StateChangeEvent;

    #region ���� �� �ʱ�ȭ
    private void Awake()
    {
        playerBySpawnID = new Dictionary<int, PlayerController>();
        enemyBySpawnID = new Dictionary<int, EnemyMovement>();
    }
    public void SetDatapipe(SOStageDataPipe dataPipe)
    {
        stageDataPipe = dataPipe;
    }

    private void Start()
    {
        Spawn();
        stageDataPipe.GameStartEvent += OnGameStart;
        stageDataPipe.GameClearEvent += OnGameEnd;
        stageDataPipe.GameOverEvent += OnGameEnd;
    }

    public void RemoveDatapipe()
    {
        stageDataPipe.GameStartEvent -= OnGameStart;
        stageDataPipe.GameClearEvent -= OnGameEnd;
        stageDataPipe.GameOverEvent -= OnGameEnd;
    }
    #endregion

    #region �̺�Ʈ ������ �Լ�
    void OnGameStart()
    {
        gameState = EGameState.������;
        StateChangeEvent?.Invoke(EGameState.������);
    }

    void OnGameEnd()
    {
        Debug.Log("��������");
        gameState = EGameState.��������;
        StateChangeEvent?.Invoke(EGameState.��������);
    }
    #endregion

    void Spawn()
    {
        UserSpawn();
        EnemySpawn();
    }

    void UserSpawn()
    {
        var userObj = Instantiate(userPrefab, userSpawnpoint);
        bool isPlayable = gameState == EGameState.������ ? true : false;

        //ī�޶� ����
        followCam.Follow = userObj.transform;
 
        //�÷��̾� ��Ʈ�ѷ� ����
        PlayerController player = userObj.GetComponent<PlayerController>();
        StateChangeEvent += player.OnGameState;
        player.DeadEvent += DeadManage;
        player.Initialize(spawnID, ETeamNum.User, 
            PlayerPrefs.GetString("NickName"), userHp, isPlayable);

        playerBySpawnID.Add(spawnID, player);
        spawnID++;
    }

    void EnemySpawn()
    {
        var enemyObj = Instantiate(enemyPrefab, enemySpawnpoint);
        bool isPlayable = gameState == EGameState.������ ? true : false;
        Transform target = null;

        foreach (var user in playerBySpawnID)
        {
            target = user.Value.transform;
        }

        Stats stat = new Stats { str = 10 };

        EnemyMovement enemy = enemyObj.GetComponent<EnemyMovement>();

        StateChangeEvent += enemy.OnGameState;
        enemy.DeadEvent += DeadManage;
        enemy.Setting(target, spawnID, ETeamNum.Enemy, enemyHp, stat);

        enemyBySpawnID.Add(spawnID, enemy);
        spawnID++;
    }

    /// <summary>
    /// <para>���� Ŭ������ �ִ� ���� �� �Ǵ� ���� ���� ���� ���� ���� ����</para>
    /// <para>��� ���� ��� == GameOver;</para>
    /// <para>��� ���� ��� == GameClear;</para>
    /// </summary>
    /// <param name="teamNum"></param>
    /// <returns> ���� ���� �� == True </returns>
    public void DeadManage(int spawnID,ETeamNum teamNum)
    {
        switch (teamNum)
        {
            case ETeamNum.User:
                if (playerBySpawnID.ContainsKey(spawnID))
                {
                    var player = playerBySpawnID[spawnID];
                    StateChangeEvent -= player.OnGameState;
                    player.DeadEvent -= DeadManage;
                    playerBySpawnID.Remove(spawnID);
                    //�÷��̾ Destroy�ϰ� ������ ���⼭ �ϱ�
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError($"None Spawn ID : {spawnID}");
#endif
                }
                if (playerBySpawnID.Count <= 0)
                    stageDataPipe.GameOver();
                break;
            case ETeamNum.Enemy:
                if (enemyBySpawnID.ContainsKey(spawnID))
                {
                    var enemy = enemyBySpawnID[spawnID];
                    StateChangeEvent -= enemy.OnGameState;
                    enemy.DeadEvent -= DeadManage;
                    enemyBySpawnID.Remove(spawnID);
                    //enemy�� Destroy�ϰ� ������ ���⼭ �ϱ�
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError($"None Spawn ID : {spawnID}");
#endif
                }
                if (enemyBySpawnID.Count <= 0)
                    stageDataPipe.GameClear();
                break;
        }
    }

    private void OnDestroy()
    {
        //����ִ� �÷��̾��� �̺�Ʈ ���ε� ���� 
        foreach(var player in playerBySpawnID)
        {
            player.Value.DeadEvent -= DeadManage;
            StateChangeEvent -= player.Value.OnGameState;
#if UNITY_EDITOR
            player.Value.EventNullCheck();
#endif
        }

        //����ִ� ������ �̺�Ʈ ���ε� ���� 
        foreach (var enemy in enemyBySpawnID)
        {
            enemy.Value.DeadEvent -= DeadManage;
            StateChangeEvent -= enemy.Value.OnGameState;
        }
        RemoveDatapipe();
        playerBySpawnID.Clear();
        enemyBySpawnID.Clear();

#if UNITY_EDITOR
        if (StateChangeEvent != null)
        {
            Debug.Log($"StateChangeEvent{StateChangeEvent == null}");
            Debug.Log(StateChangeEvent?.Method.Name);
            Debug.Log(StateChangeEvent?.GetInvocationList().Length);
        }
#endif
    }
}
