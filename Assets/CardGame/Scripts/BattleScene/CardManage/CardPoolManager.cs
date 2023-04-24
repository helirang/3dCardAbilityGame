using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CardPoolManager : MonoBehaviour
{
    public IObjectPool<GameCard> pool;

    [Header("����")]
    [Tooltip("ī�� ������")]
    [SerializeField] GameCard cardPrefab;
    [Tooltip("�����Ǵ� ī�尡 ���� Ʈ������")]
    [SerializeField] Transform parent;
    [Tooltip("�����Ǵ� ī�尡 ���� ĵ����")]
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
        //ī���� ������ �Ҵ��Ѵ�.
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
