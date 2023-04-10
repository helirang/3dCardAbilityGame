using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndterLoading_ByButton : EnterLoading
{
    [SerializeField] Button enterBtn;

    private void Awake()
    {
        enterBtn.onClick.AddListener(Enter);
    }

    private void OnDestroy()
    {
        enterBtn.onClick.RemoveAllListeners();
    }
}
