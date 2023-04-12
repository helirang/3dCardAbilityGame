using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ability_DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Image dropDetecter;
    ICardTargetable target;

    public void Setting(ICardTargetable target)
    {
        this.target = target;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameCard gameCard = eventData.pointerDrag.gameObject.GetComponent<GameCard>();

        if (gameCard != null && target != null)
        {
            gameCard.Active(target);
        }
        else
        {
            CustomDebugger.Debug(LogType.LogWarning, "None Target Or Not Found GameCard");
        }
    }
}
