using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    게임준비,
    게임중,
    게임종료,
    턴시작,
    턴종료
}

/// <summary>
/// <para>게임 상태를 전파하는 데이터 파이프</para>
/// <para>Editor 상태에서 하이라키에 배치되는 Class들에만 사용할 것.</para>
/// </summary>
[CreateAssetMenu(fileName = "New DataPipe", menuName = "DataPipe/BattleStagePipe")]
public class SOStageDataPipe : ScriptableObject
{
    public event System.Action GameStartEvent;
    public event System.Action GameOverEvent;
    public event System.Action GameClearEvent;

    public void GameStart()
    {
        GameStartEvent?.Invoke();
    }

    public void GameOver()
    {
        GameOverEvent?.Invoke();
    }

    public void GameClear()
    {
        GameClearEvent?.Invoke();
    }

    private void OnDestroy()
    {
        GameStartEvent = null;
        GameOverEvent = null;
        GameClearEvent = null;
    }
}
