using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCard : MonoBehaviour
{
    [SerializeField] SOCard cardData;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI abilityTMP;
    [SerializeField] TextMeshProUGUI costTMP;

    CardManager cardManager;

    public void Setting(SOCard data, CardManager cardManager)
    {
        this.cardManager = cardManager;
        this.cardData = data;
        itemImage.sprite = cardData.sprite;
        abilityTMP.text = cardData.GetDesc();
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
            //���⼭ ī�� �Ŵ����� ���� ���� �˾� ȣ���ϱ�
        }

        return result;
    }
}
