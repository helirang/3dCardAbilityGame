using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Ability_DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject character;
    ICardTargetable target;
    public ETeamNum teamNum = ETeamNum.User;

    private void Awake()
    {
        target = character.GetComponent<ICardTargetable>();
    }

    public void Setting(ETeamNum teamNum)
    {
        this.teamNum = teamNum;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        GameCard gameCard = eventData.pointerDrag.gameObject.GetComponent<GameCard>();

        if (gameCard != null && target != null)
        {
            gameCard.Active(target, teamNum);
        }
        else
        {
            Debug.LogWarning("None Target Or Not Found GameCard");
        }
    }

    public bool TeamCheck(ETeamNum teamNum)
    {
        Debug.Log($"My Team : {this.teamNum} / Injection Team : {teamNum}");
        return this.teamNum == teamNum;
    }
}
