using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using TMPro;

[RequireComponent(typeof(DragDrop))]
public class GameCard : MonoBehaviour
{
    [SerializeField] SOCard cardData;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI abilityTMP;
    [SerializeField] TextMeshProUGUI costTMP;
    public DragDrop dragDrop;

    IObjectPool<GameCard> cardPool;

    //@todo ������ �����ϴ°� ���� ���ΰ� ����� ���.
    CardManager cardManager;

    bool isActive = false;

    private void Awake()
    {
        if (dragDrop == null) dragDrop = this.GetComponent<DragDrop>();
    }

    public void Setting(SOCard data, CardManager cardManager, IObjectPool<GameCard> pool)
    {
        this.cardManager = cardManager;
        this.cardData = data;
        this.cardPool = pool;
        itemImage.sprite = cardData.sprite;
        abilityTMP.text = cardData.GetDesc();
        costTMP.text = cardData.cost.ToString();

        isActive = false;
    }

    public void Active(ICardTargetable target)
    {
        if (TeamCheck(target.GetTeamNum()))
        {
            cardManager.SetMana(-cardData.cost);
            cardData.ability.Active(target);
            isActive = true;
        }
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public ETeamNum GetTargetTeam()
    {
        return cardData.targetTeam;
    }

    bool TeamCheck(ETeamNum targetTeam)
    {
        return this.cardData.targetTeam == targetTeam;
    }

    public void ReleaseCard()
    {
        cardPool.Release(this);
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
