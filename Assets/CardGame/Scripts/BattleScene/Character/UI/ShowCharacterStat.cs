using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCharacterStat : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    bool isActive = false;
    [SerializeField]CardCharacterController cardCharacter;
    //UI â �����

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            isActive = true;
            // ���� â Ȱ��ȭ
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isActive)
        {
            // ���� â ��Ȱ��ȭ
        }
    }
}
