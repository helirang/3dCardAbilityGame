using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] List<SOCard> shopCardList;
    [SerializeField] GameObject popupObj;
    [SerializeField] ShopCard shopCardPrefab;
    [SerializeField] RectTransform cardParent;

    [SerializeField] TextMeshProUGUI moneyTMP;

    private void Start()
    {
        UpdateMoneyUI();
        foreach (var card in shopCardList)
        {
            bool isAlreadyHave = CardStorage.CheckSerialNum(card.serialNumber);

            ShopCard shopCard = Instantiate(shopCardPrefab,cardParent);
            shopCard.Setting(card,isAlreadyHave);
            shopCard.BuyEvent += OnCardSold;
            shopCard.MoneyLackEvent += OnMoneyLack;
        }
    }

    void UpdateMoneyUI()
    {
        moneyTMP.text = MoneyStorage.zem.ToString();
    }

    void OnCardSold()
    {
        UpdateMoneyUI();
    }

    /// <summary>
    /// ��ȭ�� ������ ��, ȣ��Ǵ� �Լ�
    /// </summary>
    void OnMoneyLack()
    {
        //��ȭ ���� �˸� �˾�
    }
}
