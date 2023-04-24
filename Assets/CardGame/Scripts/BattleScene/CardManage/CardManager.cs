using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CardPoolManager),typeof(ParticlePoolManager))]
public class CardManager : MonoBehaviour
{
    [Header("셋팅")]
    [Tooltip("카드 생성 및 비활성 관리 스크립트")]
    [SerializeField] CardPoolManager cardPoolManager;
    [SerializeField] ParticlePoolManager particlePoolManager;
    [Tooltip("잔여 마나 표시 UI")]
    [SerializeField] TextMeshProUGUI manaTMP;
    [Tooltip("턴 시작 시, 카드 접근 차단 이미지")]
    [SerializeField] GameObject blockImg;

    [Header("카드 관련 변수")]
    [Tooltip("시작 시 획득할 카드 개수")]
    [SerializeField] int startCard = 5;
    [Tooltip("매 턴 획득할 카드 개수")]
    [SerializeField] int turnCard = 1;
    [Tooltip("가질 수 있는 최대 카드 개수")]
    [SerializeField] int maxCard = 10;
    List<SOCard> cardList;

    [Header("마나 관련 변수")]
    [Tooltip("시작 시 획득할 마나")]
    [SerializeField] int startMana = 5;
    [Tooltip("매턴 획득할 마나")]
    [SerializeField] int turnMana = 1;
    int runtimeMana = 0;
    int runtimeCardCount = 0;

    private void Awake()
    {
        if (cardPoolManager == null) this.GetComponent<CardPoolManager>();
        if (particlePoolManager == null) this.GetComponent<ParticlePoolManager>();
        SetMana(startMana);
        blockImg.SetActive(false);
    }

    private void Start()
    {
        //Static Class인 CardStorage에 있는 모든 카드 데이터를 불러온다.
        cardList = CardStorage.GetAllCardDatas();

        foreach (var data in cardList)
        {
            if (data.ability.particle != null)
            {
                particlePoolManager.MakeParticlePool(data.ability.particle);
                CustomDebugger.Debug(LogType.Log, $"소유한 카드 : {data.name}");
            }
        }
        //cardList의 데이터에 기반하여 카드 생성
        MakeCard(startCard);
    }

    public void InvokeParticle(int id, Vector3 position)
    {
        particlePoolManager.InvokeParticle(id, position);
    }

    /// <summary>
    /// 현재는 Random값을 사용하여 카드를 생성한다.
    /// </summary>
    /// <param name="num">만들 카드 수</param>
    public void MakeCard(int num)
    {
        if (runtimeCardCount >= maxCard) return;

        for(int i=0; i < num; i++)
        {
            int randNum = Random.Range(0, cardList.Count);
            var cardData = cardList[randNum];
            var card = cardPoolManager.pool.Get();
            card.Setting(cardData,this,cardPoolManager.pool);

            runtimeCardCount++;
            if (runtimeCardCount >= maxCard) break;
        }
    }

    public void ReleaseCard()
    {
        runtimeCardCount--;
    }

    /// <summary>
    /// 현재 활성화되어 있는 카드의 수를 반환
    /// </summary>
    /// <returns></returns>
    public int GetAliveCardCount()
    {
        return runtimeCardCount;
    }

    public void OnGameState(EGameState gameState)
    {
        switch (gameState)
        {
            case EGameState.게임종료:
                blockImg.SetActive(true);
                break;
            case EGameState.턴시작:
                blockImg.SetActive(true);
                break;
            case EGameState.턴종료:
                NextTurn();
                break;
        }
    }

    public void NextTurn()
    {
        SetMana(turnMana);
        MakeCard(turnCard);
        blockImg.SetActive(false);
    }

    public int GetMana()
    {
        return runtimeMana;
    }

    /// <summary>
    /// 마나의 값 변경은 모두 SetMana를 통해서 이루어진다.
    /// </summary>
    /// <param name="value"></param>
    public void SetMana(int value)
    {
        runtimeMana += value;
        manaTMP.text = runtimeMana.ToString();
    }
}
