using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCard : MonoBehaviour
{
    [SerializeField] SOCard cardData;
    [SerializeField] Button button;
    [SerializeField] Image soldOut;
    [SerializeField] Image itemImg;
    [SerializeField] TextMeshProUGUI priceTMP;
    [SerializeField] TextMeshProUGUI abilityTMP;
    public event System.Action BuyEvent;
    public event System.Action MoneyLackEvent;

    /// <summary>
    /// </summary>
    /// <param name="card">카드 데이터</param>
    /// <param name="isSoldOut">해당 카드를 구매한 적이 있으면 true</param>
    public void Setting(SOCard card, bool isSoldOut)
    {
        cardData = card;
        priceTMP.text = card.price.ToString();
        abilityTMP.text = card.GetDesc();
        itemImg.sprite = card.sprite;

        if (isSoldOut)
        {
            button.interactable = false;
            soldOut.gameObject.SetActive(true);
        }
        else
        {
            button.interactable = true;
            button.onClick.AddListener(BuyCard);
            soldOut.gameObject.SetActive(false);
        }
    }

    public void BuyCard()
    {
        if(MoneyStorage.zem >= cardData.price)
        {
            soldOut.gameObject.SetActive(true);
            bool isAlreadyHave = CardStorage.CheckSerialNum(cardData.serialNumber);
            if (!isAlreadyHave)
            {
                MoneyStorage.zem -= cardData.price;
                CardStorage.AddCard(cardData);
                BuyEvent.Invoke();
                button.interactable = false;
            }
        }
        else
        {
            MoneyLackEvent.Invoke();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        BuyEvent = null;
        MoneyLackEvent = null;
    }
}
