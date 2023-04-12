using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(CardManager))]
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
    [Tooltip("ī�� �Ŵ��� ( ī�� ��Ȱ��ȭ ���� )")]
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
        //ī���� ������ �Ҵ��Ѵ�.
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
