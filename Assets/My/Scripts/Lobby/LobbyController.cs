using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
   enum UINum
    {
        Game,
        Inventory,
        Shop,
        Option
    }

    [SerializeField]List<Canvas> canvas;
    [SerializeField]List<Button> activeBtns;

    private void Awake()
    {
        for (int i = 0; i < activeBtns.Count; i++)
        {
            int value = i;
            activeBtns[i].onClick.AddListener(() => ActiveCanvas((UINum)value));
        }
    }
    void ActiveCanvas(UINum activeNum)
    {
        canvas[(int)activeNum].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach(var btn in activeBtns)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
