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
    [SerializeField] GameObject userPrefab;
    [SerializeField] Transform userSpawnpoint;
    [SerializeField] int userHp = 100;
    [SerializeField] int userDmg = 10;
    [SerializeField] string userName = "";

    [Header("Enemys")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnpoint;
    [SerializeField] int enemyHp = 500;
    [SerializeField] int enemyDmg = 10;
    [SerializeField] string enemyNmae = "";

    EGameState gameState;
    [Header("���� ���¸� ���� �޴� ������")]
    [SerializeField] CardManager cardManager;

    [Header("���� �� ��ü ������ ������")]
    Dictionary<int, CardCharacterController> playerBySpawnID;
    Dictionary<int, CardCharacterController> enemyBySpawnID;
    int spawnID = 0;

    [Tooltip("ĳ���͵��� �ൿ �������� spawnID�� ������ ť(������)�̴�.")]
    Queue<int> actionQueue = new Queue<int>();

    [Header("���� ��ư")]
    [SerializeField] Button startButton;

    public delegate void GameStateChangeDel(EGameState gameState);
    public event GameStateChangeDel StateChangeEvent;
    public delegate void CharacterListUpdateDel(ETeamNum teamNum,
        List<CardCharacterController> characters);
    public event CharacterListUpdateDel CharacterListUpdateEvent;

    #region ���� �� �ʱ�ȭ
    private void Awake()
    {
        playerBySpawnID = new Dictionary<int, CardCharacterController>();
        enemyBySpawnID = new Dictionary<int, CardCharacterController>();

        startButton.onClick.AddListener(TurnStart);
        StateChangeEvent += cardManager.OnGameState;
    }

    public void SetDatapipe(SOStageDataPipe dataPipe)
    {
        stageDataPipe = dataPipe;
    }

    private void Start()
    {
        stageDataPipe.GameStartEvent += OnGameStart;
        stageDataPipe.GameClearEvent += OnGameEnd;
        stageDataPipe.GameOverEvent += OnGameEnd;

        //@todo ���� ������ SO�� �����
        Spawn(ETeamNum.User, userName, userHp, userDmg);
        Spawn(ETeamNum.Enemy, enemyNmae, enemyHp, enemyDmg);
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
        StateChangeEvent?.Invoke(gameState);
    }

    void OnGameEnd()
    {
        gameState = EGameState.��������;
        StateChangeEvent?.Invoke(gameState);
    }

    /// <summary>
    /// <para>ĳ���Ͱ� ����ϸ� ȣ��Ǵ� �̺�Ʈ</para>
    /// ������ �� ���ַ� �ٲٸ� ��� ����
    /// </summary>
    /// <param name="teamNum"></param>
    /// <returns> ���� ���� �� == True </returns>
    void OnCharacterDead(int spawnID,ETeamNum teamNum)
    {
    }

    /// <summary>
    ///<para>ĳ������ �ൿ�� ������ ȣ��Ǵ� �Լ�</para>
    ///<para>ĳ���Ͱ� ��� �����̸� ���� ��ũ��Ʈ�� ��� ȣ���Ѵ�.</para>
    /// </summary>
    void OnActionEnd()
    {
        if (actionQueue.Count <= 0) TurnEnd();
        else CharacterActive();
    }
    #endregion

    void Spawn(ETeamNum teamNum, string characterName, int hp ,int damage)
    {
        GameObject spawnObj = null;
        CardCharacterController character = null;

        switch (teamNum)
        {
            case ETeamNum.User:
                spawnObj = Instantiate(userPrefab, userSpawnpoint);
                character = spawnObj.GetComponent<CardCharacterController>();
                playerBySpawnID.Add(spawnID, character);
                break;
            case ETeamNum.Enemy:
                spawnObj = Instantiate(enemyPrefab, enemySpawnpoint);
                character = spawnObj.GetComponent<CardCharacterController>();
                enemyBySpawnID.Add(spawnID, character);
                break;
        }

        StateChangeEvent += character.OnGameState;
        CharacterListUpdateEvent += character.OnTargetsChange;
        character.DeadEvent += OnCharacterDead;
        character.ActionEndEvent += OnActionEnd;

        character.Setting(spawnID, teamNum, hp, characterName, damage);
        spawnID++;
    }

    public void TurnStart()
    {
        startButton.interactable = false;
        actionQueue.Clear();

        CharacterListUpdateEvent.Invoke(ETeamNum.Enemy, enemyBySpawnID.Values.ToList());
        CharacterListUpdateEvent.Invoke(ETeamNum.User, playerBySpawnID.Values.ToList());

        //@todo ���⼭ �ӵ��� ���� �켱 ������ �����ϱ�
        int maxCount = enemyBySpawnID.Count + playerBySpawnID.Count;
        List<int> spawnIds = new List<int>();
        spawnIds.AddRange(playerBySpawnID.Keys.ToList());
        spawnIds.AddRange(enemyBySpawnID.Keys.ToList());
        for (int i=0; i < maxCount; i++)
        {
            actionQueue.Enqueue(spawnIds[i]);
        }

        CharacterActive();
    }

    /// <summary>
    /// <para>��� ���� �׾����� ���� Ŭ���� ȣ��</para>
    /// <para>��� ������ �׾����� ���� ���� ȣ��</para>
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
            StateChangeEvent(EGameState.������);
            startButton.interactable = true;
        }
    }

    /// <summary>
    /// actionQueue�� ������ ���� ĳ���͸� Ȱ��ȭ��Ű�� �Լ�
    /// </summary>
    void CharacterActive()
    {
        CardCharacterController character = null;
        int idx = actionQueue.Dequeue();

        if (playerBySpawnID.ContainsKey(idx))
            character = playerBySpawnID[idx];
        else if (enemyBySpawnID.ContainsKey(idx))
            character = enemyBySpawnID[idx];

        if (character != null && character.GetIsAlive()) character.ActionStart();
        else if (character == null) CustomDebugger.Debug(LogType.LogWarning, "None character");
        else if (!character.GetIsAlive()) OnActionEnd();
    }

    /// <summary>
    /// <para> ĳ���� ���� ���� �Ǵ� �� </para>
    /// <para> ��� ĳ���� ���� </para>
    /// </summary>
    /// <param name="characters"></param>
    /// <returns>���޹��� ������ ����ִ� ĳ���Ͱ� ������ True</returns>
    bool AliveCheck(Dictionary<int,CardCharacterController> characters)
    {
        Queue<int> deadCharacters = new Queue<int>();

        ///���� ĳ���͵��� key���� ť�� ���� �ִ´�.
        ///@why = ��ųʸ��� foreach���� �߿��� �� ������ �Ұ����Ͽ� ť�� �����͸� �����Ͽ� ����.
        foreach (var data in characters)
        {
            var character = data.Value;
            if (!character.GetIsAlive())
            {
                deadCharacters.Enqueue(data.Key);
            }
        }

        ///ť�� ����ִ� Ű ������ ĳ���͵��� ���� ó���� ���ش�.
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
    /// ĳ���Ϳ� ������ �̺�Ʈ�� �Ҵ� �����ϴ� �Լ�
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
        //����ִ� �÷��̾��� �̺�Ʈ ���ε� ���� 
        foreach(var character in playerBySpawnID.Values)
        {
            CharacterEventUnbind(character);
        }

        //����ִ� ������ �̺�Ʈ ���ε� ���� 
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
