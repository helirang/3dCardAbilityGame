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
    public int slotSign = -1;
    public CalGameManager calGm;

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if(eventData.pointerDrag != null)
        {
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            SetText(eventData.pointerDrag.gameObject.name);
            Debug.Log(eventData.pointerDrag.gameObject.name);
            slotSign = GetSymbolNum(eventData.pointerDrag.gameObject.name);
            calGm.VerifyStart(slotNum, slotSign);
        }
    }

    private void SetText(string symbol)
    {
        tmp.fontSize = 100;
        switch (symbol)
        {
            case "Plus":
                tmp.text = "+";
                break;
            case "Minus":
                tmp.text = "-";
                break;
            case "Multiply":
                tmp.text = "x";
                break;
            case "Divide":
                tmp.text = "÷";
                break;
        }
    }

    private int GetSymbolNum(string symbol)
    {
        int signNum = 0;
        switch (symbol)
        {
            case "Plus":
                signNum = 0;
                break;
            case "Minus":
                signNum = 1;
                break;
            case "Multiply":
                signNum = 2;
                break;
            case "Divide":
                signNum = 3;
                break;
        }
        return signNum;
    }
}
