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

    [Header("스폰 된 객체 관리용 데이터")]
    int spawnID = 0;
    Dictionary<int, PlayerController> playerBySpawnID;
    Dictionary<int, EnemyMovement> enemyBySpawnID;

    public delegate void GameStateChangeDel(EGameState gameState);
    public event GameStateChangeDel StateChangeEvent;

    #region 셋팅 및 초기화
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

    #region 이벤트 리스너 함수
    void OnGameStart()
    {
        gameState = EGameState.게임중;
        StateChangeEvent?.Invoke(EGameState.게임중);
    }

    void OnGameEnd()
    {
        Debug.Log("게임종료");
        gameState = EGameState.게임종료;
        StateChangeEvent?.Invoke(EGameState.게임종료);
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
        bool isPlayable = gameState == EGameState.게임중 ? true : false;

        //카메라 셋팅
        followCam.Follow = userObj.transform;
 
        //플레이어 컨트롤러 설정
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
        bool isPlayable = gameState == EGameState.게임중 ? true : false;
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
    /// <para>관리 클래스에 있는 유저 수 또는 적군 수에 따라 게임 상태 변경</para>
    /// <para>모든 유저 사망 == GameOver;</para>
    /// <para>모든 적군 사망 == GameClear;</para>
    /// </summary>
    /// <param name="teamNum"></param>
    /// <returns> 게임 진행 중 == True </returns>
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
                    //플레이어를 Destroy하고 싶으면 여기서 하기
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
                    //enemy를 Destroy하고 싶으면 여기서 하기
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
        //살아있는 플레이어의 이벤트 바인딩 해제 
        foreach(var player in playerBySpawnID)
        {
            player.Value.DeadEvent -= DeadManage;
            StateChangeEvent -= player.Value.OnGameState;
#if UNITY_EDITOR
            player.Value.EventNullCheck();
#endif
        }

        //살아있는 적군의 이벤트 바인딩 해제 
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
