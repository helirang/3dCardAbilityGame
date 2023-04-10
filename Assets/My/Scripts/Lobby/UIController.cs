using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Button closeBtn;

    protected virtual void Awake()
    {
        closeBtn.onClick.AddListener(()=>this.gameObject.SetActive(false));
    }

    protected virtual void OnDestroy()
    {
        closeBtn.onClick.RemoveAllListeners();
    }
}
