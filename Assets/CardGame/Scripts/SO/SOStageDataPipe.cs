using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    �����غ�,
    ������,
    ��������,
    �Ͻ���,
    ������
}

/// <summary>
/// <para>���� ���¸� �����ϴ� ������ ������</para>
/// <para>Editor ���¿��� ���̶�Ű�� ��ġ�Ǵ� Class�鿡�� ����� ��.</para>
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
