using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCard : MonoBehaviour
{
    [SerializeField] SOCard cardData;

    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI descTMP;
    [SerializeField] TextMeshProUGUI costTMP;

    CardManager cardManager;

    public void Setting(SOCard data, CardManager cardManager)
    {
        this.cardManager = cardManager;
        this.cardData = data;
        cardImage.sprite = cardData.sprite;
        descTMP.text = cardData.GetDesc();
        costTMP.text = cardData.cost.ToString();
    }

    public void Active(ICardTargetable target, ETeamNum targetTeam)
    {
        if (TeamCheck(targetTeam))
        {
            cardManager.SetMana(-cardData.cost);
            cardData.ability.Active(target);
        }
    }

    public ETeamNum GetTargetTeam()
    {
        return cardData.targetTeam;
    }

    bool TeamCheck(ETeamNum targetTeam)
    {
        return this.cardData.targetTeam == targetTeam;
    }

    public bool CostCheck()
    {
        bool result = this.cardData.cost <= cardManager.GetMana();
        if(result == false)
        {
            //여기서 카드 매니저의 마나 부족 팝업 호출하기
        }

        return result;
    }
}
