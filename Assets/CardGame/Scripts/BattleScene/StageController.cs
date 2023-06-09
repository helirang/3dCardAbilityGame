using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour,IDataPipeInjection
{
    [SerializeField] SOStageDataPipe stageDataPipe;

    [Header("Users")]
    [SerializeField] SOCardCharacter userCardCharacter;
    [SerializeField] Transform userSpawnpoint;

    [Header("Enemys")]
    [SerializeField] SOCardCharacter enemyCardCharacter;
    [SerializeField] Transform enemySpawnpoint;

    EGameState gameState;
    [Header("게임 상태를 수신 받는 관리자")]
    [SerializeField] CardManager cardManager;

    [Header("스폰 된 객체 관리용 데이터")]
    Dictionary<int, CardCharacterController> playerBySpawnID;
    Dictionary<int, CardCharacterController> enemyBySpawnID;
    int spawnID = 0;

    [Tooltip("캐릭터들의 행동 순번으로 spawnID를 저장한 큐(데이터)이다.")]
    Queue<int> actionQueue = new Queue<int>();

    [Header("시작 버튼")]
    [SerializeField] Button startButton;

    public delegate void GameStateChangeDel(EGameState gameState);
    /// <summary>
    /// <para>게임의 상태가 바뀌면 호출되는 이벤트</para>
    /// </summary>
    public event GameStateChangeDel StateChangeEvent;
    public delegate void CharacterListUpdateDel(ETeamNum teamNum,
        List<CardCharacterController> characters);
    /// <summary>
    /// <para>각 팀별 캐릭터 List를 전파하는 이벤트 </para>
    /// </summary>
    public event CharacterListUpdateDel CharacterListUpdateEvent;

    #region 셋팅 및 초기화
    private void Awake()
    {
        playerBySpawnID = new Dictionary<int, CardCharacterController>();
        enemyBySpawnID = new Dictionary<int, CardCharacterController>();

        startButton.onClick.AddListener(TurnStart);
        StateChangeEvent += cardManager.OnGameState;
    }

    private void Start()
    {
        //@todo 몬스터 데이터 SO로 만들기
        Spawn(ETeamNum.User);
        Spawn(ETeamNum.Enemy);
    }

    public void SetDatapipe(SOStageDataPipe dataPipe)
    {
        stageDataPipe = dataPipe;

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
        StateChangeEvent?.Invoke(gameState);
    }

    void OnGameEnd()
    {
        gameState = EGameState.게임종료;
        StateChangeEvent?.Invoke(gameState);
    }

    /// <summary>
    /// <para>캐릭터가 사망하면 호출되는 이벤트</para>
    /// 게임을 턴 위주로 바꾸며 기능 제거
    /// </summary>
    /// <param name="teamNum"></param>
    /// <returns> 게임 진행 중 == True </returns>
    void OnCharacterDead(int spawnID,ETeamNum teamNum)
    {
        if (!AliveCheck(enemyBySpawnID))
        {
            stageDataPipe.GameClear();
        }
        else if (!AliveCheck(playerBySpawnID))
        {
            stageDataPipe.GameOver();
        }
    }

    /// <summary>
    ///<para>캐릭터의 행동이 끝나면 호출되는 함수</para>
    ///<para>캐릭터가 사망 상태이면 현재 스크립트가 즉시 호출한다.</para>
    /// </summary>
    void OnActionEnd()
    {
        if (actionQueue.Count <= 0) TurnEnd();
        else CharacterActive();
    }
    #endregion

    void Spawn(ETeamNum teamNum)
    {
        GameObject spawnObj = null;
        CardCharacterController characterController = null;
        SOCardCharacter cardCharacter = null;

        switch (teamNum)
        {
            case ETeamNum.User:
                cardCharacter = userCardCharacter;
                spawnObj = Instantiate(cardCharacter.character, userSpawnpoint);
                characterController = spawnObj.GetComponent<CardCharacterController>();
                playerBySpawnID.Add(spawnID, characterController);
                break;
            case ETeamNum.Enemy:
                cardCharacter = enemyCardCharacter;
                spawnObj = Instantiate(cardCharacter.character, enemySpawnpoint);
                characterController = spawnObj.GetComponent<CardCharacterController>();
                enemyBySpawnID.Add(spawnID, characterController);
                break;
        }

        StateChangeEvent += characterController.OnGameState;
        CharacterListUpdateEvent += characterController.OnTargetsChange;
        characterController.DeadEvent += OnCharacterDead;
        characterController.ActionEndEvent += OnActionEnd;

        characterController.Setting(spawnID,teamNum,cardCharacter.name, cardCharacter.stats);
        spawnID++;
    }

    public void TurnStart()
    {
        startButton.interactable = false;

        StateChangeEvent.Invoke(EGameState.턴시작);
        actionQueue.Clear();

        ///@todo 사망 체크 등을 진행하면 불필요한 인보크를 없앨 수 있다. 
        CharacterListUpdateEvent.Invoke(ETeamNum.Enemy, enemyBySpawnID.Values.ToList());
        CharacterListUpdateEvent.Invoke(ETeamNum.User, playerBySpawnID.Values.ToList());

        //@todo 여기서 속도에 따른 우선 순위로 변경하기
        int maxCount = enemyBySpawnID.Count + playerBySpawnID.Count;
        List<int> spawnIds = new List<int>();
        spawnIds.AddRange(playerBySpawnID.Keys.ToList());
        spawnIds.AddRange(enemyBySpawnID.Keys.ToList());

        //행동 순서에 따라 spawnId를 액션큐에 넣기
        for (int i=0; i < maxCount; i++)
        {
            actionQueue.Enqueue(spawnIds[i]);
        }

        CharacterActive();
    }

    /// <summary>
    /// <para>모든 적이 죽었으면 게임 클리어 호출</para>
    /// <para>모든 유저가 죽었으면 게임 오버 호출</para>
    /// </summary>
    void TurnEnd()
    {
        if (!AliveCheck(enemyBySpawnID))
        {
            stageDataPipe.GameClear();
        }
        else if (!AliveCheck(playerBySpawnID))
        {
            stageDataPipe.GameOver();
        }
        else
        {
            StateChangeEvent(EGameState.턴종료);
            startButton.interactable = true;
        }
    }

    /// <summary>
    /// actionQueue의 순번에 따라 캐릭터를 활성화시키는 함수
    /// </summary>
    void CharacterActive()
    {
        int idx = actionQueue.Dequeue();
        CardCharacterController character = null;

        if (playerBySpawnID.ContainsKey(idx))
            character = playerBySpawnID[idx];
        else if (enemyBySpawnID.ContainsKey(idx))
            character = enemyBySpawnID[idx];

        if (character != null && character.GetIsAlive()) character.ActionStart();
        else if (character == null) CustomDebugger.Debug(LogType.LogWarning, "None character");
        else if (!character.GetIsAlive()) OnActionEnd();
    }

    /// <summary>
    /// <para> 캐릭터 생존 여부 판단 및 </para>
    /// <para> 사망 캐릭터 삭제 </para>
    /// </summary>
    /// <param name="characters"></param>
    /// <returns>전달받은 팀에서 살아있는 캐릭터가 있으면 True</returns>
    bool AliveCheck(Dictionary<int,CardCharacterController> characters)
    {
        Queue<int> deadCharacters = new Queue<int>();

        ///죽은 캐릭터들의 key값을 큐에 집어 넣는다.
        ///@why = 딕셔너리의 foreach과정 중에는 값 변경이 불가능하여 큐로 데이터를 추출하여 변경.
        foreach (var data in characters)
        {
            var character = data.Value;
            if (!character.GetIsAlive())
            {
                deadCharacters.Enqueue(data.Key);
            }
        }

        ///큐에 들어있는 키 값으로 캐릭터들의 제거 처리를 해준다.
        foreach(var id in deadCharacters)
        {
            CardCharacterController character = characters[id];
            CharacterEventUnbind(character);
            characters.Remove(id);
            Destroy(character.gameObject);
        }

        return characters.Count > 0;
    }

    /// <summary>
    /// 캐릭터와 연관된 이벤트를 할당 해제하는 함수
    /// </summary>
    /// <param name="character"></param>
    void CharacterEventUnbind(CardCharacterController character)
    {
        CharacterListUpdateEvent -= character.OnTargetsChange;
        StateChangeEvent -= character.OnGameState;
        character.DeadEvent -= OnCharacterDead;
        character.ActionEndEvent -= OnActionEnd;
    }

    private void OnDestroy()
    {
        //살아있는 플레이어의 이벤트 바인딩 해제 
        foreach(var character in playerBySpawnID.Values)
        {
            CharacterEventUnbind(character);
        }

        //살아있는 적군의 이벤트 바인딩 해제 
        foreach (var character in enemyBySpawnID.Values)
        {
            CharacterEventUnbind(character);
        }

        RemoveDatapipe();

        playerBySpawnID.Clear();
        enemyBySpawnID.Clear();

        startButton.onClick.RemoveListener(TurnEnd);
        StateChangeEvent -= cardManager.OnGameState;

        StateChangeEvent = null;
        CharacterListUpdateEvent = null;
    }
}
