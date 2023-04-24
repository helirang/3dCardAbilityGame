using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    int activeCardCount = 0;

    private void Awake()
    {
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
        int cardIdx = activeCardCount;
        card.dragDrop.Setting(cardCanvas, parent, cardIdx);
        card.gameObject.SetActive(true);
        activeCardCount++;
    }

    private void OnReleaseCard(GameCard card)
    {
        card.gameObject.SetActive(false);
        activeCardCount--;
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
