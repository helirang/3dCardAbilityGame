using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(CardManager))]
public class CardPoolManager : MonoBehaviour
{
    public IObjectPool<GameCard> pool;

    [Header("셋팅")]
    [Tooltip("카드 프리팹")]
    [SerializeField] GameCard cardPrefab;
    [Tooltip("생성되는 카드가 속할 트랜스폼")]
    [SerializeField] Transform parent;
    [Tooltip("생성되는 카드가 속할 캔버스")]
    [SerializeField] Canvas cardCanvas;
    [Tooltip("카드 매니저 ( 카드 비활성화 전달 )")]
    [SerializeField] CardManager cardManager;

    private void Awake()
    {
        if (cardManager == null) cardManager = this.GetComponent<CardManager>();
        pool = new ObjectPool<GameCard>(CreateCard, OnGetCard,
            OnReleaseCard, OnDestroyCard, maxSize: 10);
    }

    GameCard CreateCard()
    {
        GameCard card = Instantiate(cardPrefab, parent);
        return card;
    }

    private void OnGetCard(GameCard card)
    {
        //카드의 순번을 할당한다.
        int cardIdx = cardManager.GetAliveCardCount();
        card.dragDrop.Setting(cardCanvas, parent, pool, cardIdx);

        card.gameObject.SetActive(true);
    }

    private void OnReleaseCard(GameCard card)
    {
        card.gameObject.SetActive(false);
        cardManager.ReleaseCard();
    }

    private void OnDestroyCard(GameCard card)
    {
        if (card != null)
            Destroy(card.gameObject);
    }

    private void OnDestroy()
    {
        pool.Clear();
    }
}
