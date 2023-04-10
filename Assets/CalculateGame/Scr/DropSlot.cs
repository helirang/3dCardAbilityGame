using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    TextMeshProUGUI tmp;
    public int slotNum = 0;
    public CharacterController target;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if(eventData.pointerDrag != null)
        {
            GameCard gameCard = eventData.pointerDrag.gameObject.GetComponent<GameCard>();
            if (target != null)
            {
                //ICardTargetable ICardTargetable = target.GetComponent<ICardTargetable>();
                //gameCard.Active(ICardTargetable);
            }
            else
            {
                Debug.LogWarning("None Target");
            }
        }
    }

}
