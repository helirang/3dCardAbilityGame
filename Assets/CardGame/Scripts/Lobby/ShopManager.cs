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

    private void Awake()
    {
        MoneyStorage.Load();
    }

    private void Start()
    {
        UpdateMoneyUI();
        foreach (var card in shopCardList)
        {
            bool isAlreadyHave = CardStorage.CheckSerialNum(card.serialNumber);

            ShopCard shopCard = Instantiate(shopCardPrefab,cardParent);
            shopCard.Setting(card,isAlreadyHave);
            shopCard.BuyEvent += OnItemSold;
            shopCard.MoneyLackEvent += OnMoneyLack;
        }
    }

    void UpdateMoneyUI()
    {
        moneyTMP.text = MoneyStorage.ZEM.ToString();
    }

    void OnItemSold()
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
