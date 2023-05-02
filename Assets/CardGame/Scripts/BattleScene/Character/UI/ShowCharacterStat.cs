using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCharacterStat : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    bool isActive = false;
    [SerializeField]CardCharacterController cardCharacter;
    //UI 창 만들기

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            isActive = true;
            // 스탯 창 활성화
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isActive)
        {
            // 스탯 창 비활성화
        }
    }
}
